using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    [CreateAssetMenu(fileName = "Data", menuName = "Zeltex2D/DialogueData", order = 1)]
    public class DialogueData : ScriptableObject
    {
        [TextArea]
        public List<string> SpeechText = new List<string>();
    }
}