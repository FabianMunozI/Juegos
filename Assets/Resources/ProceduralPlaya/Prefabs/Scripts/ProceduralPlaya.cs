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

    private UnityEngine.Object[] detallesMar;

    private Color[] coloresPelo = {Color.black, Color.gray, Color.yellow, new Color(0.7f, 0.5f, 0f, 1f)};
    private Color[] coloresPiel = {new Color(141f/255f, 85f/255f, 36f/255f, 1f), 
                                    new Color(198f/255f, 134f/255f, 66f/255f, 1f), 
                                    new Color(224f/255f, 172f/255f, 105f/255f, 1f),
                                    new Color(241f/255f, 194f/255f, 125f/255f, 1f),
                                    new Color(255f/255f, 219f/255f, 172f/255f, 1f)};

    void Start()
    {   
        centrosArena = new Vector2[Random.Range(1,3)];
        Debug.Log(centrosArena.Length);

        for (int i = 0; i < centrosArena.Length; i++)
        {
            centrosArena[i] = new Vector2(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        }



        detallesMar = Resources.LoadAll("ProceduralPlaya/Prefabs/DetallesMar");
        
        GameObject planoTierra = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Planos/Plano64x64_Pref");
        //var temp = Instantiate(planoTierra, new Vector3(Random.Range(-100,100),-81f,Random.Range(-100,100)), Quaternion.identity);
        var temp = Instantiate(planoTierra, new Vector3(0,-80f,0), Quaternion.identity);
        temp.transform.localScale = new Vector3(Random.Range(3,4),1,Random.Range(3,4));
        ShapeTerrain(temp);
        PlaceComplementaryTerrain();
        grid = new Grid(cellsWidth, cellsHeight, cellsSize, posicionOptima.x, posicionOptima.z);
        AddDetailsUp();
        PlaceSeaDetails();
        PlaceBeachDetails();

        GameObject planoAgua = Resources.Load<GameObject>("ProceduralPlaya/Mar/PlanoAgua_R");
        Instantiate(planoAgua, new Vector3(0,0,0), Quaternion.identity);

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
    private void ShapeTerrain(GameObject terrenoPlaya)
    {
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

                float perlinValue = Mathf.PerlinNoise(xValue + seed, zValue + seed);

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


        for (int i = 0; i < heights.Count; i++)
        {   
            

            // normalization
            heights[i] = (heights[i] - min) / (max - min);
            heights[i] = Mathf.Pow(heights[i], redistribution);

            Debug.Log(heights[i] + " , " + heights[i] * scale_ver);

            if (heights[i] * scale_ver > 90f)
                vertices[i].y = 90f + Random.Range(0f,.05f);
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

    private void Update()
    {
        Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);
        Debug.DrawRay(posicionOptima, -1*transform.up * 1000, Color.magenta);
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


    void AddDetailsUp()
    {
        GameObject reja1 = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Reja1");
        GameObject reja2 = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Reja2");

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
                    if (j == 0 || j == cellsHeight - 1)
                        Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,90,0));
                    else
                        Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.identity);
                }

            }
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

    void PlaceBeachDetails()
    {
        UnityEngine.Object[] detallesPlaya = Resources.LoadAll("ProceduralPlaya/Prefabs/DetallesPlaya");

        int cont = 0;

        while (cont < 1000)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if  (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y > 1f)
            {
                Object insta = Instantiate(detallesPlaya[Random.Range(0,detallesPlaya.Length)], hit.point, Quaternion.Euler(0, Random.Range(0,360), 0));

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

        UnityEngine.Object[] animales = Resources.LoadAll("ProceduralPlaya/Prefabs/Animales");

        int cont2 = 0;

        while (cont2 < 10)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if  (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Plano64x64_Pref(Clone)" && hit.point.y > 0)
            {
                Instantiate(animales[Random.Range(0,animales.Length)], hit.point, Quaternion.Euler(0, Random.Range(0,360), 0));
                cont2 ++;
            }
        }

        
    }

}
