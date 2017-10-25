using UnityEngine;
using SlimeWitch;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Zeltex2D
{
    public class LevelProgressor : MonoBehaviour
    {
        private bool IsProgressing;

        void OnTriggerEnter2D(Collider2D other)
        {
            Character2D PossibleUser = other.gameObject.GetComponent<Character2D>();
            if (!IsProgressing && PossibleUser && PossibleUser.name == "MrPenguin")
            {
                Debug.Log("Progressing " + PossibleUser.name);
                IsProgressing = true;
                StartCoroutine(ProgressLevelRoutine());
            }
        }

        private IEnumerator ProgressLevelRoutine()
        {
            yield return new WaitForSeconds(1f);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

}