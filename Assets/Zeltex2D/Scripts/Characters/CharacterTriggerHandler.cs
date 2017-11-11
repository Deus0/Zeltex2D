using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class CharacterTriggerHandler : MonoBehaviour
    {
        //public CharacterDirection MyDirection;
        public UserControl2D TargetCharacter;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (TargetCharacter)
            {
                //Debug.LogError(other.gameObject.name + " has entered " + transform.parent.name);
                Character2D TriggeredCharacter = other.gameObject.GetComponent<Character2D>();
                if (TriggeredCharacter)
                {
                    TargetCharacter.SelectCharacter(TriggeredCharacter);
                    return;
                }
                Item TriggeredItem = other.gameObject.GetComponent<Item>();
                if (TriggeredItem)
                {
                    TargetCharacter.SelectItem(TriggeredItem);
                }
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (TargetCharacter)
            {
                Character2D TriggeredCharacter = other.gameObject.GetComponent<Character2D>();
                if (TriggeredCharacter)
                {
                    TargetCharacter.DeSelectCharacter(TriggeredCharacter);
                    return;
                }
                Item TriggeredItem = other.gameObject.GetComponent<Item>();
                if (TriggeredItem)
                {
                    TargetCharacter.DeSelectItem(TriggeredItem);
                }
            }
        }
    }
}