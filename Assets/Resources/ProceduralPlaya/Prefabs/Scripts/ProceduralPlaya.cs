using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProceduralPlaya : MonoBehaviour
{   
    private int left_limit = -300, right_limit = 300;
    private int up_limit = 300, down_limit = -300;
    // Procedural Playa
    [Header("Configuracion Terreno Playa")]
    public int seed = -1, octaves = 5;
    public float freq = 5f, redistribution = 0.5f, persistance = 0.5f, lacunarity = 2f;
    private List<float> heights = new List<float>();

    [Header("Config Detalles Mar")]

    public int cantidadDetalles = 20;

    [Header("Config Celdas")]

    public int cellsWidth = 25;
    public int cellsHeight = 25;
    public int cellsSize = 7;

    private Vector3 posicionOptima = new Vector3(0,0,0);
    public Grid grid;



    private Vector2[] centrosArena;
    static float r = 10;
    float denom = Mathf.Sqrt(r * 0.5f);
    public int cantidadBasura = 100; 
    public int cantidadPersonas = 20;

    private UnityEngine.Object[] detallesMar;

    private Color[] coloresPelo = {Color.black, Color.gray, Color.yellow, new Color(0.7f, 0.5f, 0f, 1f)};
    private Color[] coloresPiel = {new Color(141f/255f, 85f/255f, 36f/255f, 1f), 
                                    new Color(198f/255f, 134f/255f, 66f/255f, 1f), 
                                    new Color(224f/255f, 172f/255f, 105f/255f, 1f),
                                    new Color(241f/255f, 194f/255f, 125f/255f, 1f),
                                    new Color(255f/255f, 219f/255f, 172f/255f, 1f)};

    private Object[] nubes;
                                

    private float tierraAltura = -80f;

    private GameObject contenedorBasura;
    private GameObject contenedorCiudad;
    private GameObject contenedorAnimales;

    public int cantidadnubes = 20;
    private int nubesActuales = 0;
    private List<GameObject> nubesLista = new List<GameObject>();

    private List<Vector2> vectorProhibido = new List<Vector2>();

    void Start()
    {   
        Random.seed = seed;

        contenedorBasura = new GameObject("ContenedorBasura");
        contenedorCiudad = new GameObject("contenedorCiudad");
        contenedorAnimales = new GameObject("contenedorAnimales");
        
        centrosArena = new Vector2[Random.Range(1,3)];
        Debug.Log(centrosArena.Length);

        for (int i = 0; i < centrosArena.Length; i++)
        {
            centrosArena[i] = new Vector2(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        }

        if (centrosArena.Length == 1)
            tierraAltura=-76f;
        else if (centrosArena.Length == 2)
            tierraAltura=-76.6f;



        detallesMar = Resources.LoadAll("ProceduralPlaya/Prefabs/DetallesMar");
        
        GameObject planoTierra = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Planos/Plano64x64_Pref");
        //var temp = Instantiate(planoTierra, new Vector3(Random.Range(-100,100),-81f,Random.Range(-100,100)), Quaternion.identity);
        var temp = Instantiate(planoTierra, new Vector3(0, tierraAltura,0), Quaternion.identity);
        temp.transform.localScale = new Vector3(Random.Range(3,4),1,Random.Range(3,4));
        ShapeTerrain(temp);
        PlaceComplementaryTerrain();
        grid = new Grid(cellsWidth, cellsHeight, cellsSize, posicionOptima.x, posicionOptima.z);
        AddDetailsUp();
        PlaceSeaDetails();

        GameObject planoAgua = Resources.Load<GameObject>("ProceduralPlaya/Mar/PlanoAgua_R");
        Instantiate(planoAgua, new Vector3(0,0,0), Quaternion.identity);

        PlaceBeachDetails();
        PlaceSeaTrash();

        nubes = Resources.LoadAll("ProceduralPlaya/Prefabs/Nubes");

        for (int i = 0; i < cantidadnubes; i++)
        {
            nubesLista.Add((GameObject)Instantiate(nubes[Random.Range(0, nubes.Length)], new Vector3(Random.Range(-400,400), Random.Range(90,120), Random.Range(-400,400)), Quaternion.identity));
            nubesActuales ++;
        }  


        // Terrenos extras
        for (int i = 0; i < Random.Range(3,6); i++)
        {
            int posx = 0;
            int posy = 0;
            if (i == 0)
            {
                posx = Random.Range(-400,-600);
                posy = Random.Range(-300,300);
            } else if(i == 1)
            {
                posx = Random.Range(-300,300);
                posy = Random.Range(-200,400);
            } else if(i == 2)
            {
                posx = Random.Range(-300,300);
                posy = Random.Range(200,400);
            } else if(i == 3)
            {
                posx = Random.Range(400,600);
                posy = Random.Range(-200,400);
            } else
            {
                int a = Random.Range(0,1);
                if (a == 0)
                {
                    posx = Random.Range(400,600);
                    posy = Random.Range(-200,400);
                } else{
                    posx = Random.Range(-300,300);
                    posy = Random.Range(-200,400);
                }
            }

            temp = Instantiate(planoTierra, new Vector3(posx, -77.6f, posy), Quaternion.identity);
            temp.transform.localScale = new Vector3(Random.Range(2,4),1,Random.Range(2,4));
            ShapeTerrain(temp, 64f * i);

        }

        Invoke("GenerarGaviotasVolando", Random.Range(0,5)); // En proceso

    }

    void GenerarGaviotasVolando()
    {
        Object[] gavs = Resources.LoadAll("ProceduralPlaya/Prefabs/GaviotasVolando");
        Vector3 pos;

        if (Random.Range(0,1) == 0)
            pos = new Vector3(Random.Range(-400, 400), 20f, -400);
        else
            pos = new Vector3(-400, 20f, Random.Range(-400, 400));

        Vector3 pos2 = pos;
        pos2.y = 0;

        var gaviota = (GameObject)Instantiate(gavs[Random.Range(0,gavs.Length)], pos, Quaternion.Euler(0,0,0));
        gaviota.transform.LookAt(new Vector3(0,20f,0));

        Invoke("GenerarGaviotasVolando", Random.Range(10,40));
    }

    private void Update() {
        Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);
        Debug.DrawRay(posicionOptima, -1*transform.up * 1000, Color.magenta);

        if (nubesActuales < cantidadnubes)
        {
            nubesLista.Add((GameObject)Instantiate(nubes[Random.Range(0, nubes.Length)], new Vector3(-1000f, 100f, Random.Range(-400,400)), Quaternion.identity));
            nubesActuales++;
        } else{

            for (int i = nubesLista.Count - 1; i >= 0; i--)
            {
                if (nubesLista[i].transform.position.x > 600)
                {
                    DestroyImmediate(nubesLista[i]);
                    nubesLista.RemoveAt(i);
                    nubesActuales --;
                }
            }

        }
    }



    float calculateDistance(float nx, float nz)
    {
        float valor = 0f;

        foreach (var centro in centrosArena)
        {
            valor += Mathf.Sqrt(
                Mathf.Pow(nx - centro.x, 2) + Mathf.Pow(nz - centro.y, 2)
                ) / (denom);
           
        }

        valor = 1f - valor;
        return valor;
    }
    private void ShapeTerrain(GameObject terrenoPlaya, float offset = 0)
    {
        heights = new List<float>();
        Mesh mesh = terrenoPlaya.GetComponent<MeshFilter>().mesh;

        
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            //Debug.Log("x: " + (float) mesh.vertices[i].x + ", z: "+ (float) mesh.vertices[i].z);
            float scale = 50f;
            float amplitude = 1f;
            float noiseHeight = 0f;
            float frequency = freq;

            for (int o = 0; o < octaves; o++)
            {
                float xValue = (float) mesh.vertices[i].x / scale * frequency + 64/2;
                float zValue = (float) mesh.vertices[i].z / scale * frequency + 64/2;

                float perlinValue = Mathf.PerlinNoise(xValue + seed + offset, zValue + seed + offset);

                noiseHeight += perlinValue * amplitude;
                
                amplitude *= persistance;
                frequency *= lacunarity;
            }
            
            
            float nx = (float) mesh.vertices[i].x / 64;
            float nz = (float) mesh.vertices[i].z / 64;
            

            //heights.Add(noiseHeight * (1 - (Mathf.Sqrt((nx * nx) + (nz * nz)) / denom)));
            //heights.Add(noiseHeight);
            heights.Add(noiseHeight * calculateDistance(nx, nz));
            //if (vertices[i].y <= 0f) vertices[i].y = 0;
            //else vertices[i].y = Mathf.Pow(noiseHeight * (1 - (Mathf.Sqrt((nx * nx) + (nz * nz)) / denom)), redistribution); 
            //Debug.Log(vertices[i].y)
            

        }

    

        
        Vector3 [] vertices = mesh.vertices;

        
        float min = heights.Min();
        float max = heights.Max();
        float percentage = 0.7f;
        float range = max - min;

        float limit = min + range * percentage;


        float scale_ver = 100f;
        float limit_heightmap = 3f;


        for (int i = 0; i < heights.Count; i++)
        {   
            

            // normalization
            heights[i] = (heights[i] - min) / (max - min);
            heights[i] = Mathf.Pow(heights[i], redistribution);

            Debug.Log(heights[i] + " , " + heights[i] * scale_ver);

            if (tierraAltura + heights[i] * scale_ver > limit_heightmap)
                vertices[i].y = limit_heightmap - tierraAltura + Random.Range(-1.5f,1.5f);
            else
                vertices[i].y = heights[i] * scale_ver;
        }

        mesh.vertices = vertices;

        // Recalcular las normales y la información de la malla
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshCollider meshCollider = terrenoPlaya.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;



    }


    void PlaceComplementaryTerrain()
    {
        bool posicionarNuevo = false;
        while(!posicionarNuevo)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y > 0)
            {
                Ray ray2 = new Ray(ray.origin + new Vector3(1,0,0) * cellsSize * cellsWidth/2, ray.direction);

                if (Physics.Raycast(ray2, out RaycastHit hit2) && hit2.transform.name == "Plano64x64_Pref(Clone)" && hit2.point.y < 0)
                {
                    posicionOptima = ray2.origin;
                    posicionarNuevo = true;
                    break;
                }

                Ray ray3 = new Ray(ray.origin - new Vector3(1,0,0) * cellsSize * cellsWidth/2, ray.direction);
                if (Physics.Raycast(ray3, out RaycastHit hit3) && hit3.transform.name == "Plano64x64_Pref(Clone)" && hit3.point.y < 0)
                {
                    posicionOptima = ray3.origin;
                    posicionarNuevo = true;
                    break;
                }

                Ray ray4 = new Ray(ray.origin + new Vector3(0,0,1) * cellsSize * cellsWidth/2, ray.direction);
                if (Physics.Raycast(ray4, out RaycastHit hit4) && hit4.transform.name == "Plano64x64_Pref(Clone)" && hit4.point.y < 0)
                {
                    posicionOptima = ray4.origin;
                    posicionarNuevo = true;
                    break;
                }

                Ray ray5 = new Ray(ray.origin - new Vector3(0,0,1) * cellsSize * cellsWidth/2, ray.direction);
                if (Physics.Raycast(ray5, out RaycastHit hit5) && hit5.transform.name == "Plano64x64_Pref(Clone)" && hit5.point.y < 0)
                {
                    posicionOptima = ray5.origin;
                    posicionarNuevo = true;
                    break;
                }

            }

        }

        GameObject cuboBase = Instantiate(Resources.Load<GameObject>("ProceduralPlaya/Prefabs/CuboBase"));
        cuboBase.transform.position = posicionOptima + new Vector3(0,-1,0) * posicionOptima.y + new Vector3(0,-44,0);
        cuboBase.transform.localScale = new Vector3(cellsWidth * cellsSize, 100,cellsHeight * cellsSize); 
    }

    float probEscalera = 0.1f;
    void AddDetailsUp()
    {
        GameObject reja1 = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Reja1");
        GameObject reja2 = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Reja2");
        GameObject escaleras = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Escaleras");

        for (int i = 0; i < cellsWidth; i++)
        {
            for(int j = 0; j < cellsHeight; j++)
            {

                if(i == 0 && j == 0) 
                    Instantiate(reja2, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,90,0));
                else if (i == 0 && j == cellsHeight - 1)
                    Instantiate(reja2, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,180,0));
                else if (i == cellsWidth - 1 && j == 0)
                    Instantiate(reja2, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,0,0));
                else if (i == cellsWidth - 1 && j == cellsHeight - 1)
                    Instantiate(reja2, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,270,0));
                else if( (i == 0 || i == cellsWidth - 1) || (j == 0 || j == cellsHeight - 1))
                {
                    Vector3 centro1;
                    Vector3 centro2;
                    if (j == 0)
                    {
                        centro1 = grid.GetCellCenter(i,j);
                        centro2 = grid.GetCellCenter(i,j+1);
                        Vector3 diferencia = centro1 - centro2;

                        Ray ray = new Ray( centro1 + (2*diferencia) + new Vector3(0,100f,0), -1 * transform.up);

                        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y > 0)
                        {
                            if (Random.Range(0f,1f) < probEscalera)
                            {
                                Instantiate(escaleras, new Vector3((centro1+diferencia).x, 4.27f, (centro1+diferencia).z + 2.1f), Quaternion.Euler(0,270,0));
                                vectorProhibido.Add(new Vector2(i,j));
                            } else{
                                Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,270,0));
                            }
                            Debug.Log("j0 No hay arena enfrente.");
                        } else {
                            Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,270,0));
                        }

                    }
                    else if (j == cellsHeight - 1)
                    {
                        centro1 = grid.GetCellCenter(i,j);
                        centro2 = grid.GetCellCenter(i,j-1);
                        Vector3 diferencia = centro1 - centro2;

                        Ray ray = new Ray( centro1 + (2*diferencia) + new Vector3(0,100f,0), -1 * transform.up);

                        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y > 0)
                        {
                            if (Random.Range(0f,1f) < probEscalera)
                            {
                                Instantiate(escaleras, new Vector3((centro1+diferencia).x, 4.27f, (centro1+diferencia).z - 2.1f), Quaternion.Euler(0,90,0));
                                vectorProhibido.Add(new Vector2(i,j));
                            } else{
                                Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,90,0));
                            }
                            Debug.Log("ja No hay arena enfrente.");
                        } else {
                            Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,90,0));
                        }
                    }
                    else if (i == 0)
                    {
                        centro1 = grid.GetCellCenter(i,j);
                        centro2 = grid.GetCellCenter(i+1,j);
                        Vector3 diferencia = centro1 - centro2;

                        Ray ray = new Ray( centro1 + (2*diferencia) + new Vector3(0,100f,0), -1 * transform.up);

                        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y > 0)
                        {
                            if (Random.Range(0f,1f) < probEscalera)
                            {
                                Instantiate(escaleras, new Vector3((centro1+diferencia).x + 2.1f, 4.27f, (centro1+diferencia).z), Quaternion.Euler(0,0,0));
                                vectorProhibido.Add(new Vector2(i,j));
                            } else{
                                Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,0,0));
                            }
                            Debug.Log("i0 No hay arena enfrente.");
                        } else {
                            Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.identity);
                        }
                    }
                        
                    else
                    {
                        centro1 = grid.GetCellCenter(i,j);
                        centro2 = grid.GetCellCenter(i-1,j);
                        Vector3 diferencia = centro1 - centro2;

                        Ray ray = new Ray( centro1 + (2*diferencia) + new Vector3(0,100f,0), -1 * transform.up);

                        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y > 0)
                        {
                            if (Random.Range(0f,1f) < probEscalera)
                            {
                                Instantiate(escaleras, new Vector3((centro1+diferencia).x - 2.1f, 4.27f, (centro1+diferencia).z), Quaternion.Euler(0,180,0));
                                vectorProhibido.Add(new Vector2(i,j));
                            } else{
                                Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,180,0));
                            }
                            Debug.Log("ia No hay arena enfrente.");
                        } else {
                            Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,180,0));
                        }
                    } 
                        
                }

            }
        }



        
        GameObject Pueblo6x6 = Resources.Load<GameObject>("ProceduralPlaya/Pueblo6x6");
        GameObject Pueblo7x7 = Resources.Load<GameObject>("ProceduralPlaya/Pueblo7x7");
        GameObject Pueblo8x8 = Resources.Load<GameObject>("ProceduralPlaya/Pueblo8x8");
        GameObject Pueblo7x6 = Resources.Load<GameObject>("ProceduralPlaya/Pueblo7x6");
        GameObject Pueblo8x6 = Resources.Load<GameObject>("ProceduralPlaya/Pueblo8x6");
        GameObject Pueblo8x7 = Resources.Load<GameObject>("ProceduralPlaya/Pueblo8x7");


        int largoo = Random.Range(6,8);
        int anchoo = Random.Range(6,8);


        Vector2 centro_xyP = new Vector2(Random.Range(1 + (int) Mathf.Ceil((largoo)/ 2),cellsWidth - 2 - (int) Mathf.Ceil((largoo)/ 2)), 
                                        Random.Range(1 + (int) Mathf.Ceil((anchoo)/ 2), cellsHeight - 2 - (int) Mathf.Ceil((anchoo)/ 2))); 

        float altoinst = -7.3f;

        
        if (largoo == anchoo)
        {
            if (largoo == 6)
            Instantiate(Pueblo6x6, grid.GetCellCenter((int)centro_xyP.x , (int)centro_xyP.y ) + new Vector3(0,altoinst,0), Quaternion.Euler(0,0,0));
            else if (largoo == 7)
            Instantiate(Pueblo7x7, grid.GetCellCenter((int)centro_xyP.x , (int)centro_xyP.y ) + new Vector3(0,altoinst,0), Quaternion.Euler(0,0,0));
            else 
            Instantiate(Pueblo8x8, grid.GetCellCenter((int)centro_xyP.x , (int)centro_xyP.y ) + new Vector3(0,altoinst,0), Quaternion.Euler(0,0,0));
        } else{
            if (largoo == 6)
            {
                if (anchoo == 7)
                {
                    Instantiate(Pueblo7x6, grid.GetCellCenter((int)centro_xyP.x , (int)centro_xyP.y ) + new Vector3(0,altoinst,0), Quaternion.Euler(0,90,0));
                }
                else if (anchoo == 8)
                {
                    Instantiate(Pueblo8x6, grid.GetCellCenter((int)centro_xyP.x , (int)centro_xyP.y ) + new Vector3(0,altoinst,0), Quaternion.Euler(0,90,0));
                }
            }
            else if (largoo == 7)
            {
                if (anchoo == 6)
                {
                    Instantiate(Pueblo7x6, grid.GetCellCenter((int)centro_xyP.x , (int)centro_xyP.y ) + new Vector3(0,altoinst,0), Quaternion.Euler(0,0,0));
                }
                else if (anchoo == 8)
                {
                    Instantiate(Pueblo8x7, grid.GetCellCenter((int)centro_xyP.x , (int)centro_xyP.y ) + new Vector3(0,altoinst,0), Quaternion.Euler(0,90,0));
                }
            } else{
                if (anchoo == 6)
                {
                    Instantiate(Pueblo8x6, grid.GetCellCenter((int)centro_xyP.x , (int)centro_xyP.y ) + new Vector3(0,altoinst,0), Quaternion.Euler(0,0,0));
                }
                else if (anchoo == 7)
                {
                    Instantiate(Pueblo8x7, grid.GetCellCenter((int)centro_xyP.x , (int)centro_xyP.y ) + new Vector3(0,altoinst,0), Quaternion.Euler(0,0,0));
                }
            }
        }



            grid.SetCellDontBuild((int)centro_xyP.x , (int)centro_xyP.y);
            
            largoo = (int) Mathf.Ceil((largoo)/ 2);
            anchoo = (int) Mathf.Ceil((anchoo)/ 2);

            for (int j = 1; j < largoo + 1; j++)
            {

                for (int a = 1; a < anchoo + 1; a++)
                {
                    if(centro_xyP.y - j > 1)
                    {
                        if(centro_xyP.x + a < cellsWidth - 2)
                            grid.SetCellDontBuild((int)centro_xyP.x + a, (int)centro_xyP.y - j);
                        if(centro_xyP.x - a > 1)
                            grid.SetCellDontBuild((int)centro_xyP.x - a, (int)centro_xyP.y - j);
                    }
                    if(centro_xyP.y + j < cellsHeight -2)
                    {
                        if(centro_xyP.x + a < cellsWidth - 2)
                            grid.SetCellDontBuild((int)centro_xyP.x + a, (int)centro_xyP.y + j);
                        if(centro_xyP.x - a > 1)
                            grid.SetCellDontBuild((int)centro_xyP.x - a, (int)centro_xyP.y + j);
                    }
                }
                if(centro_xyP.y - j > 1)
                    grid.SetCellDontBuild((int)centro_xyP.x , (int)centro_xyP.y - j);
                if(centro_xyP.y + j < cellsHeight -2)
                    grid.SetCellDontBuild((int)centro_xyP.x , (int)centro_xyP.y + j);
            }

            for (int a = 1; a < anchoo + 1; a++)
            {
                if(centro_xyP.x + a < cellsWidth - 2)
                    grid.SetCellDontBuild((int)centro_xyP.x + a, (int)centro_xyP.y);
                if(centro_xyP.x - a > 1)
                    grid.SetCellDontBuild((int)centro_xyP.x - a, (int)centro_xyP.y);
            }       
        
        

        
        GameObject cuboBase = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Plaza");
        UnityEngine.Object[] detallesPlaza = Resources.LoadAll("ProceduralPlaya/Prefabs/Terreno");

        float prob_palmera = 0.4f;

        int plazas = 3;
        float altura = 6f;
        float altura_palmera = 6f;
        for (int i = 0; i < plazas; i++)
        {
            Vector2 centro_xy = new Vector2(Random.Range(1,cellsWidth - 2), Random.Range(1, cellsHeight - 2)); 
            int largo = Random.Range(2,4);
            int ancho = Random.Range(2,4);

            if (grid.GetCellDontBuild( (int)centro_xy.x , (int)centro_xy.y ) != true)
            {
            Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x , (int)centro_xy.y ) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
            PonerPalmera(prob_palmera, (int)centro_xy.x , (int)centro_xy.y, altura_palmera);
            }


            for (int j = 1; j < largo + 1; j++)
            {

                for (int a = 1; a < ancho + 1; a++)
                {
                    if(centro_xy.y - j > 1)
                    {
                        if(centro_xy.x + a < cellsWidth - 2 && grid.GetCellDontBuild( (int)centro_xy.x + a, (int)centro_xy.y - j ) != true)
                        {
                            Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x + a, (int)centro_xy.y - j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                            PonerPalmera(prob_palmera, (int)centro_xy.x + a , (int)centro_xy.y - j, altura_palmera);
                        }
                        if(centro_xy.x - a > 1  && grid.GetCellDontBuild( (int)centro_xy.x - a, (int)centro_xy.y - j ) != true)
                        {
                            Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x - a, (int)centro_xy.y - j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                            PonerPalmera(prob_palmera, (int)centro_xy.x - a , (int)centro_xy.y - j, altura_palmera);
                        }
                    }
                    if(centro_xy.y + j < cellsHeight -2)
                    {
                        if(centro_xy.x + a < cellsWidth - 2  && grid.GetCellDontBuild( (int)centro_xy.x + a, (int)centro_xy.y + j ) != true)
                        {
                            Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x + a, (int)centro_xy.y + j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                            PonerPalmera(prob_palmera, (int)centro_xy.x + a , (int)centro_xy.y + j, altura_palmera);
                        }
                        if(centro_xy.x - a > 1  && grid.GetCellDontBuild( (int)centro_xy.x - a, (int)centro_xy.y + j ) != true)
                        {
                            Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x - a, (int)centro_xy.y + j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                        PonerPalmera(prob_palmera, (int)centro_xy.x - a , (int)centro_xy.y + j, altura_palmera);
                        }
                    }
                }
                if(centro_xy.y - j > 1  && grid.GetCellDontBuild( (int)centro_xy.x, (int)centro_xy.y - j ) != true)
                {   
                Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x, (int)centro_xy.y - j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                PonerPalmera(prob_palmera, (int)centro_xy.x , (int)centro_xy.y - j, altura_palmera);
                }
                if(centro_xy.y + j < cellsHeight - 2  && grid.GetCellDontBuild( (int)centro_xy.x, (int)centro_xy.y + j ) != true)
                {
                Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x, (int)centro_xy.y + j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                PonerPalmera(prob_palmera, (int)centro_xy.x , (int)centro_xy.y + j, altura_palmera);
                }
            }

            for (int a = 1; a < ancho + 1; a++)
            {
                if(centro_xy.x + a < cellsWidth - 2  && grid.GetCellDontBuild( (int)centro_xy.x + a, (int)centro_xy.y ) != true)
                {
                    Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x + a, (int)centro_xy.y) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                    PonerPalmera(prob_palmera, (int)centro_xy.x + a, (int)centro_xy.y, altura_palmera);
                }
                if(centro_xy.x - a > 1  && grid.GetCellDontBuild( (int)centro_xy.x - a, (int)centro_xy.y ) != true)
                {
                    Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x - a, (int)centro_xy.y) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                    PonerPalmera(prob_palmera, (int)centro_xy.x - a, (int)centro_xy.y, altura_palmera);
                }       
            }

        }





        float prob_silla = 0.2f;
        float prob_basura = 0.1f;
        UnityEngine.Object[] sillas = Resources.LoadAll("ProceduralPlaya/Prefabs/Detalles/Borde");
        UnityEngine.Object[] basureros = Resources.LoadAll("ProceduralPlaya/Prefabs/Detalles/BasurerosBorde");

        for (int i = 0; i < cellsWidth; i++)
        {
            for(int j = 0; j < cellsHeight ; j++)
            {

                if(i == 0 && j == 0) 
                    continue;
                else if (i == 0 && j == cellsHeight - 1)
                    continue;
                else if (i == cellsWidth - 1 && j == 0)
                    continue;
                else if (i == cellsWidth - 1 && j == cellsHeight - 1)
                    continue;
                else if( (i == 0 || i == cellsWidth - 1) || (j == 0 || j == cellsHeight - 1))
                {
                    if (j == 0)
                    {
                        if (Random.Range(0f,1f) < prob_silla && !vectorProhibido.Contains(new Vector2(i,j)))
                        {
                            variarhumanos(
                                Instantiate(sillas[Random.Range(0, sillas.Length)], grid.GetCellCenter(i,j) + new Vector3(0,6,0), Quaternion.Euler(0,180,0))
                            as GameObject);
                        } else if (Random.Range(0f,1f) < prob_basura)
                            Instantiate(basureros[Random.Range(0, basureros.Length)], grid.GetCellCenter(i,j) + new Vector3(0,7,0), Quaternion.Euler(0,270,0));

                    }
                    else if (j == cellsHeight - 1)
                    {
                       
                        if (Random.Range(0f,1f) < prob_silla && !vectorProhibido.Contains(new Vector2(i,j)))
                        {
                            variarhumanos(
                            Instantiate(sillas[Random.Range(0, sillas.Length)], grid.GetCellCenter(i,j) + new Vector3(0,6,0), Quaternion.Euler(0,0,0))
                            as GameObject);
                        } else if (Random.Range(0f,1f) < prob_basura)
                            Instantiate(basureros[Random.Range(0, basureros.Length)], grid.GetCellCenter(i,j) + new Vector3(0,7,0), Quaternion.Euler(0,90,0));
                        
                    }
                    else if (i == 0)
                    {
                        
                        if (Random.Range(0f,1f) < prob_silla && !vectorProhibido.Contains(new Vector2(i,j)))
                        {
                            variarhumanos(
                            Instantiate(sillas[Random.Range(0, sillas.Length)], grid.GetCellCenter(i,j) + new Vector3(0,6,0), Quaternion.Euler(0,270,0))
                            as GameObject);
                        } else if (Random.Range(0f,1f) < prob_basura)
                            Instantiate(basureros[Random.Range(0, basureros.Length)], grid.GetCellCenter(i,j) + new Vector3(0,7,0), Quaternion.Euler(0,0,0));
                       
                    }
                        
                    else
                    {
                        if (Random.Range(0f,1f) < prob_silla && !vectorProhibido.Contains(new Vector2(i,j)))
                        {
                            variarhumanos(
                            Instantiate(sillas[Random.Range(0, sillas.Length)], grid.GetCellCenter(i,j) + new Vector3(0,6,0), Quaternion.Euler(0,90,0))
                            as GameObject);
                        } else if (Random.Range(0f,1f) < prob_basura)
                            Instantiate(basureros[Random.Range(0, basureros.Length)], grid.GetCellCenter(i,j) + new Vector3(0,7,0), Quaternion.Euler(0,180,0));

                    } 
                        
                }

            }
        }

        




    }

    void PonerPalmera(float prob, int x, int y, float altura)
    {
        UnityEngine.Object[] palmera = Resources.LoadAll("ProceduralPlaya/Prefabs/Detalles/PlantasArbol");

        if (Random.Range(0f,1f) < prob && grid.GetCellDontBuild(x,y) == false)
        {
                Instantiate(palmera[Random.Range(0,palmera.Length)], grid.GetCellCenter(x , y) + new Vector3(0,altura,0), Quaternion.Euler(0,Random.Range(0,360),0));

                grid.SetCellDontBuild(x,y);
        }
    }


    void PlaceSeaDetails()
    {
        
        for (int i = 0; i < cantidadDetalles; i++)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // If the raycast hits an object, print the name of the object to the console.
                // Debug.Log("Hit object: " + hit.transform.name);
            }
            else{
                Vector3 pos = ray.origin;
                pos.y = -5;
                Instantiate(detallesMar[Random.Range(0, detallesMar.Length)], pos, Quaternion.identity);
                // Debug.Log("Sin colision");
            }
        }
        
    }

    void PlaceSeaTrash()
    {
        int nBasuraAgua = 1000;
        UnityEngine.Object[] basuraAgua = Resources.LoadAll("ProceduralPlaya/Prefabs/DetallesPlaya/BasuraMar");
        
        for (int i = 0; i < nBasuraAgua; i++)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y < 0.5f)
                {
                    Vector3 pos = new Vector3(hit.point.x, 0, hit.point.z);
                    Instantiate(basuraAgua[Random.Range(0,basuraAgua.Length)], pos, Quaternion.Euler(0,Random.Range(0,360), 0));
                }
            }
        }
    }

    void PlaceBeachDetails()
    {
        UnityEngine.Object[] detallesPlaya = Resources.LoadAll("ProceduralPlaya/Prefabs/DetallesPlaya/Basura");

        int cont = 0;

        while (cont < cantidadBasura)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if  (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y > 1f)
            {
                Object insta = Instantiate(detallesPlaya[Random.Range(0,detallesPlaya.Length)], hit.point, Quaternion.Euler(0, Random.Range(0,360), 0), contenedorBasura.transform);

                if (insta.name == "Sillas2(Clone)" || insta.name == "Sillas1(Clone)" || insta.name == "Sillas3(Clone)" || insta.name == "Sillas4(Clone)")
                {
                    Color pelo = coloresPelo[Random.Range(0,coloresPelo.Length)];
                    Color pelo2 = coloresPelo[Random.Range(0,coloresPelo.Length)];
                    Color conjuntotodo = Random.ColorHSV();

                    foreach (Renderer rend in (insta as GameObject).GetComponentsInChildren<Renderer>())
                    {  
                        Color conjunto = Random.ColorHSV();
                        
                        foreach(var mat in rend.materials)
                            switch (mat.name)
                            {  
                                case "Sombrilla (Instance)":
                                case "Camilla (Instance)":  
                                    mat.color = conjuntotodo;
                                    break;
                                case "Shorts (Instance)":
                                case "TB_down.003 (Instance)":
                                case "TB_up (Instance)":
                                    mat.color = conjunto;
                                    break;
                                case "Material.007 (Instance)":
                                    mat.color = pelo2;
                                    break;
                                case "Material.002 (Instance)":
                                    mat.color = pelo;
                                    break;
                                case "Skin.001 (Instance)":
                                case "Skin (Instance)":
                                    mat.color = coloresPiel[Random.Range(0,coloresPiel.Length)];
                                    break;
                                case "Madera (Instance)":
                                default:
                                    break;
                            }
                            
                        
                    }
                }


                cont ++;
            }
        }


        UnityEngine.Object[] detallesHumanos = Resources.LoadAll("ProceduralPlaya/Prefabs/DetallesPlaya/NPC");
        int contHum = 0;

        while (contHum < cantidadPersonas)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if  (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y > 1f)
            {
                Object insta = Instantiate(detallesHumanos[Random.Range(0,detallesHumanos.Length)], hit.point, Quaternion.Euler(0, Random.Range(0,360), 0), contenedorBasura.transform);

                if (insta.name == "Sillas2(Clone)" || insta.name == "Sillas1(Clone)" || insta.name == "Sillas3(Clone)" || insta.name == "Sillas4(Clone)")
                {
                    Color pelo = coloresPelo[Random.Range(0,coloresPelo.Length)];
                    Color pelo2 = coloresPelo[Random.Range(0,coloresPelo.Length)];
                    Color conjuntotodo = Random.ColorHSV();

                    foreach (Renderer rend in (insta as GameObject).GetComponentsInChildren<Renderer>())
                    {  
                        Color conjunto = Random.ColorHSV();
                        
                        foreach(var mat in rend.materials)
                            switch (mat.name)
                            {  
                                case "Sombrilla (Instance)":
                                case "Camilla (Instance)":  
                                    mat.color = conjuntotodo;
                                    break;
                                case "Shorts (Instance)":
                                case "TB_down.003 (Instance)":
                                case "TB_up (Instance)":
                                    mat.color = conjunto;
                                    break;
                                case "Material.007 (Instance)":
                                    mat.color = pelo2;
                                    break;
                                case "Material.002 (Instance)":
                                    mat.color = pelo;
                                    break;
                                case "Skin.001 (Instance)":
                                case "Skin (Instance)":
                                    mat.color = coloresPiel[Random.Range(0,coloresPiel.Length)];
                                    break;
                                case "Madera (Instance)":
                                default:
                                    break;
                            }
                            
                        
                    }
                }


                contHum ++;
            }
        }

        UnityEngine.Object[] animales = Resources.LoadAll("ProceduralPlaya/Prefabs/Animales");

        int cont2 = 0;

        while (cont2 < 100)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if  (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y > 0)
            {
                Instantiate(animales[Random.Range(0,animales.Length)], hit.point, Quaternion.Euler(0, Random.Range(0,360), 0), contenedorAnimales.transform);
                cont2 ++;
            }
        }

        
    }

    void variarhumanos(GameObject insta)
    {
        Color pelo = coloresPelo[Random.Range(0,coloresPelo.Length)];
        Color conjuntotodo = Random.ColorHSV();

        foreach (Renderer rend in (insta as GameObject).GetComponentsInChildren<Renderer>())
        {  
            Color conjunto = Random.ColorHSV();
                        
                        foreach(var mat in rend.materials)
                        {
                            switch (mat.name)
                            {  
                                
                                case "Shorts (Instance)":
                                case "TB_down.003 (Instance)":
                                case "TB_up (Instance)":
                                    mat.color = conjunto;
                                    break;
                                case "Material.007 (Instance)":
                                    mat.color = pelo;
                                    break;
                                case "Material.002 (Instance)":
                                    mat.color = pelo;
                                    break;
                                case "Skin.001 (Instance)":
                                case "Skin (Instance)":
                                    mat.color = coloresPiel[Random.Range(0,coloresPiel.Length)];
                                    break;
                                default:
                                    break;
                            }
                        }
                            
                        
        }
    }

}
