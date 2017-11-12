using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets._2D;
using UnityEngine.Events;

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
        [Header("Links")]
        public HealthBar MyHealthBar;
        public DialogueAnimator MyDialogue;
        public GameObject MyOutline;
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D MyRigidbody2D;
        private MinionControl2D MyControl;
        private Shooter MyShooter;
        private FoW.FogOfWarUnit MyFog;

        [Header("Stats")]
        public int Level = 1;
        public float Health = 5;
        public float HealthRegenRate = 4f;
        public float HealthRegenValue = 0f;
        public float AttackDamage = 1f;
        public float AttackSpeed = 1f;
        public float Vision = 2f;
        public float Range = 2f;
        [HideInInspector]
        public float MaxHealth = 5;
        public float MaxSpeed = 1f;   // The fastest the player can travel in the xy axis.

        [Header("Options")]
        public bool IsDestroyOnZeroHealth = true;
        public bool IsShowHealthBar = true;

        [HideInInspector]
        public bool CanMove = true;
        [HideInInspector]
        public CharacterDirection Direction = CharacterDirection.Down;
        private float MoveChangeThreshold = 0.5f;
        private float LastTakenHit;
        private bool IsDying;
        private int KillCount = 0;
        [Header("Prefabs")]
        public GameObject HealthBarPrefab;
        [Header("Events")]
        public UnityEvent OnHealthUpdatedEvent;
        public UnityEvent OnHealthRegenUpdatedEvent;
        [Serializable]
        public class Character2DEvent : UnityEvent<Character2D> { }
        public Character2DEvent OnKilledCharacterEvent = new Character2DEvent();

        public int GetGoldCoinsDrop()
        {
            int GoldIncrease = Level % 3;
            return Mathf.FloorToInt(UnityEngine.Random.Range(GoldIncrease, 1.99f + GoldIncrease * 1.1f));
        }

        private void Awake()
        {
            m_Anim = GetComponent<Animator>();
            MyRigidbody2D = GetComponent<Rigidbody2D>();
            MyControl = GetComponent<MinionControl2D>();
            MyShooter = GetComponent<Shooter>();
            MyFog = GetComponent<FoW.FogOfWarUnit>();
            if (IsShowHealthBar)
            {
                if (MyHealthBar == null && HealthBarPrefab)
                {
                    GameObject HealthBarObject = Instantiate(HealthBarPrefab, transform);
                    HealthBarObject.transform.localPosition = new Vector3(0, 0.4f, 0);
                    MyHealthBar = HealthBarObject.GetComponent<HealthBar>();
                    HideCharacterInFog MyHideInFog = transform.GetChild(0).GetComponent<HideCharacterInFog>();
                    if (MyHideInFog)
                    {
                        MyHideInFog.MyHealthBar = MyHealthBar;
                    }
                }
                MyHealthBar.gameObject.SetActive(true);
                MyHealthBar.SetHealthBars(Health);
            }
            MaxHealth = Health;
            OnUpdatedAttackSpeed();
            OnUpdatedRange();
            OnUpdatedVision();
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
            if (Time.timeScale != 0)
            {
                RegenerateHealth();
            }
        }

        private void RegenerateHealth()
        {
            if (HealthRegenRate != 0  && HealthRegenValue  != 0
                && Time.time - LastTakenHit >= HealthRegenRate)
            {
                LastTakenHit = Time.time;
                if (Health < MaxHealth)
                {
                    Health += HealthRegenValue;
                    Health = Mathf.Clamp(Health, 0, MaxHealth);
                    OnHealthUpdatedEvent.Invoke();
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

        #region AddStats

        public void AddMaxHealth(float HealthMaxAdd)
        {
            MaxHealth += HealthMaxAdd;
            Health += HealthMaxAdd;
            OnHealthUpdatedEvent.Invoke();
            MyHealthBar.SetHealthBars(MaxHealth);
        }

        public void AddHealthRegen(float HealthRegenValueAdd)
        {
            HealthRegenValue += HealthRegenValueAdd;
            OnHealthRegenUpdatedEvent.Invoke();
        }

        public void AddDamage(float DamageIncrease)
        {
            AttackDamage += DamageIncrease;
        }

        public void AddAttackSpeed(float Addition)
        {
            AttackSpeed += Addition;
            OnUpdatedAttackSpeed();
        }
        public void AddVision(float Addition)
        {
            Vision += Addition;
            OnUpdatedVision();
        }

        public void AddRange(float Addition)
        {
            Range += Addition;
            OnUpdatedRange();
        }
        #endregion

        #region UpdatedStats
        private float Size = 1f;

        /// <summary>
        /// Level up a character and randomly assign stats based on level
        /// </summary>
        public void SetLevel(int NewLevel)
        {
            Level = NewLevel;
            MaxHealth = 1 + Level / 4;
            Health = MaxHealth;
            MyHealthBar.SetHealthBars(Health);
        }

        public void SetSize(float NewSize)
        {
            Size = NewSize;
            if (GetComponent<RandomizeSize>())
            {
                GetComponent<RandomizeSize>().AlterSize(Size);
            }
        }

        public void SetMovementSpeed(float NewSpeed)
        {
            MaxSpeed = NewSpeed;
        }

        public void OnUpdatedVision()
        {
            if (MyFog)
            {
                MyFog.radius = Vision;
            }
        }

        public void OnUpdatedAttackSpeed()
        {
            if (MyShooter)
            {
                MyShooter.FireRate = 1 / AttackSpeed;
            }
        }
        public void OnUpdatedRange()
        {
            if (MyShooter)
            {
                MyShooter.FireRange = Range;
            }
        }
        #endregion

        public void GetHit(Character2D MyCharacter, float Damage = -1)
        {
            if (Damage == -1)
            {
                Damage = MyCharacter.AttackDamage;
            }
            LastTakenHit = Time.time;
            Health -= Damage;
            Health = Mathf.Clamp(Health, 0, MaxHealth);
            OnHealthUpdatedEvent.Invoke();
            if (MyHealthBar)
            {
                MyHealthBar.SetBars(Health);
            }
            if (MyControl)
            {
                MyControl.OnHit(MyCharacter);
            }
            if (Health == 0 && IsDestroyOnZeroHealth && !IsDying)
            {
                IsDying = true;
                MapData.Instance.SpawnedCharacters.Remove(gameObject);
                MyHealthBar.SetVisibility(false);
                SetMovement(false);
                ShrinkDeath MyDeath = gameObject.AddComponent<ShrinkDeath>();
                MyDeath.SetDeathTime(0.4f);
                MyCharacter.OnKilledCharacter(this);
            }
        }

        public void OnKilledCharacter(Character2D DeadCharacter)
        {
            KillCount++;
            OnKilledCharacterEvent.Invoke(DeadCharacter);
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
