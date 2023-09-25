using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

using Random=UnityEngine.Random;


public class TestProce : MonoBehaviour
{

    public int width = 5;
    public int height = 5;
    public float cellSize = 10f;

    public float cellsHeight = 0.5f;

    public int beach_lenght = 3;

    public GameObject prefabTest;
    public GameObject prefabPlaya;
    public GameObject prefabArenaMojada;
    public Grid grid;

    public int seed = 42;

    public int center_x = -1;
    public int center_y = -1;

    private UnityEngine.Object[] terrainPrefabs;
    private UnityEngine.Object[] detailsPrefabs;
    private UnityEngine.Object[] detailsPlayaPrefabs;

    private GameObject Borde1Prefab;
    private GameObject Borde2Prefab;
    private GameObject Borde3Prefab;
    private GameObject EsquinaPrefab;

    public void Start()
    {
        terrainPrefabs = Resources.LoadAll("ProceduralPlaya/Prefabs/Terreno");
        detailsPrefabs = Resources.LoadAll("ProceduralPlaya/Prefabs/Detalles");
        detailsPlayaPrefabs = Resources.LoadAll("ProceduralPlaya/Prefabs/DetallesPlaya");
        Borde1Prefab = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/TerrenoTransicion/1Borde");
        Borde2Prefab = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/TerrenoTransicion/2Bordes");
        EsquinaPrefab = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/TerrenoTransicion/Esquina");
        
        Debug.Log("Cargadas Prefabs.");
        Debug.Log(terrainPrefabs.Length);
        Random.seed = seed;

        grid = new Grid(width, height, cellSize);

        Generate();
        BuildWorld();
    }

    void Generate()
    {
        // DEFINICION PRIMER NIVEL Y CENTRO.
        // --
        // ---


        // Redefinir Centro
        if (center_x == -1) center_x = Random.Range(0, width);
        if (center_y == -1) center_y = Random.Range(0, height);


        int left_floor = Random.Range((int) width/4, (int) 3 * width / 4);
        int right_floor = Random.Range((int) width/4, (int) 3 * width / 4);
        int up_floor = Random.Range((int) height/4, (int) 3 * height / 4);
        int down_floor = Random.Range((int) height/4, (int) 3 * height / 4);


        grid.setCellType(center_x, center_y, cellType.PISO);

        Tuple <int, int> left_limit = center_x - left_floor >= 0 ? new Tuple<int, int>(center_x - left_floor, center_y) : new Tuple<int, int>(0, center_y);
        Tuple <int, int> right_limit = center_x + right_floor < width - 1 ? new Tuple<int, int>(center_x + right_floor, center_y) : new Tuple<int, int>(width - 1, center_y);
        Tuple <int, int> up_limit = center_y + up_floor < height - 1 ? new Tuple<int, int>(center_x, center_y + up_floor) : new Tuple<int, int>(center_x, height - 1);
        Tuple <int, int> down_limit = center_y - down_floor >= 0 ? new Tuple<int, int>(center_x, center_y - down_floor) : new Tuple<int, int>(center_x, 0);

        Debug.Log(left_limit);
        Debug.Log(right_limit);
        Debug.Log(up_limit);
        Debug.Log(down_limit);

        for(int i = 0; i < left_floor; i++)
        {
            if (center_x - (i + 1) < 0) break;

            grid.setCellType(center_x - (i + 1), center_y, cellType.PISO);
        }

        for(int i = 0; i < right_floor; i++)
        {
            if (center_x + (i + 1) > width - 1) break;

            grid.setCellType(center_x + (i + 1), center_y, cellType.PISO);
        }

        for(int i = 0; i < up_floor; i++)
        {
            if (center_y + (i + 1) > height - 1) break;

            grid.setCellType(center_x, center_y + (i + 1), cellType.PISO);
        }

        for(int i = 0; i < down_floor; i++)
        {
            if (center_y - (i + 1) < 0) break;

            grid.setCellType(center_x, center_y - (i + 1), cellType.PISO);
        }

        foreach (var paso in PasosEstocasticos(left_limit, up_limit))
        {
            grid.setCellType(paso.Item1, paso.Item2, cellType.PISO);
        }
        
        foreach (var paso in PasosEstocasticos(up_limit, right_limit))
        {
            grid.setCellType(paso.Item1, paso.Item2, cellType.PISO);
        }
        foreach (var paso in PasosEstocasticos(right_limit, down_limit))
        {
            grid.setCellType(paso.Item1, paso.Item2, cellType.PISO);
        }
        foreach (var paso in PasosEstocasticos(down_limit, left_limit))
        {
            grid.setCellType(paso.Item1, paso.Item2, cellType.PISO);
        }
        
        FillIsland();
        Transform_Beach();

        /*
        foreach (var coor in GetOutsideLimits())
        {
            grid.setCellType(coor.Item1, coor.Item2, cellType.ARENAMOJADA, -1);
        }

        foreach (var coor in GetOutsideLimits())
        {
            grid.setCellType(coor.Item1, coor.Item2, cellType.ARENAMOJADA, -1);
        }

        foreach (var coor in GetOutsideLimits())
        {
            grid.setCellType(coor.Item1, coor.Item2, cellType.ARENAMOJADA, -2);
        }

        foreach (var coor in GetOutsideLimits())
        {
            grid.setCellType(coor.Item1, coor.Item2, cellType.ARENAMOJADA, -3);
        }
        */


        GenerateHeightMap(center_x - left_limit.Item1, right_limit.Item1 - center_x, up_limit.Item2 - center_y, center_y - down_limit.Item2);
    }

