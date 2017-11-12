using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

namespace Zeltex2D
{
    [RequireComponent(typeof (Character2D))]
    public class UserControl2D : MonoBehaviour
    {
        public Inventory MyInventory;
        public GameObject Slime;
        public Skillbar MySkillbar;
        public Camera MyCamera;
        protected Character2D MyCharacter;
        protected bool m_Jump;
        protected Character2D SelectedCharacter;
        protected Item SelectedItem;
        protected bool CanInput = true;

        private void Awake()
        {
            MyCharacter = GetComponent<Character2D>();
        }

        public void TeleportCamera()
        {
            if (MyCamera == null)
            {
                MyCamera = Camera.main;
            }
            MyCamera.transform.position =
                new Vector3(transform.position.x, transform.position.y, MyCamera.transform.position.z);
        }

        protected virtual void FixedUpdate()
        {
            // Read the inputs.
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            if (!CanInput)
            {
                h = 0;
                v = 0;
            }
            // Pass all parameters to the character control script.
            MyCharacter.Move(h, v);
        }

        protected virtual void Update()
        {
            if (MyCharacter.CanMove)
            {
                if ((Input.GetKeyDown(KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Submit")))//Input.GetKeyDown(KeyCode.E))
                {
                    if (SelectedCharacter)
                    {
                        ActionSpeak();
                    }
                    else if (SelectedItem)
                    {
                        ActionPickupItem();
                    }
                }
                if (CanInput)
                {
                    if (CrossPlatformInputManager.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Q))
                    {
                        if (Slime)
                        {
                            Slime.SetActive(!Slime.activeSelf);
                            if (Slime.activeSelf)
                            {
                                // teleport around me
                                Slime.GetComponent<MinionControl2D>().Teleport();
                            }
                        }
                    }
                    if (CrossPlatformInputManager.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.I))
                    {
                        MyInventory.Toggle();
                    }
                    if (MySkillbar)
                    {
                        if (CrossPlatformInputManager.GetButtonDown("Right Bumper") || Input.GetAxis("Mouse ScrollWheel") > 0f)
                        {
                            MySkillbar.IncreaseSkillSelection();
                        }
                        if (CrossPlatformInputManager.GetButtonDown("Left Bumper") || Input.GetAxis("Mouse ScrollWheel") < 0f)
                        {
                            MySkillbar.DecreaseSkillSelection();
                        }
                        if (CrossPlatformInputManager.GetButtonDown("Attack") || Input.GetKeyDown(KeyCode.F))
                        {
                            if (MySkillbar.CurrentTargetedSkill == 0)
                            {
                                Slime.GetComponent<MinionControl2D>().Follow();
                            }
                            else if (MySkillbar.CurrentTargetedSkill == 1)
                            {
                                Slime.GetComponent<MinionControl2D>().AttackClosest();
                            }
                            else if (MySkillbar.CurrentTargetedSkill == 2)
                            {
                                Slime.GetComponent<MinionControl2D>().WanderAndDestroy();
                            }
                        }
                    }
                }
            }
        }

        private void DeselectAll()
        {
            DeSelectCharacter(SelectedCharacter);
            DeSelectItem(SelectedItem);
        }
        #region CharacterInteraction

        public void ActionSpeak()
        {
            if (SelectedCharacter && SelectedCharacter.MyDialogue)
            {
                CanInput = false;
                SelectedCharacter.MyDialogue.Speak(
                    () =>
                    {
                        CanInput = true;
                    });
            }
        }

        public virtual void SelectCharacter(Character2D NewSelectedCharacter)
        {
            if (NewSelectedCharacter && NewSelectedCharacter.MyDialogue)
            {
                SelectedCharacter = NewSelectedCharacter;
                SelectedCharacter.Glow();
            }
        }

        public virtual void DeSelectCharacter(Character2D NewSelectedCharacter)
        {
            if (NewSelectedCharacter == SelectedCharacter && SelectedCharacter != null)
            {
                SelectedCharacter.RemoveGlow();
                SelectedCharacter.MyDialogue.StopSpeak();
                SelectedCharacter = null;
            }
        }

        #endregion

        #region ItemInteraction

        public void ActionPickupItem()
        {
            if (SelectedItem && SelectedItem.CanPickup)
            {
                MyInventory.AddItem(SelectedItem.Data);
                SelectedItem.PickedUp();
                SelectedItem = null;
            }
        }

        public void SelectItem(Item NewItem)
        {
            if (NewItem)
            {
                SelectedItem = NewItem;
                SelectedItem.Glow();
            }
        }

        public void DeSelectItem(Item OldSelectedItem)
        {
            if (OldSelectedItem == SelectedItem && SelectedItem != null)
            {
                SelectedItem.RemoveGlow();
                SelectedItem = null;
            }
        }

        #endregion
    }
}
