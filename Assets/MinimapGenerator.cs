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
        public RawImage MapImage;
        public RawImage FogImage;
        public List<Color> TileColours;
        private MapData Data;

        private void Awake()
        {
            Data = GetComponent<MapData>();
            if (TileColours.Count == 0)
            {
                TileColours.Add(Color.black);
                TileColours.Add(Color.magenta);
                TileColours.Add(Color.green);
                TileColours.Add(Color.cyan);
            }
        }

        public void GenerateMap()
        {
            // Create new texture
            Texture2D NewTexture = new Texture2D(Data.MapWidth, Data.MapHeight, TextureFormat.ARGB32, false);
            NewTexture.filterMode = FilterMode.Point;
            Color[] Pixels = NewTexture.GetPixels();
            int TileIndex = 0;
            for (int i = 0; i < Data.MapWidth; i++)
            {
                for (int j = 0; j < Data.MapHeight; j++)
                {
                    // set pixel
                    TileIndex = Data.GetTileType(i, j);
                    if (TileIndex >= 0 && TileIndex < TileColours.Count)
                    {
                        Pixels[Mathf.FloorToInt(i + j * Data.MapWidth)] = TileColours[TileIndex];
                    }
                    else
                    {
                        Pixels[Mathf.FloorToInt(i + j * Data.MapWidth)] = Color.red;
                    }
                }
            }
            NewTexture.SetPixels(Pixels);
            NewTexture.Apply();
            MapImage.texture = NewTexture as Texture;
        }

        public void GenerateFog()
        {

        }
    }

}