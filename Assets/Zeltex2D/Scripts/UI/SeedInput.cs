using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    public class SeedInput : MonoBehaviour
    {
        private InputField MyInput;
        public static string SeedDefault = "PenguinSeed";
        public static string SeedKey = "Seed";

        private void Start()
        {
            MyInput = GetComponent<InputField>();
            MyInput.text = PlayerPrefs.GetString(SeedKey, SeedDefault);
            PlayerPrefs.SetInt("Level", 1);
        }

        public void InputSeed(string NewSeed)
        {
            PlayerPrefs.SetString(SeedKey, NewSeed);
            PlayerPrefs.Save();
        }
    }

}