    void GenerateHeightMap(int ll, int rl, int ul, int dl)
    {

        int n_heights = Random.Range(3, 6);
        Debug.Log(n_heights);
        for (int i = 0; i < n_heights; i++)
        {
            int total_height = Random.Range(1, 3);

            int cx = center_x + Random.Range(- (int)ll/2, (int)rl/2);
            int cy = center_y + Random.Range(- (int)dl/2, (int)ul/2);


            /*
            if (cx >= 0 && cx < width && cy >= 0 && cy < height){
            if (grid.GetCellType(cx, cy) != cellType.VACIO) grid.SetCellHeight(cx, cy, 1);
            else break;}*/

            int ll_h = Random.Range(0, (int)ll/4);
            int rl_h = Random.Range(0, (int)rl/4);
            int ul_h = Random.Range(0, (int)ul/4);
            int dl_h = Random.Range(0, (int)dl/4);

            for (int j = 0; j < 1; j++)
            {
                

                if (grid.GetCellHeight(cx-ll_h, cy) < j + 1)
                    grid.SetCellHeight(cx-ll_h, cy, grid.GetCellHeight(cx-ll_h, cy) + 1);
                else
                    grid.SetCellHeight(cx-ll_h, cy, j+1);

                if (grid.GetCellHeight(cx+rl_h, cy) < j + 1)
                    grid.SetCellHeight(cx+rl_h, cy, grid.GetCellHeight(cx+rl_h, cy) + 1);
                else
                    grid.SetCellHeight(cx+rl_h, cy, j+1);

                if (grid.GetCellHeight(cx, cy+ul_h) < j + 1)
                    grid.SetCellHeight(cx, cy + ul_h, grid.GetCellHeight(cx, cy+ul_h) + 1);
                else
                    grid.SetCellHeight(cx, cy+ul_h, j+1);

                if (grid.GetCellHeight(cx, cy-dl_h) < j + 1)
                    grid.SetCellHeight(cx, cy - dl_h, grid.GetCellHeight(cx, cy-dl_h) + 1);
                else
                    grid.SetCellHeight(cx, cy- dl_h, j+1);

                foreach (var item in PasosEstocasticos(new Tuple<int, int>(cx-ll_h, cy), new Tuple<int, int>(cx, cy + ul_h)))
                {
                    grid.SetCellHeight(item.Item1, item.Item2, 1);
                }
                foreach (var item in PasosEstocasticos(new Tuple<int, int>(cx, cy + ul_h), new Tuple<int, int>(cx + rl_h, cy)))
                {
                    grid.SetCellHeight(item.Item1, item.Item2, 1);
                }
                foreach (var item in PasosEstocasticos(new Tuple<int, int>(cx+rl_h, cy), new Tuple<int, int>(cx, cy - dl_h)))
                {
                    grid.SetCellHeight(item.Item1, item.Item2, 1);
                }
                foreach (var item in PasosEstocasticos(new Tuple<int, int>(cx, cy - dl_h), new Tuple<int, int>(cx -ll_h, cy)))
                {
                    grid.SetCellHeight(item.Item1, item.Item2, 1);
                }
                

                ll_h = Random.Range(0, ll_h);
                rl_h = Random.Range(0, rl_h);
                ul_h = Random.Range(0, ul_h);
                dl_h = Random.Range(0, dl_h);


            }
        }
    }

