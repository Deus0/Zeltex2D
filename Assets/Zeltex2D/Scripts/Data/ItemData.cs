using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    [CreateAssetMenu(fileName = "Data", menuName = "Zeltex2D/ItemData", order = 1)]
    public class ItemData : ScriptableObject
    {
        public Sprite MySprite;
    }
}