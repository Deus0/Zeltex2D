using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class PauseButton : MonoBehaviour
    {
        UnityEngine.UI.Text PauseText;

        private void Awake()
        {
            PauseText = transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>();
        }

        public void TogglePause()
        {
            if (PauseText.text == "Pause")
            {
                Time.timeScale = 0;
                PauseText.text = "UnPause";
            }
            else
            {
                Time.timeScale = 1;
                PauseText.text = "Pause";
            }
        }
    }

}