using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public float InitialDelay = 5f;
        public float TimePerSpawnMin = 0.2f;
        public float TimePerSpawnMax = 0.4f;
        private float TimePerMinionSpawn;
        public float TimePerWave = 10;
        public int MinionsToSpawn = 10;
        private float LastSpawnedMinions;
        private List<GameObject> Spawns = new List<GameObject>();
        private bool IsSpawning;
        private float LastSpawnedMinion;
        private int MinionsLeftToSpawn = 0;
        private BoxCollider2D MyBox;

        private void Start()
        {
            MyBox = GetComponent<BoxCollider2D>();
            LastSpawnedMinions = Time.time - InitialDelay;
            TimePerMinionSpawn = Random.Range(TimePerSpawnMin, TimePerSpawnMax);
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
                if (Time.time - LastSpawnedMinion >= TimePerMinionSpawn)
                {
                    MinionsLeftToSpawn--;
                    LastSpawnedMinion = Time.time;
                    TimePerMinionSpawn = Random.Range(TimePerSpawnMin, TimePerSpawnMax);
                    SpawnMinion();
                    if (MinionsLeftToSpawn == 0)
                    {
                        IsSpawning = false;
                        LastSpawnedMinions = Time.time;
                        MinionsToSpawn++;
                    }
                }
            }
            else
            {
                for (int i = Spawns.Count-1; i >= 0; i--)
                {
                    if (Spawns[i] == null)
                    {
                        Spawns.RemoveAt(i);
                    }
                }
                if (Spawns.Count == 0 && Time.time - LastSpawnedMinions >= TimePerWave)
                {
                    LastSpawnedMinions = Time.time;
                    IsSpawning = true;
                    LastSpawnedMinion = Time.time;
                    TimePerMinionSpawn = Random.Range(TimePerSpawnMin, TimePerSpawnMax);
                    MinionsLeftToSpawn = MinionsToSpawn;
                }
            }
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
        }

        public Vector3 GetRandomPosition()
        {
            return transform.position + new Vector3(
                Random.Range(-MyBox.size.x / 2f, MyBox.size.x / 2f),
                Random.Range(-MyBox.size.y / 2f, MyBox.size.y / 2f), 0);// for now
        }
    }

}