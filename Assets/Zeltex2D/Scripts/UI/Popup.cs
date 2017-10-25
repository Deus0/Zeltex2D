using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    public class Popup : MonoBehaviour
    {
        public Text PopupText;
        private float TimeStarted;
        private float TimeLength;
        private float HeightAddition;
        private float LerpValue;
        private Vector3 OriginalPosition;
        private Vector3 NewPosition;
        private Color OriginalColor;
        private Color NewColor;
        private Color InvisibleColor;

        public void Initialise(float StatValue, float DeathTime)
        {
            TimeLength = DeathTime;
            PopupText.text = StatValue.ToString();
            TimeStarted = Time.time;
            HeightAddition = Random.Range(1f, 2f);
            OriginalPosition = transform.position;
            NewPosition = transform.position;
            OriginalColor = PopupText.color;
            NewColor = PopupText.color;
            InvisibleColor = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, 0);
        }

        private void Update()
        {
            // Animate upwards
            LerpValue = (Time.time - TimeStarted) / TimeLength;
            NewPosition.Set(OriginalPosition.x, OriginalPosition.y + Mathf.Lerp(0, HeightAddition, LerpValue), OriginalPosition.z);
            transform.position = NewPosition;
            NewColor = Color.Lerp(OriginalColor, InvisibleColor, LerpValue);
            PopupText.color = NewColor;
        }
    }
}
