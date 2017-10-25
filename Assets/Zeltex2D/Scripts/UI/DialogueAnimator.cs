using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    public class DialogueAnimator : MonoBehaviour
    {
        public DialogueData Data;
        public Text DialogueText;
        [TextArea]
        public string FinalText = "";
        private string CurrentText;
        private float TypeSpeed = 0.1f;
        private float LastTypedTime;
        private int TypedCount = 0;
        private bool IsSpeaking;
        public bool IsSpeakOnStart;
        private System.Action OnCompleteSpeaking;
        [TextArea]
        public List<string> AllTheTexts = new List<string>();
        private int TextIndex = 0;
        private bool IsSpeakingDismissed = true;

        void Start()
        {
            DialogueText.gameObject.SetActive(false);
            if (IsSpeakOnStart)
            {
                Speak();
            }
        }

        private void ResetTextAnimation()
        {
            CurrentText = "";
            DialogueText.text = CurrentText;
            LastTypedTime = Time.time;
            TypedCount = 0;
            IsSpeaking = true;
        }
        public void Speak(System.Action NewOnCompleteSpeaking = null)
        {
            // Begin Speaking
            if (IsSpeakingDismissed)
            {
                DialogueText.gameObject.SetActive(true);
                transform.GetChild(0).gameObject.SetActive(true);
                IsSpeakingDismissed = false;
                OnCompleteSpeaking = NewOnCompleteSpeaking;
                TextIndex = 0;
                if (AllTheTexts.Count > 0)
                {
                    FinalText = AllTheTexts[TextIndex];
                }
                ResetTextAnimation();
            }
            // Finish speaking
            else if (!IsSpeakingDismissed && !IsSpeaking)
            {
                if (TextIndex < AllTheTexts.Count)
                {
                    Debug.LogError("Finished Speaking");
                    TextIndex++;
                    FinalText = AllTheTexts[TextIndex];
                    ResetTextAnimation();
                }
                else
                {
                    IsSpeakingDismissed = true;
                    if (OnCompleteSpeaking != null)
                    {
                        OnCompleteSpeaking();
                    }
                    DialogueText.text = "";
                    DialogueText.gameObject.SetActive(false);
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        public void StopSpeak()
        {
            DialogueText.text = "";
            DialogueText.gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
            if (OnCompleteSpeaking != null)
            {
                OnCompleteSpeaking();
                OnCompleteSpeaking = null;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (IsSpeaking &&
                Time.time - LastTypedTime >= TypeSpeed)
            {
                LastTypedTime = Time.time;
                TypeSpeed = Random.Range(0.08f, 0.12f);
                CurrentText = FinalText.Substring(0, TypedCount + 1);
                DialogueText.text = CurrentText;
                TypedCount++;
                if (TypedCount == FinalText.Length)
                {
                    DialogueText.text = FinalText;
                    IsSpeaking = false;
                }
                TypedCount = Mathf.Clamp(TypedCount, 0, FinalText.Length - 1);
            }
        }
    }
}