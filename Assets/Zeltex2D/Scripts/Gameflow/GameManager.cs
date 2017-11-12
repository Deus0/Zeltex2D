using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zeltex2D.TowerDefence;

namespace Zeltex2D
{
    /// <summary>
    /// Manages the game, from start to end
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public GameObject LevelFaderPrefab;
        private GameObject LevelFader;
        private CanvasFader MyFader;
        public static GameManager Instance;
        private bool IsGameOvering;
        public TowerBuilder MyTowerBuilder;
        public WaveSpawner MySpawner;
        public CanvasGroup ExplorationGui;

        private void Awake()
        {
            LevelFader = GameObject.Instantiate(LevelFaderPrefab);
            Instance = this;
        }

        private void Start()
        {
            if (MyTowerBuilder && MySpawner)
            {
                StartCoroutine(SpawnInTime());
            }
        }

        private IEnumerator SpawnInTime()
        {
            Vector3 SpawnPosition = MyTowerBuilder.transform.position;
            yield return new WaitForSeconds(0.5f);
            Character2D SpawnedCharacter = MyTowerBuilder.SpawnTower(SpawnPosition).GetComponent<Character2D>();
            yield return new WaitForSeconds(4f);
            ExplorationGui.interactable = true;
            MyTowerBuilder.OnBeginGame();
            MyTowerBuilder.SelectTower(SpawnedCharacter);
            MySpawner.OnBeginGame();
        }

        public void GameOver()
        {
            if (!IsGameOvering)
            {
                IsGameOvering = true;
                StartCoroutine(GameOverRoutine());
            }
        }
        
        private IEnumerator GameOverRoutine()
        {
            MyFader = LevelFader.GetComponent<CanvasFader>();
            MyFader.SetText("Game Over");
            MyFader.ReverseFade();
            yield return new WaitForSeconds(4f);
            //MyFader.Fade();
            //yield return new WaitForSeconds(2f);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

}