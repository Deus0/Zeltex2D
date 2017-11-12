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
        private Character2D TargetMinion;
        public LineRenderer AimLine;

        // Use this for initialization
        void Start()
        {
            LastFired = Time.time;
            MyCharacter = GetComponent<Character2D>();
            AimLine.SetPositions(new Vector3[] { transform.position, transform.position + 0.3f * (new Vector3(0, 1f, 0)) });
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
            if (TargetMinion == null)
            {
                TargetMinion = MapData.Instance.GetClosestWithTag("Minion", transform.position, FireRange);
            }
            if (TargetMinion && FoW.FogOfWar.current.IsInFog(TargetMinion.transform.position + new Vector3(-0.5f, -0.5f, 0), 0.2f))
            {
                TargetMinion = null;
            }
            if (TargetMinion)
            {
                // Check that minion is in fog!
                Vector2 BulletDirection = (TargetMinion.transform.position - transform.position).normalized;
                GameObject MyBullet = Instantiate(BulletPrefab, transform.position + FireSpawnOffset * (new Vector3(BulletDirection.x, BulletDirection.y, 0)), Quaternion.Euler(BulletDirection.x, BulletDirection.y, 0));
                Rigidbody2D BulletPhysics = MyBullet.GetComponent<Rigidbody2D>();
                BulletPhysics.AddForce(BulletDirection * FireForce);
                Bullet MyBulletComponent = MyBullet.GetComponent<Bullet>();
                MyBulletComponent.ShooterCharacter = MyCharacter;
                MyBulletComponent.AttackDamage = MyCharacter.AttackDamage;
                MyBulletComponent.DeathTime = MyCharacter.Range / 2f;
                AimLine.SetPositions(new Vector3[] { transform.position, transform.position + 0.3f * (new Vector3(BulletDirection.x, BulletDirection.y, 0)) });
            }
        }
    }

}