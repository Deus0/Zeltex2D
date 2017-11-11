using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D.UI
{
    [RequireComponent(typeof(Text))]
    public class SetLevelText : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Text MyText = GetComponent<Text>();
            MyText.text = "Level " + PlayerPrefs.GetInt("Level", 1);
        }
    }
}
