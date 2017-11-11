using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class MinimapIcon : MonoBehaviour
    {
        public Transform Target;
        public MapData Map;
        private Vector3 LastPosition;

        private void Start()
        {
            if (Map == null)
            {
                Debug.LogError(name + " Has no assigned Map");
            }
            RefreshPosition(GetTargetGridPosition());
        }

        // Update is called once per frame
        void Update()
        {
            if (Target)
            {
                Vector3 TargetPosition = GetTargetGridPosition();
                if (!(TargetPosition.x != LastPosition.x && TargetPosition.y != LastPosition.y && TargetPosition.z != LastPosition.z))
                {
                    RefreshPosition(TargetPosition);
                }
            }
        }

        private Vector3 GetTargetGridPosition()
        {
            return new Vector3(Mathf.CeilToInt(Target.transform.position.x), Mathf.CeilToInt(Target.transform.position.y), Mathf.CeilToInt(Target.transform.position.z));
        }

        private void RefreshPosition(Vector3 NewPosition)
        {
            LastPosition = NewPosition;
            // position this icon inside of the rect
            Vector2 RectSize = transform.parent.gameObject.GetComponent<RectTransform>().rect.size;// new Vector2(270, 270);
            Vector2 MapSize = new Vector2(Map.MapWidth, Map.MapHeight);
            GetComponent<RectTransform>().anchoredPosition =
                new Vector3(
                        (LastPosition.x / MapSize.x) * RectSize.x,
                        (LastPosition.y / MapSize.y) * RectSize.y,
                        0);
        }
    }

}