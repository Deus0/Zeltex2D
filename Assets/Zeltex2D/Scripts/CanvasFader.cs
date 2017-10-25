using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    public class CanvasFader : MonoBehaviour
    {
        public Text FaderText;
        private Animator MyAnimator;

        private void Awake()
        {
            MyAnimator = GetComponent<Animator>();
        }

        public void SetText(string NewText)
        {
            FaderText.text = NewText;
        }

        public void SetText(bool IsText)
        {
            FaderText.enabled = IsText;
        }

        public void ReverseFade()
        {
            MyAnimator.SetBool("IsReverseFade", true);
            MyAnimator.SetTrigger("Play");
        }

        public void Fade()
        {
            MyAnimator.SetBool("IsReverseFade", false);
            MyAnimator.SetTrigger("Play");
        }
    }

}