using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimeWitch
{
    public class LevelRestarter : MonoBehaviour
    {
        public GameObject MainCharacter;
        private bool IsRestarting;
        public bool IsMainMenu;
        public string MainMenuSceneName = "MainMenu";
        public float TimeToRestart = 3f;

        public void Update()
        {
            if (!IsRestarting && MainCharacter == null)
            {
                IsRestarting = true;
                StartCoroutine(RestartLevelRoutine());
            }
        }

        public IEnumerator RestartLevelRoutine()
        {
            yield return new WaitForSeconds(TimeToRestart);
            RestartLevel();
        }

        public void RestartLevel()
        {
            if (IsMainMenu)
            {
                SceneManager.LoadScene(MainMenuSceneName);
            }
            else
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

        public void SwitchLevel(string LevelName)
        {
            SceneManager.LoadScene(LevelName);
        }
    }
}