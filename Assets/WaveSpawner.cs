using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    public class WaveSpawner : MonoBehaviour
    {
        private bool HasBegunGame = false;
        public List<SpawnZone> MySpawners;
        private SpawnZone CurrentSpawner;
        private int WaveCount = 0;
        public Text WavesText;
        public int MinionsToSpawn = 3;
        private float LastSpawnedMinions;
        public float TimePerWave = 10;

        private void Start()
        {
            OnUpdatedWaveCount();
        }

        private void Update()
        {
            if (HasBegunGame)
            {
                if (CurrentSpawner)
                {
                    if (!CurrentSpawner.IsDefending())
                    {
                        CurrentSpawner = null;
                        LastSpawnedMinions = Time.time;
                    }
                }
                else
                {
                    IncreaseWave();
                }
            }
        }

        public void OnBeginGame()
        {
            HasBegunGame = true;
        }

        public void OnUpdatedWaveCount()
        {
            WavesText.text = "Wave " + WaveCount;
        }

        private void IncreaseWave()
        {
            float TimeSinceLastWave = (Time.time - LastSpawnedMinions);
            if (TimeSinceLastWave >= TimePerWave)
            {
                LastSpawnedMinions = Time.time;
                OnWaveIncrease();
            }
            else
            {
                WavesText.text = "Wave " + WaveCount + "\nNext Wave " + Mathf.CeilToInt(TimePerWave - TimeSinceLastWave) + "s";
            }
        }

        private void OnWaveIncrease()
        {
            // Either increase number, or increase their health
            //MinionsToSpawn++;
            if (WaveCount % 5 == 0)
            {
                MinionsToSpawn++;
            }
            WaveCount++;
            OnUpdatedWaveCount();
            EnableSpawning(Random.Range(0, MySpawners.Count - 1));
        }

        private void EnableSpawning(int SpawnerIndex)
        {
            for (int i = 0; i < MySpawners.Count; i++)
            {
                MySpawners[SpawnerIndex].enabled = false;
            }
            CurrentSpawner = MySpawners[SpawnerIndex];
            CurrentSpawner.enabled = true;
            CurrentSpawner.SpawnLevel = WaveCount;
            CurrentSpawner.SetMinionsToSpawn(MinionsToSpawn);
            CurrentSpawner.BeginSpawning();
        }
    }

}