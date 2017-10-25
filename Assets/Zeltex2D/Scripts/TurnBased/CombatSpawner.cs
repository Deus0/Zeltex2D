using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeWitch;

namespace Zeltex2D.TurnBased
{
    public class CombatSpawner : MonoBehaviour
    {
        Character2D TeleportedCharacter;
        Transform OriginalParent;
        Vector3 OriginalPosition;

        public void Teleport(Character2D MyCharacter)
        {
            TeleportedCharacter = MyCharacter;
            OriginalParent = TeleportedCharacter.transform.parent;
            OriginalPosition = TeleportedCharacter.transform.position;
            TeleportedCharacter.transform.position = transform.position;
            TeleportedCharacter.transform.SetParent(transform);
            TeleportedCharacter.SetMovement(false);
            TeleportedCharacter.SetHealthBarVisibility(true);
            Transform SpriteTransform = TeleportedCharacter.transform.GetChild(0);
            FoW.HideInFog MyFog = SpriteTransform.GetComponent<FoW.HideInFog>();
            if (MyFog)
            {
                MyFog.enabled = false;
            }
            TeleportedCharacter.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }

        public void RevertTeleport()
        {
            if (TeleportedCharacter)
            {
                Transform SpriteTransform = TeleportedCharacter.transform.GetChild(0);
                FoW.HideInFog MyFog = SpriteTransform.GetComponent<FoW.HideInFog>();
                if (MyFog)
                {
                    MyFog.enabled = true;
                }
                TeleportedCharacter.transform.position = OriginalPosition;
                TeleportedCharacter.transform.parent = OriginalParent;
                //TeleportedCharacter.SetMovement(true);
                TeleportedCharacter.SetHealthBarVisibility(false);
            }
        }
    }
}
