using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    /// <summary>
    /// Generate Minimap
    /// </summary>
    [ExecuteInEditMode, RequireComponent(typeof(MapData))]
    public class MinimapGenerator : MonoBehaviour
    {
        public Image MapImage;
        public Image FogImage;
        public List<Color> TileColours;
        private MapData Data;

        private void Awake()
        {
            Data = GetComponent<MapData>();
        }

        public void GenerateMap()
        {
            // Create new texture
            Texture2D NewTexture = new Texture2D(Data.MapWidth, Data.MapHeight, TextureFormat.ARGB32, false);
            NewTexture.filterMode = FilterMode.Point;
            for (int i = 0; i < Data.MapWidth; i++)
            {
                for (int j = 0; j < Data.MapHeight; j++)
                {
                    // set pixel
                }
            }

        }

        public void GenerateFog()
        {

        }
    }

}