using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    /// <summary>
    /// On collide with a minion, does damage
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        public Character2D ShooterCharacter;
        public float DeathTime = 1.5f;
        public float AttackDamage = 1;
        private bool WasHit = false;

        private void Start()
        {
            StartCoroutine(DieInTime());
        }

        private IEnumerator DieInTime()
        {
            yield return new WaitForSeconds(DeathTime);
            if (WasHit == false)
            {
                WasHit = true;
                gameObject.AddComponent<ShrinkDeath>();
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (!WasHit)
            {
                //Debug.LogError(other.gameObject.name + " has entered " + transform.parent.name);
                Character2D HitCharacter = other.gameObject.GetComponent<Character2D>();
                if (HitCharacter && HitCharacter.gameObject.tag == "Minion")
                {
                    WasHit = true;
                    gameObject.AddComponent<ShrinkDeath>();
                    HitCharacter.GetHit(ShooterCharacter, AttackDamage);
                }
            }
        }
    }
}