using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class TowerScanner : MonoBehaviour
    {
        private float LastScanTime;
        private float ScanCooldown = 0.2f;
        private MinionControl2D MyController;
        private Character2D ClosestTower;
        // Use this for initialization
        void Start()
        {
            MyController = GetComponent<MinionControl2D>();
               LastScanTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time - LastScanTime >= ScanCooldown && !MyController.IsAttacking())
            {
                LastScanTime = Time.time;
                ClosestTower = MapData.Instance.GetClosestIshWithTag("Tower", transform.position, 2);
                if (ClosestTower != null)
                {
                    MyController.Attack(ClosestTower.transform);
                }
            }
        }
    }

}