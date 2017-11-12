using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private void Awake()
        {
            LevelFader = GameObject.Instantiate(LevelFaderPrefab);
            Instance = this;
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