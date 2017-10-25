using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        // Use this for initialization
        protected virtual void Start()
        {
            if (target)
            {
                m_LastTargetPosition = target.position;
                m_OffsetZ = (transform.position - target.position).z;
            }
        }

        public void SetTarget(Transform newTarget, Vector3 OffsetPosition)
        {
            target = newTarget;
            if (target)
            {
                m_LastTargetPosition = target.position + OffsetPosition;
                m_OffsetZ = (transform.position - target.position).z;
                transform.position = target.position + OffsetPosition;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateCamera(Time.deltaTime);
        }

        protected void UpdateCamera(float deltaTime)
        {
            if (target)
            {
                Vector3 TargetPosition = GetTargetPosition(Time.deltaTime);
                TargetPosition = Vector3.SmoothDamp(transform.position, TargetPosition, ref m_CurrentVelocity, damping);
                transform.position = TargetPosition;

                m_LastTargetPosition = target.position; //transform.position;// 
            }
        }

        protected virtual Vector3 GetTargetPosition(float deltaTime)
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, deltaTime * lookAheadReturnSpeed);
            }

            Vector3 TargetPosition = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
            return TargetPosition;
        }
    }
}
