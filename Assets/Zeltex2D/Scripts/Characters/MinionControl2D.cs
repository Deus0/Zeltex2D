using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

namespace Zeltex2D
{
    public enum MinionBehaviour
    {
        Idle,
        Wander,
        Follow,
        Charge,
        Attack,
        SeekTarget
    }
    [RequireComponent(typeof (Character2D))]
    public class MinionControl2D : MonoBehaviour
    {
        public Transform FollowTransform;
        public MinionBehaviour MyBehaviour;
        public Transform AttackTransform;
        public float MaxAttackDistance = 4f;
        private Character2D m_Character;
        private bool m_Jump;
        private Vector2 WanderValue = Vector2.zero;
        private float WanderAddition = 0.2f;
        private float DistanceToTarget;
        private Vector2 MyPosition;
        private Vector2 TargetPosition;
        private Vector2 InputMovement = Vector2.zero;
        private float MinimumDistanceToTarget = 0.3f;
        private float LastTimeAttacked;
        // Seek
        private float LastSeekedTarget;
        // Attack
        private MinionBehaviour BeforeAttackBehaviour;
        private bool IsInAttackRange;
        private float AttackRate = 1f;
        private float MinimumAttackDistance = 0.2f;
        private Character2D MyCharacter;
        private float WhiskersCheckTime = 1f;
        public LayerMask BricksLayer;
        private float LastWhiskerTime;
        private CircleCollider2D MyCircleCollider;
        private BoxCollider2D MyBoxCollider;

        private void Awake()
        {
            m_Character = GetComponent<Character2D>();
            WanderValue = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            if (MyCircleCollider == null)
            {
                MyCircleCollider = GetComponent<CircleCollider2D>();
            }
            if (MyBoxCollider == null)
            {
                MyBoxCollider = GetComponent<BoxCollider2D>();
            }
            MyCharacter = GetComponent<Character2D>();
            WanderValue.Normalize();
        }

        public void ToggleAttack()
        {
            if (MyBehaviour == MinionBehaviour.Attack)
            {
                StopAttacking();
            }
            else
            {
                Attack();
            }
        }

        public void Wander()
        {
            MinimumDistanceToTarget = 0f;
            MyBehaviour = MinionBehaviour.Wander;
        }

        public void WanderAndDestroy()
        {
            MinimumDistanceToTarget = 0f;
            MyBehaviour = MinionBehaviour.SeekTarget;
        }

        public void Attack()
        {
            if (MyBehaviour != MinionBehaviour.Attack)
            {
                MinionControl2D[] MyObjects = GameObject.FindObjectsOfType<MinionControl2D>();
                float ClosestDistance = MaxAttackDistance;
                MinionControl2D ClosestObject = null;
                for (int i = 0; i < MyObjects.Length; i++)
                {
                    if (MyObjects[i] != this)
                    {
                        float ThisDistance = Vector3.Distance(transform.position, MyObjects[i].transform.position);
                        if (ThisDistance < ClosestDistance)
                        {
                            ClosestDistance = ThisDistance;
                            ClosestObject = MyObjects[i];
                        }
                    }
                }
                if (ClosestObject != null)
                {
                    MinimumDistanceToTarget = 0f;
                    BeforeAttackBehaviour = MyBehaviour;
                    MyBehaviour = MinionBehaviour.Attack;
                    AttackTransform = ClosestObject.transform;
                }
                else
                {
                    // keep following?! or wandering
                }
            }
        }

        public void StopAttacking()
        {
            MinimumDistanceToTarget = 0.3f;
            if (BeforeAttackBehaviour == MinionBehaviour.Attack)
            {
                Debug.LogError("BAD: " + BeforeAttackBehaviour.ToString());
                MyBehaviour = MinionBehaviour.Wander;
            }
            else
            {
                MyBehaviour = BeforeAttackBehaviour;
            }
        }

        public void Follow()
        {
            MinimumDistanceToTarget = 0.6f;
            MyBehaviour = MinionBehaviour.Follow;
        }

        private void FixedUpdate()
        {
            if (MyBehaviour == MinionBehaviour.Follow)
            {
                FollowTarget();
            }
            else if (MyBehaviour == MinionBehaviour.Attack)
            {
                AttackTarget();
            }
            else if (MyBehaviour == MinionBehaviour.Wander)
            {
                UpdateWander();
            }
            else if (MyBehaviour == MinionBehaviour.SeekTarget)
            {
                UpdateWander();
                if (Time.time - LastSeekedTarget >= 2f)
                {
                    LastSeekedTarget = Time.time;
                    Attack();
                }
            }
            else if (MyBehaviour == MinionBehaviour.Idle)
            {
                InputMovement.Set(0, 0);
            }
            // Pass all parameters to the character control script.
            m_Character.Move(InputMovement.x, InputMovement.y);
        }

        public void OnHit(Character2D MyCharacter)
        {
            if (MyCharacter && MyBehaviour != MinionBehaviour.Attack)
            {
                MinimumDistanceToTarget = 0f;
                BeforeAttackBehaviour = MyBehaviour;
                MyBehaviour = MinionBehaviour.Attack;
                AttackTransform = MyCharacter.transform;
            }
        }

