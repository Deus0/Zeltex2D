using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets._2D;

namespace Zeltex2D
{
    public enum CharacterDirection
    {
        Down,
        Up,
        Left,
        Right,
        None
    }

    /// <summary>
    /// A character for 2d implementation
    /// </summary>
    public class Character2D : MonoBehaviour
    {
        public static bool IsDestroyOnZeroHealth = false;

        [Header("Links")]
        public HealthBar MyHealthBar;
        public DialogueAnimator MyDialogue;
        public GameObject MyOutline;
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D MyRigidbody2D;
        private MinionControl2D MyControl;

        [Header("Stats")]
        public int Health = 5;
        public float MaxSpeed = 1f;   // The fastest the player can travel in the xy axis.
        public float HealthRegenRate = 4f;
        private int MaxHealth = 5;
        [Header("Options")]
        public bool IsShowHealthBar = true;

        [HideInInspector]
        public bool CanMove = true;
        [HideInInspector]
        public CharacterDirection Direction = CharacterDirection.Down;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private float MoveChangeThreshold = 0.5f;
        private float LastTakenHit;

        private void Awake()
        {
            m_Anim = GetComponent<Animator>();
            MyRigidbody2D = GetComponent<Rigidbody2D>();
            MyControl = GetComponent<MinionControl2D>();
            if (MyHealthBar && IsShowHealthBar)
            {
                MyHealthBar.SetHealthBars(Health);
            }
            MaxHealth = Health;
        }

        public void SetHealthBarVisibility(bool NewHealthBarState)
        {
            if (MyHealthBar)
            {
                MyHealthBar.SetVisibility(NewHealthBarState);
                MyHealthBar.SetHealthBars(Health);
            }
        }

        void Update()
        {
            if (HealthRegenRate != 0)
            {
                RegenerateHealth();
            }
        }

        private void RegenerateHealth()
        {
            if (Time.time - LastTakenHit >= HealthRegenRate)
            {
                LastTakenHit = Time.time;
                if (Health < MaxHealth)
                {
                    Health++;
                    if (MyHealthBar)
                    {
                        MyHealthBar.SetBars(Health);
                    }
                    else
                    {
                        Debug.Log(name + " does not have a health bar");
                    }
                }
            }
        }

        public void SetMovement(bool NewMovementState)
        {
            if (MyRigidbody2D)
            {
                MyRigidbody2D.isKinematic = !NewMovementState;
            }
            if (CanMove && NewMovementState == false)
            {
                Move(0, 0);
            }
            /*UserControl2D MyControl = GetComponent<UserControl2D>();
            if (MyControl)
            {
                MyControl.enabled = NewMovementState;
            }*/
            CanMove = NewMovementState;
        }

        public void GetHit(Character2D MyCharacter, int Damage = 1)
        {
            LastTakenHit = Time.time;
            Health -= Damage;
            if (MyHealthBar)
            {
                MyHealthBar.SetBars(Health);
            }
            if (MyControl)
            {
                MyControl.OnHit(MyCharacter);
            }
            if (Health == 0 && IsDestroyOnZeroHealth)
            {
                Destroy(gameObject);
            }
        }

        public void Glow()
        {
            if (MyOutline)
            {
                MyOutline.SetActive(true);
            }
        }
        public void RemoveGlow()
        {
            if (MyOutline)
            {
                MyOutline.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            // Set the vertical animation
            if (m_Anim && MyRigidbody2D)
            {
                m_Anim.SetFloat("vSpeed", MyRigidbody2D.velocity.y);
            }
            else if (MyRigidbody2D == null)
            {
                Debug.LogError(name + " does not have rigidbody");
            }
        }

        public void Move(float horizontalMove, float verticalMove)
        {
            if (CanMove)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                //horizontalMove = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(horizontalMove + verticalMove));

                // Move the character
                MyRigidbody2D.velocity = new Vector2(horizontalMove * MaxSpeed, verticalMove * MaxSpeed);

                // Already Walking
                if ((int)Direction >= 0)
                {
                    AlreadyWalkingSetAnimations(horizontalMove, verticalMove);
                }
                // Start Walking
                else
                {
                    SetNormalDirections(horizontalMove, verticalMove);
                }
            }
        }

        private void AlreadyWalkingSetAnimations(float horizontalMove, float verticalMove)
        {
            // Stopped Direction was previously walking
            if ((int)Direction >= 0 && (int)Direction <= 1)
            {
                if (verticalMove >= -MoveChangeThreshold && verticalMove <= MoveChangeThreshold)
                {
                    SetNormalDirections(horizontalMove, verticalMove);
                }
                else
                {
                    // Vertical Movement only
                    if (verticalMove > 0)
                    {
                        Direction = CharacterDirection.Up;
                        m_Anim.SetInteger("Direction", (int)Direction);
                    }
                    else if (verticalMove < 0)
                    {
                        Direction = CharacterDirection.Down;
                        m_Anim.SetInteger("Direction", (int)Direction);
                    }
                }
            }
            else if ((int)Direction >= 2)
            {
                if (horizontalMove >= -MoveChangeThreshold && horizontalMove <= MoveChangeThreshold)
                {
                    SetNormalDirections(horizontalMove, verticalMove);
                }
                else
                {
                    // Horizontal Movement only
                    if (horizontalMove > 0)
                    {
                        Direction = CharacterDirection.Right;
                        m_Anim.SetInteger("Direction", (int)Direction);
                    }
                    else if (horizontalMove < 0)
                    {
                        Direction = CharacterDirection.Left;
                        m_Anim.SetInteger("Direction", (int)Direction);
                    }
                }
            }
            // Keep walking as normal
            else
            {
                //Direction = CharacterDirection.None;
                //m_Anim.SetInteger("Direction", (int)Direction);
            }
        }

        public void FaceDirection(Vector3 NormalizedDirection)
        {
            SetNormalDirections(NormalizedDirection.x, NormalizedDirection.y);
        }
        private void SetNormalDirections(float horizontalMove, float verticalMove)
        {
            if (horizontalMove > 0)
            {
                Direction = CharacterDirection.Right;
                m_Anim.SetInteger("Direction", (int)Direction);
            }
            else if (horizontalMove < 0)
            {
                Direction = CharacterDirection.Left;
                m_Anim.SetInteger("Direction", (int)Direction);
            }
            else if (verticalMove > 0)
            {
                Direction = CharacterDirection.Up;
                m_Anim.SetInteger("Direction", (int)Direction);
            }
            else if (verticalMove < 0)
            {
                Direction = CharacterDirection.Down;
                m_Anim.SetInteger("Direction", (int)Direction);
            }
            else
            {
                //Direction = CharacterDirection.None;
                //m_Anim.SetInteger("Direction", (int)Direction);
            }
        }
    }
}
