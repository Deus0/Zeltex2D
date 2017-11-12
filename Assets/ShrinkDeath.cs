using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class ShrinkDeath : MonoBehaviour
    {
        private float DeathTime = 0.2f;
        private float TimeStartedDying;
        private Vector3 StartScale;

        private void Awake()
        {
            StartScale = transform.localScale;
            TimeStartedDying = Time.time;
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x / 4f, GetComponent<Rigidbody2D>().velocity.y / 4f);
            GetComponent<Rigidbody2D>().mass = GetComponent<Rigidbody2D>().mass * 4f;
        }

        public void SetDeathTime(float NewTime)
        {
            DeathTime = NewTime;
        }

        private void Update()
        {
            transform.localScale = Vector3.Lerp(StartScale, Vector3.zero, (Time.time - TimeStartedDying) / DeathTime);
            if (Time.time - TimeStartedDying >= DeathTime)
            {
                Destroy(gameObject);
            }
        }
    }

}