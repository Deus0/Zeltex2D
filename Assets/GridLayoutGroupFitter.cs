using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    public class GridLayoutGroupFitter : MonoBehaviour
    {
        int ScreenWidth = 1920;
        int ScreenHeight = 1080;
        public int OriginalWidth = 768;
        public int OriginalHeight = 270;
        public Vector2 OriginalCellSize = new Vector2(120, 120);
        public int OriginalSpacing = 20;
        GridLayoutGroup MyGrid;


        // Use this for initialization
        void Start()
        {
            MyGrid = GetComponent<GridLayoutGroup>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Screen.width != ScreenWidth
                || Screen.height != ScreenHeight)
            {
                ScreenWidth = Screen.width;
                ScreenHeight = Screen.height;
                Vector2 RectWidthRatio = new Vector2(
                    (float)GetComponent<RectTransform>().rect.size.x / (float)OriginalWidth,
                    (float)GetComponent<RectTransform>().rect.size.y / (float)OriginalHeight);
                MyGrid.cellSize = new Vector2(Mathf.RoundToInt(OriginalCellSize.x * RectWidthRatio.x), Mathf.RoundToInt(OriginalCellSize.y * RectWidthRatio.y));
                MyGrid.spacing = new Vector2(Mathf.RoundToInt(OriginalSpacing * RectWidthRatio.x), Mathf.RoundToInt(OriginalSpacing * RectWidthRatio.y));
            }
        }
    }

}