using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Zeltex2D
{

    public class FollowCamera : UnityStandardAssets._2D.Camera2DFollow
    {
        public Vector3 CameraPositionAddition;

        protected override void Start()
        {
            base.Start();
            if (target == null)
            {
                UserControl2D MyPlayer = GameObject.FindObjectOfType<UserControl2D>();
                if (MyPlayer)
                {
                    SetTarget(MyPlayer.transform, CameraPositionAddition);
                }
            }
            else
            {
                Debug.LogError("Cannot find UserControl2D in scene.");
            }
        }

        protected override Vector3 GetTargetPosition(float deltaTime)
        {
            Vector3 TargetPosition = base.GetTargetPosition(deltaTime);
            TargetPosition += CameraPositionAddition;
            return TargetPosition;
        }
    }
}