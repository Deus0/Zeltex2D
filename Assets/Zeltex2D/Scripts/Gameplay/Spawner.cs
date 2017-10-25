using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeWitch
{
    public class Spawner : MonoBehaviour
    {
        public GameObject Prefab;
        private List<GameObject> Spawned = new List<GameObject>();
        private float LastSpawnedTime;
        public float SpawnRate = 5f;
        public int MaxSpawns = 10;

        // Update is called once per frame
        void Update()
        {
            if (Time.time - LastSpawnedTime >= SpawnRate)
            {
                LastSpawnedTime = Time.time;
                for (int i = Spawned.Count - 1; i >= 0; i--)
                {
                    if (Spawned[i])
                    {
                        Spawned.RemoveAt(i);
                    }
                }
                if (Spawned.Count <= MaxSpawns)
                {
                    GameObject NewSpawn = Instantiate(Prefab);
                    NewSpawn.transform.position = transform.position;
                    Spawned.Add(NewSpawn);
                }

            }
        }
    }
}