        private void AttackTarget()
        {
            FollowTarget();
            if (AttackTransform == null)
            {
                StopAttacking();
                return;
            }
            float TimeSinceAttacked = Time.time - LastTimeAttacked;
            bool CanAttack = TimeSinceAttacked >= AttackRate;
            // If in range, change Behaviour to charge
            if (DistanceToTarget <= MinimumAttackDistance + GetRadius() * transform.localScale.x + GetEnemyRadius(AttackTransform))
            {
                if (!IsInAttackRange || CanAttack)
                {
                    IsInAttackRange = true;
                    LastTimeAttacked = Time.time;
                    AttackTransform.GetComponent<Character2D>().GetHit(MyCharacter);
                }
            }
            else
            {
                if (CanAttack)
                {
                    LastTimeAttacked = Time.time;
                    IsInAttackRange = false;
                }
            }
        }

        private float GetEnemyRadius(Transform EnemyTransform)
        {
            CircleCollider2D EnemyCollider = EnemyTransform.GetComponent<CircleCollider2D>();
            if (EnemyCollider)
            {
                return EnemyCollider.radius * EnemyTransform.transform.localScale.x;
            }

            BoxCollider2D EnemyBoxCollider = EnemyTransform.GetComponent<BoxCollider2D>();
            if (EnemyBoxCollider)
            {
                return ((EnemyBoxCollider.bounds.size.x + EnemyBoxCollider.bounds.size.y) / 2f) * EnemyTransform.transform.localScale.x;
            }
            return 0;
        }

        private float GetRadius()
        {
            if (MyCircleCollider)
            {
                return MyCircleCollider.radius * transform.localScale.x;
            }
            else if (MyBoxCollider)
            {
                return ((MyBoxCollider.bounds.size.x + MyBoxCollider.bounds.size.y) / 2f) *transform.localScale.x;
            }
            else
            {
                return 0;
            }
        }

        private void FollowTarget()
        {
            if (MyBehaviour == MinionBehaviour.Attack)
            {
                TargetTransform = AttackTransform;
                if (AttackTransform)
                {
                    TargetPosition.Set(AttackTransform.position.x, AttackTransform.position.y);
                }
                else
                {
                    transform.localScale = transform.localScale * 1.05f;
                    StopAttacking();
                }
            }
            else
            {
                TargetTransform = FollowTransform;
                if (FollowTransform)
                {
                    TargetPosition.Set(FollowTransform.position.x, FollowTransform.position.y);
                }
                else
                {
                    Wander();
                }
            }

            if (Time.time - LastWhiskerTime >= WhiskersCheckTime)
            {
                LastWhiskerTime = Time.time;
                WhiskersCheckTime = Random.Range(0.1f, 0.4f);
                if (Physics2D.CircleCast(transform.position, 2f, InputMovement.normalized, 2f, BricksLayer))
                {
                    //Debug.LogError("Near Wall");
                    RandomizeWander(0.5f);
                    InputMovement.Set(InputMovement.x + WanderValue.x, InputMovement.y + WanderValue.y);
                }
            }
            MyPosition.Set(transform.position.x, transform.position.y);
            DistanceToTarget = Vector2.Distance(MyPosition, TargetPosition);
            EnemyRadius = GetEnemyRadius(TargetTransform);
            MyRadius = GetRadius();
            if (EnemyRadius == 0)
            {
                MyRadius = 0;
            }
            if (MinimumDistanceToTarget == 0 && MyRadius == 0)
            {
                MyRadius = 0.03f;
            }
            if (DistanceToTarget > MinimumDistanceToTarget + MyRadius + EnemyRadius)
            {
                InputMovement = (TargetPosition - MyPosition).normalized;
            }
            else
            {
                InputMovement.Set(0, 0);
                HasReachedTarget = true;
            }
        }

        private float MyRadius;
        private Transform TargetTransform;
        private float EnemyRadius;
        private bool HasReachedTarget = false;

        public System.Collections.IEnumerator WalkToTarget(Transform NewTarget, float NewMinimumDistance)
        {
            bool CouldMove = MyCharacter.CanMove;
            float OldMinimumDistance = MinimumDistanceToTarget;

            MinimumDistanceToTarget = NewMinimumDistance;
            MyCharacter.SetMovement(true);
            FollowTransform = NewTarget;
            MyBehaviour = MinionBehaviour.Follow;
            HasReachedTarget = false;
            while (!HasReachedTarget)
            {
                yield return new WaitForEndOfFrame();
            }
            MyCharacter.SetMovement(CouldMove);
            MinimumDistanceToTarget = OldMinimumDistance;
        }

        private void RandomizeWander(float WanderMax = 1f)
        {
            WanderValue.x += Random.Range(-WanderAddition, WanderAddition);
            WanderValue.y += Random.Range(-WanderAddition, WanderAddition);
            WanderValue.x = Mathf.Clamp(WanderValue.x, -WanderMax, WanderMax);
            WanderValue.y = Mathf.Clamp(WanderValue.y, -WanderMax, WanderMax);
        }
        private void UpdateWander()
        {
            RandomizeWander();
            InputMovement.Set(WanderValue.x, WanderValue.y);
        }

        public void Teleport()
        {
            TargetPosition.Set(FollowTransform.position.x, FollowTransform.position.y);
            MyPosition.Set(transform.position.x, transform.position.y);
            Vector2 NormalizedPosition = (MyPosition - TargetPosition).normalized;
            transform.position = new Vector3(
                TargetPosition.x + NormalizedPosition.x,
                TargetPosition.y + NormalizedPosition.y,
                transform.position.z);
        }
    }
}
