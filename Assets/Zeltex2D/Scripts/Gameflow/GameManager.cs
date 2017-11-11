using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    /// <summary>
    /// Manages the game, from start to end
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public GameObject LevelFaderPrefab;

        private void Awake()
        {
            GameObject LevelFader = GameObject.Instantiate(LevelFaderPrefab);
        }

    }

}