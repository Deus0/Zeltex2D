using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    /// <summary>
    /// a basic zone for spawning characters
    /// </summary>
    public class SpawnZone : MonoBehaviour
    {
        public bool DoSpawn;
        [Header("Options")]
        public GameObject CharacterPrefab;
        public float TimePerSpawnMin = 0.3f;
        public float TimePerSpawnMax = 1f;
        //public int MinionsToSpawn = 3;
        private float LastSpawnedMinion;
        private float InitialDelay = 0f;
        private BoxCollider2D MyBox;
        [Header("Links")]
        public WaveSpawner MyWaveSpawner;
        public Text WavesText;
        public int SpawnLevel = 1;
        // Spawning
        private bool IsSpawning;
        private List<GameObject> Spawns = new List<GameObject>();
        public int MinionsLeftToSpawn = 0;
        private float TimePerMinionSpawn;
        private int MaxMinionsToSpawn;

        private void Start()
        {
            MyBox = GetComponent<BoxCollider2D>();
            //LastSpawnedMinions = Time.time - InitialDelay;
            TimePerMinionSpawn = Random.Range(TimePerSpawnMin, TimePerSpawnMax);
        }

        public bool IsDefending()
        {
            for (int i = Spawns.Count - 1; i >= 0; i--)
            {
                if (Spawns[i] == null)
                {
                    Spawns.RemoveAt(i);
                }
            }
            // once spawns are dead
            return (Spawns.Count != 0);
        }

        public void SetMinionsToSpawn(int NewMinionsMax)
        {
            MaxMinionsToSpawn = NewMinionsMax;
            MinionsLeftToSpawn = MaxMinionsToSpawn;
        }

        // Update is called once per frame
        void Update()
        {
            if (DoSpawn)
            {
                DoSpawn = false;
                //SpawnMinions();
            }
            if (IsSpawning)
            {
                WavesText.text = "Wave " + SpawnLevel + "\nSpawning " + Mathf.RoundToInt(MaxMinionsToSpawn - MinionsLeftToSpawn) + "/" + MaxMinionsToSpawn;
                if (Time.time - LastSpawnedMinion >= TimePerMinionSpawn)
                {
                    BeginSpawningMinion();
                }
            }
        }


        public void BeginSpawningMinion()
        {
            MinionsLeftToSpawn--;
            LastSpawnedMinion = Time.time;
            TimePerMinionSpawn = Random.Range(TimePerSpawnMin, TimePerSpawnMax);
            if (SpawnLevel % 5 == 0)
            {
                SpawnBossMinion();
                MinionsLeftToSpawn = 0;
            }
            else
            {
                SpawnMinion();
            }
            if (MinionsLeftToSpawn == 0)
            {
                IsSpawning = false;
                //LastSpawnedMinions = Time.time;
                WavesText.text = "Wave " + SpawnLevel + "\nDefending";
            }
        }

        public void BeginSpawning()
        {
            TimePerMinionSpawn = Random.Range(TimePerSpawnMin, TimePerSpawnMax);
            LastSpawnedMinion = Time.time - TimePerMinionSpawn;
            IsSpawning = true;
            BeginSpawningMinion();
        }

        /*public void SpawnMinions()
        {
            StartCoroutine(SpawnMinionsRoutine());
        }

        public IEnumerator SpawnMinionsRoutine()
        {
            yield return new WaitForSeconds(InitialDelay);
            SpawnMinion();
        }*/

        public void SpawnMinion()
        {
            Vector3 SpawnPosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
            SpawnPosition.z = 0;
            GameObject MySpawn = Instantiate(CharacterPrefab, GetRandomPosition(), Quaternion.identity, transform);
            MapData.Instance.SpawnedCharacters.Add(MySpawn);
            Spawns.Add(MySpawn);
            Character2D MyCharacter = MySpawn.GetComponent<Character2D>();
            MyCharacter.SetLevel(SpawnLevel);
        }

        public void SpawnBossMinion()
        {
            Vector3 SpawnPosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
            SpawnPosition.z = 0;
            GameObject MySpawn = Instantiate(CharacterPrefab, GetRandomPosition(), Quaternion.identity, transform);
            MapData.Instance.SpawnedCharacters.Add(MySpawn);
            Spawns.Add(MySpawn);
            Character2D MyCharacter = MySpawn.GetComponent<Character2D>();
            MyCharacter.SetLevel(Mathf.RoundToInt(SpawnLevel * MaxMinionsToSpawn * 4f));
            MyCharacter.SetSize(2.5f);
            MyCharacter.SetMovementSpeed(0.5f);
            Generators.TextureGenerator MyGenerator = MySpawn.transform.GetChild(0).GetComponent<Generators.TextureGenerator>();
            MyGenerator.IsAddOutline = true;
            if (MyGenerator.HasStarted)
            {
                Debug.LogError("Regenerating boss sprite.");
                MyGenerator.GenerateSprite();
            }
        }

        public Vector3 GetRandomPosition()
        {
            return transform.position + new Vector3(
                Random.Range(-MyBox.size.x / 2f, MyBox.size.x / 2f),
                Random.Range(-MyBox.size.y / 2f, MyBox.size.y / 2f), 0);// for now
        }
    }

}