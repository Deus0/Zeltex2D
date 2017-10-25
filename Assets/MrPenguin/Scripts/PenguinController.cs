using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;
using SlimeWitch;
using Zeltex2D;
using Zeltex2D.TurnBased;
using System.Collections.Generic;

namespace MrPenguin
{
    [RequireComponent(typeof (Character2D))]
    public class PenguinController : UserControl2D
    {
        public CombatArea MyCombatArea;

        public override void SelectCharacter(Character2D NewSelectedCharacter)
        {
            base.SelectCharacter(NewSelectedCharacter);
            if (MyCharacter && MyCharacter.CanMove && NewSelectedCharacter)
            {
                Debug.Log(name + " is now battling with " + NewSelectedCharacter.name);
                // Start a combat area
                if (MyCombatArea)
                {
                    List<Character2D> InvolvedCharacters = new List<Character2D>();
                    InvolvedCharacters.Add(MyCharacter);
                    InvolvedCharacters.Add(NewSelectedCharacter);
                    MyCombatArea.EnterCombat(InvolvedCharacters);
                }
            }
        }
    }
}
