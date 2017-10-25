using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    [ExecuteInEditMode, RequireComponent(typeof(MapData))]
    public class CaveGenerator : MonoBehaviour
    {
        [Header("Settings")]
        public string Seed;
        public bool IsRandomSeed;
        [Range(0, 100)]
        public int RandomFillPercent;
        public bool IsGenerateOnStart;
        [Header("Actions")]
        public bool DoFillMap;
        public bool DoAlterSolids;
        private MapData MyData;

        void Awake()
        {
            MyData = GetComponent<MapData>();
        }

        private void Start()
        {
            if (Application.isPlaying && IsGenerateOnStart)
            {
                GenerateSeedMap();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (DoFillMap)
            {
                DoFillMap = false;
                GenerateMap();
            }
            if (DoAlterSolids)
            {
                DoAlterSolids = false;
                AlterSolids();
            }
        }

        public void GenerateSeedMap()
        {
            Seed = PlayerPrefs.GetString("Seed", "-282");
            int Level = PlayerPrefs.GetInt("Level", 1);
            if (Level != 1)
            {
                Seed += (char)Level;
            }
            GenerateMap();
            AlterSolids();
            MyData.FillWithTiles();
        }

        private MapData GetMapData()
        {
            if (MyData == null)
            {
                MyData = GetComponent<MapData>();
            }
            return MyData;
        }

        public void GenerateMap()
        {
            GetMapData().Empty();
            RandomFillMap();
            for (int i = 0; i < 5; i++)
            {
                SmoothMap();
            }
        }

        void RandomFillMap()
        {
            if (IsRandomSeed)
            {
                Seed = Random.Range(-400, 400).ToString();
            }

            System.Random pseudoRandom = new System.Random(Seed.GetHashCode());

            for (int x = 0; x < MyData.MapWidth; x++)
            {
                for (int y = 0; y < MyData.MapHeight; y++)
                {
                    if (x == 0 || x == MyData.MapWidth - 1 || y == 0 || y == MyData.MapHeight - 1)
                    {
                        MyData.Data[x, y] = 1;
                    }
                    else
                    {
                        MyData.Data[x, y] = (pseudoRandom.Next(0, 100) < RandomFillPercent) ? 1 : 0;
                    }
                }
            }
        }

        public void AlterSolids()
        {
            for (int x = 0; x < MyData.MapWidth; x++)
            {
                for (int y = 0; y < MyData.MapHeight; y++)
                {
                    if (MyData.Data[x, y] >= 1)
                    {
                        int neighbourWallTiles = GetDirectNeighborsCount(x, y);

                        if (neighbourWallTiles == 4)
                        {
                            MyData.Data[x, y] = 2;
                        }
                    }
                }
            }
        }

        int GetDirectNeighborsCount(int gridX, int gridY)
        {
            int wallCount = 0;
            int neighbourX = 0;
            int neighbourY = 0;

            neighbourX = gridX + 1;
            neighbourY = gridY;
            if (neighbourX >= 0 && neighbourX < MyData.MapWidth && neighbourY >= 0 && neighbourY < MyData.MapHeight)
            {
                if (MyData.Data[neighbourX, neighbourY] > 0)
                {
                    wallCount++;
                }
            }
            neighbourX = gridX - 1;
            neighbourY = gridY;
            if (neighbourX >= 0 && neighbourX < MyData.MapWidth && neighbourY >= 0 && neighbourY < MyData.MapHeight)
            {
                if (MyData.Data[neighbourX, neighbourY] > 0)
                {
                    wallCount++;
                }
            }

            neighbourX = gridX;
            neighbourY = gridY + 1;
            if (neighbourX >= 0 && neighbourX < MyData.MapWidth && neighbourY >= 0 && neighbourY < MyData.MapHeight)
            {
                if (MyData.Data[neighbourX, neighbourY] > 0)
                {
                    wallCount++;
                }
            }
            neighbourX = gridX;
            neighbourY = gridY - 1;
            if (neighbourX >= 0 && neighbourX < MyData.MapWidth && neighbourY >= 0 && neighbourY < MyData.MapHeight)
            {
                if (MyData.Data[neighbourX, neighbourY] > 0)
                {
                    wallCount++;
                }
            }
            return wallCount;
        }

        void SmoothMap()
        {
            for (int x = 0; x < MyData.MapWidth; x++)
            {
                for (int y = 0; y < MyData.MapHeight; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                        MyData.Data[x, y] = 1;
                    else if (neighbourWallTiles < 4)
                        MyData.Data[x, y] = 0;

                }
            }
        }

        int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < MyData.MapWidth && neighbourY >= 0 && neighbourY < MyData.MapHeight)
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += MyData.Data[neighbourX, neighbourY];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }
    }

}