using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class Shooter : MonoBehaviour
    {
        public GameObject BulletPrefab;
        public float FireRate = 0.5f;
        public float FireForce = 20;
        public bool DoShoot;
        private float FireSpawnOffset = 0.4f;
        private float LastFired;
        private Character2D MyCharacter;
        public float FireRange = 3f;

        // Use this for initialization
        void Start()
        {
            LastFired = Time.time;
            MyCharacter = GetComponent<Character2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (DoShoot)
            {
                DoShoot = false;
                ShootAtMinion();
            }
            if (Time.time - LastFired >= FireRate)
            {
                LastFired = Time.time;
                ShootAtMinion();
            }
        }

        private void ShootAtMinion()
        {
            Character2D Minion = MapData.Instance.GetClosestWithTag("Minion", transform.position, FireRange);
            if (Minion)
            {
                // Check that minion is in fog!
                Vector2 BulletDirection = (Minion.transform.position - transform.position).normalized;
                GameObject MyBullet = Instantiate(BulletPrefab, transform.position + FireSpawnOffset * (new Vector3(BulletDirection.x, BulletDirection.y, 0)), Quaternion.Euler(BulletDirection.x, BulletDirection.y, 0));
                Rigidbody2D BulletPhysics = MyBullet.GetComponent<Rigidbody2D>();
                BulletPhysics.AddForce(BulletDirection * FireForce);
                MyBullet.GetComponent<Bullet>().ShooterCharacter = MyCharacter;
            }
        }
    }

}