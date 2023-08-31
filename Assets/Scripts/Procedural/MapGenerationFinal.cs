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

    [SerializeField] private GameObject[] arbolesQuemadosPrefabs;

    [SerializeField] private GameObject inicioFinalRio, rioRecto, rioCurva, caminoRecto, caminoCurva, puente, pasto, pastoQuemado;

    [SerializeField] private GameObject pueblo;

    private int nOfFlowers;
    [SerializeField] private int width;
    [SerializeField] private int height;
    private int townWidth = 10;
    private int townHeight = 7;
    [SerializeField] private int townExit;
    private int roadStart;
    [SerializeField] private int mountainWidth;
    [SerializeField] private int mountainHeight;
    private (int, int) mountainPos; // No se coloca la montana!!!
    [SerializeField] private int maxRoadCornerSparation = 10;

    Transform parent;

    public GameObject player;
    //Dictionary<Vector3, GameObject> diccionarioObstaculos = new Dictionary<Vector3, GameObject>();
    //List<GameObject> pisoExtra = new List<GameObject>();


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
        Nada,
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
        InicioRioN,
        InicioRioE,
        InicioRioS,
        InicioRioW,
        FinalRioN,
        FinalRioE,
        FinalRioS,
        FinalRioW,
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
        // min = townExit;
        //int max = width - townWidth + townExit;

        CrearIndicador(new Vector3(roadStart -0.2f, 3.13f, firstSplit + 2.3f), pueblo, Quaternion.Euler(0, 180, 0));

        for(int ancho = 0 ; ancho < 10 ; ancho++)
        {
            for(int alto = 0 ; alto < 7 ; alto++)
            {
                if(!(ancho == 5 && alto == 4) && !(ancho == 5 && alto == 5) && !(ancho == 5 && alto == 6) && !(ancho == 5 && alto == 7))
                {
                    map[(roadStart - 5) + ancho][(firstSplit - 7) + alto] = TileType.Grass;
                }
                else
                {
                    map[(roadStart - 5) + ancho][(firstSplit - 7) + alto] = TileType.Nada;
                }
            }
        }

        //quizas el pozo podr�a quedar en el camino 
        map[roadStart + 1][firstSplit + 1] = TileType.Grass;
    }

    // Coloca la montana en el tercer tercio del mapa
    private void PutMountain()
    {
        // I need to understand the mountain prefab first, but this is not yet relevant.
        int min = townExit;
        int max = width - townWidth + townExit;
        //roadStart = Random.Range(min, max);
        roadStart = Random.Range(7, width - 7);
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
                if (map[x][y] != TileType.InicioRioE)
                {
                    map[x][y] = TileType.Bridge;
                }
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
                y = Random.Range(firstSplit + 2, secondSplit - 2);
            } while (corners.Contains(y));
            return y;
        }

        bool XInCorners((int, int)[] corners, int x)
        {
            foreach ((int, int) pair in corners)
            {
                if (pair.Item1 == x || pair.Item1 == x + 1 || pair.Item1 == x - 1)
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
        rCorners[0] = (5, RandY());
        (int, int) inicioRio = rCorners[0];
        rCorners[rCorners.Length - 1] = (width - 5, RandY());


        for (int i = 1; i < rCorners.Length - 1; i++)
        {
            int x;
            int y;
            do
            {
                x = Random.Range(5, width - 5);
                y = Random.Range(firstSplit + 1, secondSplit - 1);
            } while (XInCorners(rCorners, x) || corners.Contains(y) || map[x][y] == TileType.Road || map[x][y] == TileType.Grass);
            rCorners[i] = (x, y);
        }

        System.Array.Sort(rCorners, CompareTuples);


        map[inicioRio.Item1][inicioRio.Item2] = TileType.InicioRioE;


        for (int i = 0; i < rCorners.Length - 1; i++) // Los bordes ya est�n seteados hay que ver esos casos
        {
            ConnectRiver(rCorners[i].Item1, rCorners[i].Item2, rCorners[i + 1].Item1, rCorners[i + 1].Item2);
        }

        //revisar que esten dentro del mapa

        if (EsValido(inicioRio.Item1, inicioRio.Item2 + 1) )
        {
            if(map[inicioRio.Item1][inicioRio.Item2 + 1] == TileType.River)
                map[inicioRio.Item1][inicioRio.Item2] = TileType.InicioRioN;
        }
        else if (EsValido(inicioRio.Item1 - 1, inicioRio.Item2))
        {
            if (map[inicioRio.Item1 - 1][inicioRio.Item2] == TileType.River)
                map[inicioRio.Item1][inicioRio.Item2] = TileType.InicioRioW;
        }
        else if (EsValido(inicioRio.Item1 - 1, inicioRio.Item2 - 1))
        {
            if (map[inicioRio.Item1 - 1][inicioRio.Item2 - 1] == TileType.River)
                map[inicioRio.Item1][inicioRio.Item2] = TileType.InicioRioS;
        }

        if (EsValido(rCorners[rCorners.Length - 1].Item1,rCorners[rCorners.Length - 2].Item2))
        {
            if(map[rCorners[rCorners.Length - 1].Item1][rCorners[rCorners.Length - 2].Item2] == TileType.River)
                map[rCorners[rCorners.Length - 1].Item1][rCorners[rCorners.Length - 1].Item2] = TileType.FinalRioN;
        }
        else if (EsValido(rCorners[rCorners.Length - 2].Item1 , rCorners[rCorners.Length - 2].Item2))
        {
            if (map[rCorners[rCorners.Length - 2].Item1][rCorners[rCorners.Length - 2].Item2] == TileType.River)
                map[rCorners[rCorners.Length - 1].Item1][rCorners[rCorners.Length - 1].Item2] = TileType.FinalRioE;
        }
        else if(EsValido(rCorners[rCorners.Length - 1].Item1 , rCorners[rCorners.Length].Item2))
        {
            if (map[rCorners[rCorners.Length - 1].Item1][rCorners[rCorners.Length].Item2] == TileType.River)
            map[rCorners[rCorners.Length - 1].Item1][rCorners[rCorners.Length - 1].Item2] = TileType.FinalRioS;
        }
        else
        {
            map[rCorners[rCorners.Length - 1].Item1][rCorners[rCorners.Length - 1].Item2] = TileType.FinalRioW;
        }
        
    }

    private bool EsValido(int x, int z)  // es menos 1 o no?
    {
        if (x < 0 || x > width - 1 || z < 0  || z > height-1)
            return false;
        return true;
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
    private void BuildMap() // falta inicio rio 
    {
        // TODO
        int index;

        for (int x = 0; x < map.Length; x++)
        {
            for (int y = 0; y < map[x].Length; y++)
            {
                var posicion = new Vector3(x, 0, y);
                switch (map[x][y])
                {
                    case TileType.Empty:
                        break;
                    case TileType.Nada:
                        break;
                    case TileType.Grass:
                        CrearIndicador(posicion, pasto);
                        break;
                    case TileType.River:
                        break;
                    case TileType.Road:
                        break;
                    case TileType.Bridge:
                        break;
                    case TileType.Tree:
                        index = Random.Range(0,treePrefabs.Length);
                        CrearIndicador(posicion, treePrefabs[index], new Quaternion(), true);
                        break;
                    case TileType.Hill:
                        break;
                    case TileType.Flower:
                        index = Random.Range(0, flowerPrefabs.Length);
                        CrearIndicador(posicion, flowerPrefabs[index], new Quaternion(), true);
                        break;
                    case TileType.Rock:
                        index = Random.Range(0, rockPrefabs.Length);
                        CrearIndicador(posicion, rockPrefabs[index], new Quaternion(), true);
                        break;
                    case TileType.RiNS:
                        CrearIndicador(posicion + new Vector3(0, -0.0166f, 0), rioRecto);
                        break;
                    case TileType.RiEW:
                        CrearIndicador(posicion + new Vector3(0, -0.0166f, 0), rioRecto, Quaternion.Euler(0, 90, 0));
                        break;
                    case TileType.RiNE:
                        CrearIndicador(posicion + new Vector3(0, -0.036f, 0), rioCurva, Quaternion.Euler(0, 180, 0));
                        break;
                    case TileType.RiNW:
                        CrearIndicador(posicion + new Vector3(0, -0.036f, 0), rioCurva, Quaternion.Euler(0, 90, 0)); 
                        break;
                    case TileType.RiSE:
                        CrearIndicador(posicion + new Vector3(0, -0.036f, 0), rioCurva, Quaternion.Euler(0, 270, 0));
                        break;
                    case TileType.RiSW:
                        CrearIndicador(posicion + new Vector3(0, -0.036f, 0), rioCurva);
                        break;
                    case TileType.RoNS:
                        CrearIndicador(posicion + new Vector3(0, 0.083f, 0), caminoRecto);
                        break;
                    case TileType.RoEW:
                        CrearIndicador(posicion + new Vector3(0, 0.083f, 0), caminoRecto, Quaternion.Euler(0, 90, 0));
                        break;
                    case TileType.RoNE:
                        CrearIndicador(posicion + new Vector3(0, -0.056f, 0), caminoCurva, Quaternion.Euler(0, 180, 0));
                        break;
                    case TileType.RoNW:
                        CrearIndicador(posicion + new Vector3(0, -0.056f, 0), caminoCurva, Quaternion.Euler(0, 90, 0));
                        break;
                    case TileType.RoSE:
                        CrearIndicador(posicion + new Vector3(0, -0.056f, 0), caminoCurva, Quaternion.Euler(0, 270, 0));
                        break;
                    case TileType.RoSW:
                        CrearIndicador(posicion + new Vector3(0, -0.056f, 0), caminoCurva);
                        break;
                    case TileType.BNS:
                        CrearIndicador(posicion, puente, new Quaternion());
                        CrearIndicador(posicion , rioRecto, Quaternion.Euler(0, 90, 0));
                        break;
                    case TileType.BEW:
                        CrearIndicador(posicion, puente, Quaternion.Euler(0, 90, 0));
                        CrearIndicador(posicion + new Vector3(0, -0.0386f, 0), rioRecto, new Quaternion());
                        break;
                    case TileType.BGrass:
                        CrearIndicador(posicion, pastoQuemado);
                        break;
                    case TileType.BTree:
                        index = Random.Range(0, arbolesQuemadosPrefabs.Length);
                        if (index == 1)
                        {
                            CrearIndicador(posicion + new Vector3(0, 0.58f, 0), arbolesQuemadosPrefabs[index], Quaternion.Euler(270,0,0), false, true, true);
                        }
                        else
                        {
                            CrearIndicador(posicion, arbolesQuemadosPrefabs[index], new Quaternion(), false, true);
                        }
                        break;
                    case TileType.BFlower:
                        index = Random.Range(0, arbolesQuemadosPrefabs.Length);
                        if (index == 1)
                        {
                            CrearIndicador(posicion + new Vector3(0, 0.58f, 0), arbolesQuemadosPrefabs[index], Quaternion.Euler(270, 0, 0), false, true, true);
                        }
                        else
                        {
                            CrearIndicador(posicion, arbolesQuemadosPrefabs[index], new Quaternion(), false, true);
                        }
                        break;
                    case TileType.InicioRioN:
                        CrearIndicador(posicion + new Vector3(0, -0.02f, 0), inicioFinalRio, Quaternion.Euler(0, 180, 0));
                        break;
                    case TileType.InicioRioE:
                        CrearIndicador(posicion + new Vector3(0, -0.02f, 0), inicioFinalRio);
                        break;
                    case TileType.InicioRioS:
                        CrearIndicador(posicion + new Vector3(0, -0.02f, 0), inicioFinalRio, Quaternion.Euler(0, 90, 0));
                        break;
                    case TileType.InicioRioW:
                        CrearIndicador(posicion + new Vector3(0, -0.02f, 0), inicioFinalRio, Quaternion.Euler(0, 270, 0));
                        break;
                    case TileType.FinalRioN:
                        CrearIndicador(posicion + new Vector3(0, -0.02f, 0), inicioFinalRio, Quaternion.Euler(0, 180, 0));
                        break;
                    case TileType.FinalRioE:
                        CrearIndicador(posicion + new Vector3(0, -0.02f, 0), inicioFinalRio, Quaternion.Euler(0, 90, 0));
                        break;
                    case TileType.FinalRioS:
                        CrearIndicador(posicion + new Vector3(0, -0.02f, 0), inicioFinalRio);
                        break;
                    case TileType.FinalRioW:
                        CrearIndicador(posicion + new Vector3(0, -0.02f, 0), inicioFinalRio, Quaternion.Euler(0, 180, 0));
                        break;
                }
            }
        }

    }

    private void CrearIndicador(Vector3 position, GameObject prefab, Quaternion rotation = new Quaternion(), bool objet = false, bool quemado = false, bool quemaDos = false)
    {
        var placePosition = position + new Vector3(.5f, .5f, .5f);
        if (objet)
        {
            var elemento = Instantiate(pasto, placePosition, new Quaternion());
            elemento.transform.parent = parent;
            //pisoExtra.Add(elemento);

        }
        if (quemado)
        {
            if (quemaDos)
            {
                placePosition = placePosition - new Vector3(0, .58f, 0);
            }
            var elemento = Instantiate(pastoQuemado, placePosition, new Quaternion());
            elemento.transform.parent = parent;
            //pisoExtra.Add(elemento);
        }
        var placementPosition = position + new Vector3(.5f, .5f, .5f);
        var element = Instantiate(prefab, placementPosition, rotation);
        element.transform.parent = parent;
        //diccionarioObstaculos.Add(position, element);
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

    private void BurnShitUp() // agregar mas arboles quemados
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
                        map[x][y] = TileType.BGrass;
                    if (map[x][y] == TileType.Grass)
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
        PutMountain();
        int[] corners = GenRoad();
        PutTown();
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

        Invoke("GenPlayer",2f);
        Cursor.lockState = CursorLockMode.None;
    }

    public void GenPlayer(){
        Instantiate(player, new Vector3(25, 2, 25), Quaternion.identity);
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
