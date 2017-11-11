using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    /// <summary>
    /// Spawns an minimap icon
    /// </summary>
    public class MinimapIconSpawner : MonoBehaviour
    {
        public GameObject IconPrefab;
        private MinimapIcon MyIcon;
        private static string ParentName = "MinMapParent";

        private void Awake()
        {
            GameObject ParentIcons = GameObject.Find(ParentName);
            GameObject MinimapIconObject = Instantiate(IconPrefab, ParentIcons.transform);
            MyIcon = MinimapIconObject.GetComponent<MinimapIcon>();
            if (MyIcon)
            {
                MyIcon.Map = GameObject.Find("Level").GetComponent<MapData>();
                MyIcon.Target = transform;
                MyIcon.transform.SetSiblingIndex(2);
            }
            else
            {
                Destroy(MinimapIconObject);
            }
        }

        private void OnDestroy()
        {
            DestroyImmediate(MyIcon.gameObject);
        }
    }

}