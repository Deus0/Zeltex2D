using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class CameraZoom2D : MonoBehaviour
    {
        public float MinSize = 3;
        public float MaxSize = 10;
        public float ScrollSpeed = 0.1f;
        private Camera MyCamera;

        // Use this for initialization
        void Start()
        {
            MyCamera = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            var d = Input.GetAxis("Mouse ScrollWheel");
            if (d > 0f)
            {
                // scroll up
                MyCamera.orthographicSize -= 0.1f;
                MyCamera.orthographicSize = Mathf.Clamp(MyCamera.orthographicSize, MinSize, MaxSize);
            }
            else if (d < 0f)
            {
                // scroll down
                MyCamera.orthographicSize += 0.1f;
                MyCamera.orthographicSize = Mathf.Clamp(MyCamera.orthographicSize, MinSize, MaxSize);
            }

        }
    }

}