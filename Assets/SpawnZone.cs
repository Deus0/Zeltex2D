using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    /// <summary>
    /// a basic zone for spawning characters
    /// </summary>
    [ExecuteInEditMode]
    public class SpawnZone : MonoBehaviour
    {
        public bool DoSpawn;
        [Header("Options")]
        public GameObject CharacterPrefab;
        public float InitialDelay = 0.5f;
        public float TimePerSpawnMin = 0.2f;
        public float TimePerSpawnMax = 0.4f;
        public int MinionsToSpawn = 10;

        // Update is called once per frame
        void Update()
        {
            if (DoSpawn)
            {
                DoSpawn = false;
                SpawnMinions();
            }
        }

        public void SpawnMinions()
        {
            StartCoroutine(SpawnMinionsRoutine());
        }

        public IEnumerator SpawnMinionsRoutine()
        {
            yield return new WaitForSeconds(InitialDelay);
            SpawnMinion();
        }

        public void SpawnMinion()
        {
            Instantiate(CharacterPrefab, GetRandomPosition(), Quaternion.identity, transform);
        }

        public Vector3 GetRandomPosition()
        {
            return transform.position;// for now
        }
    }

}