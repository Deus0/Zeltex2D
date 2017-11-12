using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class RandomizeSize : MonoBehaviour
    {
        private float Variance = 0.4f;
        private float MySize = 1f;

        // Use this for initialization
        void Start()
        {
            AlterSize(MySize);
        }

        public void AlterSize(float NewSize = 1f)
        {
            MySize = NewSize;
            if (Random.Range(1, 11) > 9f)
            {
                transform.localScale = new Vector3(Random.Range(NewSize - Variance, NewSize + Variance), Random.Range(NewSize - Variance, NewSize + Variance), 1f);
            }
            else
            {
                float Scale = Random.Range(NewSize - Variance, NewSize + Variance);
                transform.localScale = new Vector3(Scale, Scale, 1f);
            }
        }

    }

}