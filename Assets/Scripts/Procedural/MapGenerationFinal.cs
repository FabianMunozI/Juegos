using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MapGenerationFinal : MonoBehaviour
{
    private TileType[][] map;
    private int firstSplit;
    private int secondSplit;
    [SerializeField] private GameObject[] hillPrefabs;
    private int nOfHills;
    [SerializeField] private GameObject[] treePrefabs;
    private int nOfTrees;
    [SerializeField] private GameObject[] rockPrefabs;
    private int nOfRocks;
    [SerializeField] private GameObject[] flowerPrefabs;
    private int nOfFlowers;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int townWidth;
    [SerializeField] private int townHeight;
    [SerializeField] private int townExit;
    private int roadStart;
    [SerializeField] private int mountainWidth;
    [SerializeField] private int mountainHeight;
    private (int, int) mountainPos; // No se coloca la montana!!!
    [SerializeField] private int maxRoadCornerSparation = 10;
    

    private enum TileType
    {
        Empty,
        Grass,
        River,
        Road,
        Bridge,
        Tree,
        Hill,
        Flower,
        Rock,
        // Rios y caminos, pero especificamos el tipo.
        RiNS, // River North - South
        RiEW, // River East  - West
        RiNE, // River North - East
        RiNW, // River North - West
        RiSE, // River South - East
        RiSW, // River South - West
        RoNS, // Road  North - South
        RoEW, // Road  East  - West
        RoNE, // Road  North - South
        RoNW, // Road  North - South
        RoSE, // Road  North - South
        RoSW, // Road  North - South
        BNS,  // Bridge North- South
        BEW,   // Bridge East - West
        BGrass,
        BTree,
        BFlower
    }

    private void InitializeMap()
    {
        map = new TileType[width][];
        for (int i = 0; i < height; i++)
        {
            map[i] = new TileType[height];
        }
        firstSplit = height / 4; // Altura del primer tercio
        secondSplit = height / 4 * 3; // Altura del segundo tercio
    }
    
    // Coloca el pueblo en algun lugar
    private void PutTown()
    {
        int min = townExit;
        int max = width - townWidth + townExit;
        roadStart = Random.Range(min, max);
    }

    // Coloca la montana en el tercer tercio del mapa
    private void PutMountain()
    {
        // I need to understand the mountain prefab first, but this is not yet relevant.
    }

    private void ConnectRoad(int startX, int startY, int endX, int endY)
    {
        int x = startX;
        int y = startY;
        while (x < endX)
        {
            map[x++][y] = TileType.Road;
        }
        while (x > endX)
        {
            map[x--][y] = TileType.Road;
        }
        while (y < endY)
        {
            map[x][y++] = TileType.Road;
        }
    }

    // Genera el camino, desde la salida del pueblo hasta el fin del segundo tercio del mapa
    private int[] GenRoad()
    {
        int[] corners = new int[Random.Range(3, 8)];
        int newCorner;
        corners[corners.Length - 1] = secondSplit;
        corners[0] = firstSplit;

        for (int i = 1; i < corners.Length - 1; i++)
        {
            do {
                newCorner = Random.Range(firstSplit + 1, secondSplit);
            } while (corners.Contains(newCorner) || corners.Contains(newCorner + 1) || corners.Contains(newCorner - 1));
            corners[i] = newCorner;
        }
        System.Array.Sort(corners);

        int x = roadStart;
        int y = corners[0];
        for (int c = 1; c < corners.Length; c++)
        {
            int endX;
            do
            {
                endX = Random.Range(-maxRoadCornerSparation, maxRoadCornerSparation);
            } while (Mathf.Abs(endX) < 4);
            endX = Mathf.Clamp(x + endX, 3, width - 3);
            ConnectRoad(x, y, endX, corners[c]);
            x = endX;
            y = corners[c];
        }
        return corners;
    }

    private void ConnectRiver(int x, int y, int ex, int ey)
    {
        while (y != ey)
        {
            if (map[x][y] == TileType.Empty)
            {
                map[x][y] = TileType.River;
                y = (y < ey) ? y+1 : y-1;
            }
            else if (map[x][y+((y < ey)?1:-1)] == TileType.Road)
            {
                y = (y < ey) ? y-1 : y+1;
                x++;
            }
            else
            {
                map[x][y] = TileType.Bridge;
                y = (y < ey) ? y+1 : y-1;
            }
        }
        while (x < ex)
        {
            if (map[x][y] == TileType.Empty)
                map[x][y] = TileType.River;
            else
                map[x][y] = TileType.Bridge;
            x++;
        }
    }

    // Genera el rio dentro del segundo tercio del mapa
    private void GenRiver(int[] corners)
    {
        int RandY()
        {
            int y;
            do
            {
                y = Random.Range(firstSplit + 1, secondSplit - 1);
            } while (corners.Contains(y));
            return y;
        }

        bool XInCorners((int, int)[] corners, int x)
        {
            foreach ((int, int) pair in corners)
            {
                if (pair.Item1 == x || pair.Item1 == x+1 || pair.Item1 == x-1)
                    return true;
            }
            return false;
        }

        int CompareTuples((int, int) a, (int, int) b)
        {
            if (a.Item1 > b.Item1)
                return 1;
            else if (a.Item1 < b.Item1)
                return -1;
            return 0;
        }

        (int, int)[] rCorners = new (int, int)[Random.Range(2, 8)];
        rCorners[0] = (1, RandY());
        rCorners[rCorners.Length - 1] = (width - 1, RandY());


        for (int i = 1; i < rCorners.Length - 1; i++)
        {
            int x;
            int y;
            do
            {
                x = Random.Range(3, width - 3);
                y = Random.Range(firstSplit + 1, secondSplit - 1);
            } while (XInCorners(rCorners, x) || corners.Contains(y) || map[x][y] == TileType.Road);
            rCorners[i] = (x, y);
        }

        System.Array.Sort(rCorners, CompareTuples);


        for (int i = 0; i < rCorners.Length - 1; i++)
        {
            ConnectRiver(rCorners[i].Item1, rCorners[i].Item2, rCorners[i + 1].Item1, rCorners[i + 1].Item2);
        }
    }


    // Define direcciones de curvas y rectas (Caminos)
    private void ProcessRoadDirections()
    {
        bool IsRoad(TileType t)
        {
            if (t == TileType.Road) return true;
            if (t == TileType.RoNS) return true;
            if (t == TileType.RoEW) return true;
            if (t == TileType.RoNE) return true;
            if (t == TileType.RoNW) return true;
            if (t == TileType.RoSE) return true;
            if (t == TileType.RoSW) return true;
            if (t == TileType.Bridge) return true;
            if (t == TileType.BNS) return true;
            if (t == TileType.BEW) return true;
            return false;
        }

        TileType CheckRoadDir(int x, int y)
        {
            if (IsRoad(map[x][y+1]) && IsRoad(map[x][y - 1]))
                return TileType.RoNS;
            if (IsRoad(map[x + 1][y]) && IsRoad(map[x - 1][y]))
                return TileType.RoEW;
            if (IsRoad(map[x + 1][y]) && IsRoad(map[x][y + 1]))
                return TileType.RoNE;
            if (IsRoad(map[x + 1][y]) && IsRoad(map[x][y - 1]))
                return TileType.RoSE;
            if (IsRoad(map[x - 1][y]) && IsRoad(map[x][y + 1]))
                return TileType.RoNW;
            if (IsRoad(map[x - 1][y]) && IsRoad(map[x][y - 1]))
                return TileType.RoSW;
            if (IsRoad(map[x][y+1]) || IsRoad(map[x][y - 1]))
                return TileType.RoNS;
            if (IsRoad(map[x + 1][y]) || IsRoad(map[x - 1][y]))
                return TileType.RoEW;
            return TileType.Road;
        }

        for (int x = 0; x < map.Length; x++)
        {
            for (int y = 0; y < map.Length; y++)
            {
                if (map[x][y] == TileType.Road)
                {
                    map[x][y] = CheckRoadDir(x, y);
                }
            }
        }
    }

    // Define direcciones de curvas y rectas (Rios)
    private void ProcessRiverDirections()
    {
        bool IsRiver(TileType t)
        {
            if (t == TileType.River) return true;
            if (t == TileType.RiNS) return true;
            if (t == TileType.RiEW) return true;
            if (t == TileType.RiNE) return true;
            if (t == TileType.RiNW) return true;
            if (t == TileType.RiSE) return true;
            if (t == TileType.RiSW) return true;
            if (t == TileType.Bridge) return true;
            if (t == TileType.BNS) return true;
            if (t == TileType.BEW) return true;
            return false;
        }

        TileType CheckRiverDir(int x, int y)
        {
            if (IsRiver(map[x][y + 1]) && IsRiver(map[x][y - 1]))
                return TileType.RiNS;
            if (IsRiver(map[x + 1][y]) && IsRiver(map[x - 1][y]))
                return TileType.RiEW;
            if (IsRiver(map[x + 1][y]) && IsRiver(map[x][y + 1]))
                return TileType.RiNE;
            if (IsRiver(map[x + 1][y]) && IsRiver(map[x][y - 1]))
                return TileType.RiSE;
            if (IsRiver(map[x - 1][y]) && IsRiver(map[x][y + 1]))
                return TileType.RiNW;
            if (IsRiver(map[x - 1][y]) && IsRiver(map[x][y - 1]))
                return TileType.RiSW;
            if (IsRiver(map[x - 1][y]) || IsRiver(map[x + 1][y]))
                return TileType.RiEW;
            if (IsRiver(map[x][y-1]) || IsRiver(map[x][y+1]))
                return TileType.RiNS;
            return TileType.Empty;
        }

        for (int x = 0; x < map.Length; x++)
        {
            for (int y = 0; y < map.Length; y++)
            {
                if (map[x][y] == TileType.River)
                {
                    map[x][y] = CheckRiverDir(x, y);
                }
            }
        }
    }

    // Define direcciones de puentes
    private void ProcessBridgeDirections()
    {
        for (int x = 0; x < map.Length; x++)
        {
            for (int y = 0; y < map.Length; y++)
            {
                if (map[x][y] == TileType.Bridge)
                {
                    if (map[x][y + 1] == TileType.Road)
                        map[x][y] = TileType.BNS;
                    else
                        map[x][y] = TileType.BEW;
                }
            }
        }
    }

    // Agrega obstaculos
    private void AddObstacles()
    {
        // Add Flowers
        for (int i = 0; i < nOfFlowers; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            if (map[x][y] == TileType.Empty)
                map[x][y] = TileType.Flower;
            else
                i--;
        }

        // Add Rocks
        for (int i = 0; i < nOfRocks; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            if (map[x][y] == TileType.Empty)
                map[x][y] = TileType.Rock;
            else
                i--;
        }

        // Add Trees
        for (int i = 0; i < nOfTrees; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            if (map[x][y] == TileType.Empty)
                map[x][y] = TileType.Tree;
            else
                i--;
        }
    }

    private void AddGrass()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x][y] == TileType.Empty)
                    map[x][y] = TileType.Grass;
            }
        }
    }

    // Instancia todos los prefabs
    private void BuildMap()
    {
        // TODO
    }

    

    private void DrawCircle(int x, int y, Color c, float r = 0.5f, int res = 4)
    {
        Gizmos.color = c;
        int a = 0;
        float d = 2 * Mathf.PI / res;
        while (a < res)
        {
            float sx = Mathf.Cos(a * d) * r;
            float sy = Mathf.Sin(a * d) * r;
            float ex = Mathf.Cos((a + 1) * d) * r;
            float ey = Mathf.Sin((a + 1) * d) * r;
            Gizmos.DrawLine(new Vector3(x + 0.5f + sx, 0f, y + 0.5f + sy), new Vector3(x + 0.5f + ex, 0f, y + 0.5f + ey));
            a++;
        }

    }

    private void BurnShitUp()
    {
        int r = Random.Range(5, 10);
        int cx = Random.Range(5, width - 5);
        int cy = Random.Range(firstSplit + r, height - 5);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if ((cx - x) * (cx - x) + (cy - y) * (cy - y) <= r * r + Random.Range(-100, 100))
                {
                    if (map[x][y] == TileType.Flower)
                        map[x][y] = TileType.BFlower;
                    else if (map[x][y] == TileType.Grass)
                        map[x][y] = TileType.BGrass;
                    else if (map[x][y] == TileType.Tree)
                        map[x][y] = TileType.BTree;
                }
            }
        }
    }

    // Genera el mapa
    private void GenMap()
    {
        InitializeMap();
        PutTown();
        PutMountain();
        int[] corners = GenRoad();
        GenRiver(corners);
        ProcessBridgeDirections();
        ProcessRoadDirections();
        ProcessRiverDirections();
        AddObstacles();
        AddGrass();
        BurnShitUp();
        BuildMap();
    }

    private void Start()
    {
        nOfTrees = width * height / 12;
        nOfRocks = width * height / 96;
        nOfFlowers = width * height / 24;
        GenMap();
    }

    private void DrawBounds()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, height + 1f));
        Gizmos.DrawLine(new Vector3(0f, 0f, 0f), new Vector3(width + 1f, 0f, 0f));
        Gizmos.DrawLine(new Vector3(width + 1f, 0f, 0f), new Vector3(width + 1f, 0f, height + 1f));
        Gizmos.DrawLine(new Vector3(0f, 0f, height + 1f), new Vector3(width + 1f, 0f, height + 1f));
    }

    private void DrawL(int x, int y, int dir, Color c)
    {
        Gizmos.color = c;
        float cx = x + 0.5f;
        float cy = y + 0.5f;
        switch (dir)
        {
            case 0:
                Gizmos.DrawLine(new Vector3(cx, 0f, y), new Vector3(cx, 0f, y + 1));
                break;
            case 1:
                Gizmos.DrawLine(new Vector3(x, 0f, cy), new Vector3(x + 1, 0f, cy));
                break;
            case 2:
                Gizmos.DrawLine(new Vector3(cx, 0f, y + 1), new Vector3(cx, 0f, cy));
                Gizmos.DrawLine(new Vector3(x + 1, 0f, cy), new Vector3(cx, 0f, cy));
                break;
            case 3:
                Gizmos.DrawLine(new Vector3(cx, 0f, y), new Vector3(cx, 0f, cy));
                Gizmos.DrawLine(new Vector3(x + 1, 0f, cy), new Vector3(cx, 0f, cy));
                break;
            case 4:
                Gizmos.DrawLine(new Vector3(cx, 0f, y), new Vector3(cx, 0f, cy));
                Gizmos.DrawLine(new Vector3(x - 1, 0f, cy), new Vector3(cx, 0f, cy));
                break;
            case 5:
                Gizmos.DrawLine(new Vector3(x, 0f, cy), new Vector3(cx, 0f, cy));
                Gizmos.DrawLine(new Vector3(cx, 0f, y + 1), new Vector3(cx, 0f, cy));
                break;
            default:
                Debug.LogError("Invalid draw direction!");
                break;
        }
    }

    private void DrawX(int x, int y, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(new Vector3(x, 0f, y), new Vector3(x + 1, 0f, y + 1));
        Gizmos.DrawLine(new Vector3(x, 0f, y + 1), new Vector3(x + 1, 0f, y));
    }
    private void OnDrawGizmos()
    {
        if (!(map is null))
        {
            DrawBounds();
            for (int x = 0; x < map.Length; x++)
            {
                for (int y = 0; y < map[x].Length; y++)
                {
                    switch (map[x][y])
                    {
                        case TileType.Empty:
                            DrawCircle(x, y, new Color(0f, 0f, 0f, 0.5f));
                            break;
                        case TileType.Grass:
                            DrawX(x, y, new Color(0.2f, 0.8f, 0.1f));
                            break;
                        case TileType.River:
                            DrawX(x, y, Color.blue);
                            break;
                        case TileType.Road:
                            DrawX(x, y, new Color(1f, 0.5f, 0f));
                            break;
                        case TileType.Bridge:
                            DrawX(x, y, Color.white);
                            break;
                        case TileType.Tree:
                            DrawCircle(x, y, Color.yellow, 0.4f, 12);
                            break;
                        case TileType.Hill:
                            break;
                        case TileType.Flower:
                            DrawCircle(x, y, Color.magenta, 0.25f, 10);
                            DrawX(x, y, new Color(0.2f, 0.8f, 0.1f));
                            break;
                        case TileType.Rock:
                            DrawCircle(x, y, Color.gray, 0.4f, 10);
                            DrawCircle(x, y, Color.gray, 0.25f, 10);
                            break;
                        case TileType.RiNS:
                            DrawL(x, y, 0, Color.blue);
                            break;
                        case TileType.RiEW:
                            DrawL(x, y, 1, Color.blue);
                            break;
                        case TileType.RiNE:
                            DrawL(x, y, 2, Color.blue);
                            break;
                        case TileType.RiNW:
                            DrawL(x, y, 5, Color.blue);
                            break;
                        case TileType.RiSE:
                            DrawL(x, y, 3, Color.blue);
                            break;
                        case TileType.RiSW:
                            DrawL(x, y, 4, Color.blue);
                            break;
                        case TileType.RoNS:
                            DrawL(x, y, 0, new Color(1f, 0.5f, 0f));
                            break;
                        case TileType.RoEW:
                            DrawL(x, y, 1, new Color(1f, 0.5f, 0f));
                            break;
                        case TileType.RoNE:
                            DrawL(x, y, 2, new Color(1f, 0.5f, 0f));
                            break;
                        case TileType.RoNW:
                            DrawL(x, y, 5, new Color(1f, 0.5f, 0f));
                            break;
                        case TileType.RoSE:
                            DrawL(x, y, 3, new Color(1f, 0.5f, 0f));
                            break;
                        case TileType.RoSW:
                            DrawL(x, y, 4, new Color(1f, 0.5f, 0f));
                            break;
                        case TileType.BNS:
                            DrawL(x, y, 0, new Color(1f, 0.5f, 0f));
                            DrawL(x, y, 1, Color.blue);
                            break;
                        case TileType.BEW:
                            DrawL(x, y, 1, new Color(1f, 0.5f, 0f));
                            DrawL(x, y, 0, Color.blue);
                            break;
                        case TileType.BGrass:
                            DrawCircle(x, y, new Color(0.5f, 0.5f, 0.5f));
                            break;
                        case TileType.BTree:
                            DrawCircle(x, y, new Color(0.2f, 0.2f, 0.2f), 0.4f, 12);
                            break;
                        case TileType.BFlower:
                            DrawCircle(x, y, new Color(0.6f, 0.6f, 0.6f), 0.25f, 12);
                            DrawCircle(x, y, new Color(0.5f, 0.5f, 0.5f));
                            break;
                    }
                }
            }
        }
    }
}
