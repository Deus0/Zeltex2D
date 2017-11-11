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
        public MapOutlineType MyMapOutlineType = MapOutlineType.Rect;
        [Header("Actions")]
        public bool DoFillMap;
        public bool DoAlterSolids;
        private MapData MyData;

        public enum MapOutlineType
        {
            Rect,
            Circle
        }

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
            if (MyMapOutlineType == MapOutlineType.Rect)
            {
                FillOutsideRect();
            }
            else
            {
                FillOutsideCircle();
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
                    MyData.Data[x, y] = (pseudoRandom.Next(0, 100) < RandomFillPercent) ? 1 : 0;
                }
            }
        }

        public void FillOutsideRect()
        {
            for (int x = 0; x < MyData.MapWidth; x++)
            {
                for (int y = 0; y < MyData.MapHeight; y++)
                {
                    if (x == 0 || x == MyData.MapWidth - 1 || y == 0 || y == MyData.MapHeight - 1)
                    {
                        MyData.Data[x, y] = 1;
                    }
                }
            }
        }

        public void FillOutsideCircle()
        {
            float DistanceToMid = 0;
            float MaxDistanceToMid = Mathf.Min(MyData.MapWidth, MyData.MapHeight) / 2 - 1;
            Vector2 MidPoint = new Vector2(MyData.MapWidth / 2f, MyData.MapHeight / 2f);
            for (int x = 0; x < MyData.MapWidth; x++)
            {
                for (int y = 0; y < MyData.MapHeight; y++)
                {
                    DistanceToMid = Vector2.Distance(new Vector2(x, y), MidPoint);
                    if (DistanceToMid >= MaxDistanceToMid)
                    {
                        MyData.Data[x, y] = 1;
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

            for (int x = 0; x < MyData.MapWidth; x++)
            {
                for (int y = 0; y < MyData.MapHeight; y++)
                {
                    if (MyData.Data[x, y] == 1)
                    {
                        float Chance = Random.Range(1, 100);
                        if (Chance >= 90)
                        {
                            MyData.Data[x, y] = 3;
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

        #region MapRegions

        bool IsInMapRange(int x, int y)
        {
            return x >= 0 && x < MyData.MapWidth && y >= 0 && y < MyData.MapHeight;
        }

        struct Coord
        {
            public int tileX;
            public int tileY;

            public Coord(int x, int y)
            {
                tileX = x;
                tileY = y;
            }
        }

        void ProcessMap()
        {
            List<List<Coord>> wallRegions = GetRegions(1);
            int wallThresholdSize = 50;

            foreach (List<Coord> wallRegion in wallRegions)
            {
                if (wallRegion.Count < wallThresholdSize)
                {
                    foreach (Coord tile in wallRegion)
                    {
                        MyData.Data[tile.tileX, tile.tileY] = 0;
                    }
                }
            }

            List<List<Coord>> roomRegions = GetRegions(0);
            int roomThresholdSize = 50;

            foreach (List<Coord> roomRegion in roomRegions)
            {
                if (roomRegion.Count < roomThresholdSize)
                {
                    foreach (Coord tile in roomRegion)
                    {
                        MyData.Data[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }
        List<List<Coord>> GetRegions(int tileType)
        {
            List<List<Coord>> regions = new List<List<Coord>>();
            int[,] mapFlags = new int[MyData.MapWidth, MyData.MapHeight];

            for (int x = 0; x < MyData.MapWidth; x++)
            {
                for (int y = 0; y < MyData.MapHeight; y++)
                {
                    if (mapFlags[x, y] == 0 && MyData.Data[x, y] == tileType)
                    {
                        List<Coord> newRegion = GetRegionTiles(x, y);
                        regions.Add(newRegion);

                        foreach (Coord tile in newRegion)
                        {
                            mapFlags[tile.tileX, tile.tileY] = 1;
                        }
                    }
                }
            }

            return regions;
        }

        List<Coord> GetRegionTiles(int startX, int startY)
        {
            List<Coord> tiles = new List<Coord>();
            int[,] mapFlags = new int[MyData.MapWidth, MyData.MapHeight];
            int tileType = MyData.Data[startX, startY];

            Queue<Coord> queue = new Queue<Coord>();
            queue.Enqueue(new Coord(startX, startY));
            mapFlags[startX, startY] = 1;

            while (queue.Count > 0)
            {
                Coord tile = queue.Dequeue();
                tiles.Add(tile);

                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                        {
                            if (mapFlags[x, y] == 0 && MyData.Data[x, y] == tileType)
                            {
                                mapFlags[x, y] = 1;
                                queue.Enqueue(new Coord(x, y));
                            }
                        }
                    }
                }
            }

            return tiles;
        }

        #endregion
    }

}