    List<Tuple<int, int>> GetOutsideLimits()
    {

        List<Tuple<int, int>> OutsideLimits = new List<Tuple<int, int>>();

        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                if (j + 1 < this.height)
                {
                    if (grid.GetCellType(i,j) == cellType.VACIO && grid.GetCellType(i,j+1) != cellType.VACIO)
                        OutsideLimits.Add(new Tuple<int, int>(i, j));
                        
                    if (grid.GetCellType(i,j) != cellType.VACIO && grid.GetCellType(i,j+1) == cellType.VACIO)
                        OutsideLimits.Add(new Tuple<int, int>(i, j + 1));
                }
            }
        }

        for (int j = 0; j < this.height; j++)
        {
            for (int i = 0; i < this.width; i++)
            {
                if (i + 1 < this.width)
                {
                    if (grid.GetCellType(i,j) == cellType.VACIO && grid.GetCellType(i+1,j) != cellType.VACIO)
                        OutsideLimits.Add(new Tuple<int, int>(i, j));
                        
                    if (grid.GetCellType(i,j) != cellType.VACIO && grid.GetCellType(i+1,j) == cellType.VACIO)
                        OutsideLimits.Add(new Tuple<int, int>(i+1, j));
                }
            }
        }

        return OutsideLimits;
    }

    List<Tuple<int, int>> GetOutsideLimitsFromSpecificType(cellType cT)
    {

        List<Tuple<int, int>> OutsideLimitsSpecific = new List<Tuple<int, int>>();

        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                if (grid.GetCellType(i,j) == cellType.VACIO)
                {
                    if (j + 1 < this.height)
                    {
                        if(grid.GetCellType(i,j+1) == cT)
                        {
                            OutsideLimitsSpecific.Add(new Tuple<int, int>(i, j));
                        }
                    }
                    if (j - 1 > 0)
                    {
                        if(grid.GetCellType(i,j-1) == cT)
                        {
                            OutsideLimitsSpecific.Add(new Tuple<int, int>(i, j));
                        }
                    }
                }
            }
        }

        for (int j = 0; j < this.height; j++)
        {
            for (int i = 0; i < this.width; i++)
            {
                if (grid.GetCellType(i,j) == cellType.VACIO)
                {
                    if (i + 1 < this.width)
                    {
                        if(grid.GetCellType(i+1,j) == cT)
                        {
                            OutsideLimitsSpecific.Add(new Tuple<int, int>(i, j));
                        }
                    }
                    if (i - 1 > 0)
                    {
                        if(grid.GetCellType(i-1,j) == cT)
                        {
                            OutsideLimitsSpecific.Add(new Tuple<int, int>(i, j));
                        }
                    }
                }
            }
        }

        return OutsideLimitsSpecific;
    }


    List<Tuple<int, int>> GetCornersFromSpecificType(cellType cT)
    {

        List<Tuple<int, int>> Corners = new List<Tuple<int, int>>();

        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                if (grid.GetCellType(i,j) == cellType.VACIO)
                {

                    if (j + 1 < this.height && i + 1 < this.width)
                    {
                        if(grid.GetCellType(i+1,j+1) == cT && grid.GetCellType(i+1,j) != cT && grid.GetCellType(i,j+1) != cT && grid.GetCellHeight(i+1,j+1) == grid.GetCellHeight(i,j)) 
                            Corners.Add(new Tuple<int, int>(i, j));
                    } 

                    if (j - 1 > 0 && i - 1 > 0)
                    {
                        if(grid.GetCellType(i-1,j-1) == cT && grid.GetCellType(i-1,j) != cT && grid.GetCellType(i,j-1) != cT && grid.GetCellHeight(i-1,j-1) == grid.GetCellHeight(i,j))
                            Corners.Add(new Tuple<int, int>(i, j));
                    } 

                    if (j - 1 > 0 && i + 1 < this.width)
                    {
                        if(grid.GetCellType(i+1,j-1) == cT && grid.GetCellType(i+1,j) != cT && grid.GetCellType(i,j-1) != cT && grid.GetCellHeight(i+1,j-1) == grid.GetCellHeight(i,j))
                            Corners.Add(new Tuple<int, int>(i, j));
                    } 

                    if (j + 1 < this.height && i - 1 > 0)
                    {
                        if(grid.GetCellType(i-1,j+1) == cT && grid.GetCellType(i-1,j) != cT && grid.GetCellType(i,j+1) != cT && grid.GetCellHeight(i-1,j+1) == grid.GetCellHeight(i,j))
                            Corners.Add(new Tuple<int, int>(i, j));
                    } 
                    
                }
            }
        }

        return Corners;
    }

    void FillIsland()
    {
        for(int i = 0; i < this.width; i++)
        {
            int idx_first = -1;
            int idx_last = -1;

            for(int j = 0; j < this.height; j++)
            {
                if(grid.GetCellType(i,j) != cellType.VACIO)
                {
                    if (idx_first == -1) idx_first = j;
                    idx_last = j;
                }
            }

            if (idx_first != -1 && idx_last != -1 && idx_first != idx_last)
            {
                for (int z = idx_first; z < idx_last; z++)
                {
                    grid.setCellType(i, z, cellType.PISO);
                }
            }

        }
    }

    void Transform_Beach()
    {
        for(int i = 0; i < this.width; i++)
        {
            int idx_first = -1;

            for(int j = 0; j < this.height; j++)
            {
                if(grid.GetCellType(i,j) != cellType.VACIO)
                {
                    for(int disp = 0; disp < beach_lenght; disp ++)
                    {
                        if (j + disp < this.height)
                            if (grid.GetCellType(i,j + disp) == cellType.VACIO) break;
                            else grid.setCellType(i, j + disp, cellType.ARENA);

                    }
                    break;
                }
            }


        }

        foreach (var item in GetOutsideLimitsFromSpecificType(cellType.ARENA))
        {
            grid.setCellType(item.Item1, item.Item2, cellType.TRANSICION, -1);
        }

        foreach (var item in GetCornersFromSpecificType(cellType.ARENA))
        {
            grid.setCellType(item.Item1, item.Item2, cellType.TRANSICIONESQ, -1);
        }


        foreach(var item in GetOutsideLimitsFromSpecificType(cellType.TRANSICION))
        {
            grid.setCellType(item.Item1, item.Item2, cellType.TRANSICION, -2);
        }

        foreach (var item in GetCornersFromSpecificType(cellType.TRANSICION))
        {
            grid.setCellType(item.Item1, item.Item2, cellType.TRANSICIONESQ, -2);
        }

        foreach(var item in GetOutsideLimitsFromSpecificType(cellType.TRANSICION))
        {
            grid.setCellType(item.Item1, item.Item2, cellType.TRANSICION, -3);
        }

        foreach (var item in GetCornersFromSpecificType(cellType.TRANSICION))
        {
            grid.setCellType(item.Item1, item.Item2, cellType.TRANSICIONESQ, -3);
        }

        foreach(var item in GetOutsideLimitsFromSpecificType(cellType.TRANSICION))
        {
            grid.setCellType(item.Item1, item.Item2, cellType.TRANSICION, -4);
        }

        foreach (var item in GetCornersFromSpecificType(cellType.TRANSICION))
        {
            grid.setCellType(item.Item1, item.Item2, cellType.TRANSICIONESQ, -4);
        }

        

    }

    void BuildWorld()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                int cellHeight = grid.GetCellHeight(x,y);
                bool left = false, right = false, up = false, down = false;

                switch(grid.GetCellType(x,y))
                {
                    case cellType.PISO:
                        Instantiate(terrainPrefabs[Random.Range(0,terrainPrefabs.Length)], grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellSize,0), Quaternion.Euler(0,0,0));
                        PlaceDetail(x,y);
                        break;

                    case cellType.ARENA:
                        Instantiate(prefabPlaya, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellSize,0), Quaternion.Euler(0,0,0));
                        PlaceDetailPlaya(x,y);
                        break;

                    case cellType.ARENAMOJADA:
                        Instantiate(prefabArenaMojada, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellSize,0), Quaternion.Euler(0,0,0));
                        break;

                    case cellType.TRANSICION:
                        
                        

                        if (x-1 > 0) if (grid.GetCellType(x-1, y) != cellType.VACIO && grid.GetCellHeight(x-1, y) > grid.GetCellHeight(x,y)) left = true;
                        if (x+1 < width) if (grid.GetCellType(x+1, y) != cellType.VACIO && grid.GetCellHeight(x+1, y) > grid.GetCellHeight(x,y)) right = true;
                        if (y+1 < height)if (grid.GetCellType(x, y+1) != cellType.VACIO && grid.GetCellHeight(x, y+1) > grid.GetCellHeight(x,y)) up = true;
                        if (y-1 > 0) if (grid.GetCellType(x, y-1) != cellType.VACIO && grid.GetCellHeight(x, y-1) > grid.GetCellHeight(x,y)) down = true;

                        //if (left && up && right) Instantiate(Borde1Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.172f,0,0));
                        //if (up && right && down) Instantiate(Borde1Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.172f,0,0));
                        //if (right && down && left) Instantiate(Borde1Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.172f,0,0));
                        //if (down && left && up) Instantiate(Borde1Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.172f,0,0));

                        if (left && up) Instantiate(Borde2Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.764f,0,0));
                        else if (up && right) Instantiate(Borde2Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.764f,90f,0));
                        else if (right && down) Instantiate(Borde2Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.764f,180f,0));
                        else if (down && left) Instantiate(Borde2Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.764f,270f,0));
                        
                        else if (up) Instantiate(Borde1Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.172f,0,0));
                        else if (right) Instantiate(Borde1Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.172f,90f,0));
                        else if (down) Instantiate(Borde1Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.172f,180f,0));
                        else if (left) Instantiate(Borde1Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.172f,270f,0));
                        //if (up)Instantiate(Borde1Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.172f,0,0));
                        //if (down)    Instantiate(Borde1Prefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight,0), Quaternion.Euler(-7.172f,180f,0));
                        break;

                    case cellType.TRANSICIONESQ:

                        if (x-1 > 0) if (grid.GetCellType(x-1, y) == cellType.TRANSICION) left = true;
                        if (x+1 < width) if (grid.GetCellType(x+1, y) == cellType.TRANSICION) right = true;
                        if (y+1 < height) if (grid.GetCellType(x, y+1) == cellType.TRANSICION) up = true;
                        if (y-1 > 0) if (grid.GetCellType(x, y-1) == cellType.TRANSICION) down = true;

                        if (left && up) 
                            Instantiate(EsquinaPrefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight - 0.12f,0), Quaternion.Euler(-5.798f,0,-5.798f));
                        else if (up && right) 
                            Instantiate(EsquinaPrefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight - 0.12f,0), Quaternion.Euler(-5.798f,90f,-5.798f));
                        else if (right && down) 
                            Instantiate(EsquinaPrefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight - 0.12f,0), Quaternion.Euler(-5.798f,180f,-5.798f));
                        else if (down && left) 
                            Instantiate(EsquinaPrefab, grid.GetCellCenter(x, y) + new Vector3(0, cellHeight * cellsHeight - 0.12f,0), Quaternion.Euler(-5.798f,270f,-5.798f));
                        break;

                    default:
                        break;
                }
            }
        }
    }

    void PlaceDetail(int x, int y)
    {
        float detailRate = 0.25f;
        float prob = Random.Range(0,100) / 100f;

        if (prob < detailRate)
            Instantiate(detailsPrefabs[Random.Range(0,detailsPrefabs.Length)], grid.GetCellCenter(x, y), Quaternion.Euler(0,0,0));
        
    }

    void PlaceDetailPlaya(int x, int y)
    {
        float detailRate = 0.10f;
        float prob = Random.Range(0,100) / 100f;

        if (prob < detailRate)
            Instantiate(detailsPlayaPrefabs[0], grid.GetCellCenter(x, y) + new Vector3(0,0.18f,0), Quaternion.Euler(0,0,0));
        
    }


    public static List<Tuple<int, int>> UnirPuntoCircular(Tuple<int, int> puntoInicio, Tuple<int, int> puntoDestino, int paso = 1)
    {
        if (puntoInicio.Item1 == puntoDestino.Item1 && puntoInicio.Item2 == puntoDestino.Item2)
        {
            return new List<Tuple<int, int>> { puntoInicio };
        }

        // Calcula el ángulo entre los puntos
        float angulo = Mathf.Atan2(puntoDestino.Item2 - puntoInicio.Item2, puntoDestino.Item1 - puntoInicio.Item1);

        // Calcula la distancia circular entre los puntos
        float distanciaCircular = Mathf.Sqrt(Mathf.Pow(puntoDestino.Item1 - puntoInicio.Item1, 2) + Mathf.Pow(puntoDestino.Item2 - puntoInicio.Item2, 2));

        // Calcula la cantidad de pasos necesarios
        int numPasos = (int)(distanciaCircular / paso);

        // Calcula los puntos intermedios
        List<Tuple<int, int>> puntosIntermedios = new List<Tuple<int, int>>();
        for (int i = 1; i <= numPasos; i++)
        {
            float x = puntoInicio.Item1 + i * paso * Mathf.Cos(angulo) * 10;
            float y = puntoInicio.Item2 + i * paso * Mathf.Sin(angulo) * 10;
            puntosIntermedios.Add(new Tuple<int, int>(Convert.ToInt32(Mathf.Round(x)), Convert.ToInt32(Mathf.Round(y))));
        }

        return puntosIntermedios;
    }

    private static List<int> AngulosNotables = new List<int> { 0, 45, 90, 135, 180, 225, 270, 315 };

    // Metodo para definir los limites del mapa.
    public List<Tuple<int, int>> PasosEstocasticos(Tuple<int, int> puntoInicio, Tuple<int, int> puntoDestino)
    {
        List<Tuple<int, int>> puntosIntermedios = new List<Tuple<int, int>>();

        int actual_x = puntoInicio.Item1;
        int actual_y = puntoInicio.Item2;

        while (actual_x != puntoDestino.Item1 || actual_y != puntoDestino.Item2)
        {
            double angle_rad = Math.Atan2(puntoDestino.Item2 - actual_y, puntoDestino.Item1 - actual_x);
            double angle_deg = Math.Round(angle_rad * (180.0 / Math.PI));
            if (angle_deg < 0)
            {
                angle_deg += 360;
            }

            List<double> diffs = new List<double>();
            List<double> to_inverse = new List<double>();
            foreach (int angulo in AngulosNotables)
            {
                double diff = Math.Abs(angulo - angle_deg);
                if (diff < 10)
                {
                    diff = 10.0;
                }
                diffs.Add(diff);
                to_inverse.Add(1.0 / diff);
            }

            double suma = to_inverse.Sum();

            List<double> porcentajes_finales = new List<double>();
            foreach (double inverse in to_inverse)
            {
                porcentajes_finales.Add(inverse / suma);
            }

            List<double> lista_cumulativa = new List<double>();
            for (int i = 0; i < porcentajes_finales.Count; i++)
            {
                lista_cumulativa.Add(porcentajes_finales.GetRange(0, i + 1).Sum());
            }

            float prob = Random.Range(0, 100) / 100f;

            for (int i = 0; i < lista_cumulativa.Count; i++)
            {
                if (prob <= lista_cumulativa[i])
                {
                    Tuple<int, int> move = AngleToMove(AngulosNotables[i]);
                    actual_x += move.Item1;
                    actual_y += move.Item2;

                    if (actual_x >= 0 && actual_x < this.width && actual_y >= 0 && actual_y < this.height)
                    {
                        //Console.WriteLine(actual_x + " " + actual_y);
                        puntosIntermedios.Add(new Tuple<int, int>(actual_x, actual_y));
                    }
                    break;
                }
            }
        }

        return puntosIntermedios;
    }

    private static Tuple<int, int> AngleToMove(int angle)
    {
        switch (angle)
        {
            case 0:
                return Tuple.Create(1, 0);
            case 45:
                return Tuple.Create(1, 1);
            case 90:
                return Tuple.Create(0, 1);
            case 135:
                return Tuple.Create(-1, 1);
            case 180:
                return Tuple.Create(-1, 0);
            case 225:
                return Tuple.Create(-1, -1);
            case 270:
                return Tuple.Create(0, -1);
            case 315:
                return Tuple.Create(1, -1);
            default:
                return Tuple.Create(0, 0);
        }
    }





}




