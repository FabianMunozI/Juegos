using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProceduralPlaya : MonoBehaviour
{   
    private int left_limit = -300, right_limit = 300;
    private int up_limit = 300, down_limit = -300;
    private int seed;

    // Procedural Playa
    [Header("Configuracion Terreno Playa")]

    public int beachWidth = 256;
    public int beachHeight = 256;

    [Header("Configuracion Ciudad")]

    public int cityWidth = 25;
    public int cityHeight = 25;
    private int cellsSize = 7;

    [Header("Configuracion Detalles")]

    public int cantidadDetallesMar = 20;
    public int cantidadBasura = 100; 
    public int cantidadPersonas = 20;
    public int cantidadnubes = 20;

    // VARIABLES
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

    private Object[] nubes;

    private GameObject contenedorBasura;
    private GameObject contenedorCiudad;
    private GameObject contenedorAnimales;

    private int nubesActuales = 0;
    private List<GameObject> nubesLista = new List<GameObject>();

    private List<Vector2> vectorProhibido = new List<Vector2>();

    public GameObject misionBasuraPlaya;
    public GameObject misionTriviaPlaya;

    private float beach_pos_x = 0;
    private float beach_pos_y = 0;

    public GameObject CUBOMISION;

    private GameObject NpcsPlaya;
    void Start()
    {   
        // Inicializar semilla.
        if (!PlayerPrefs.HasKey("seedPlaya"))
        {
            seed = Random.Range(0, 10000);
            PlayerPrefs.SetInt("seedPlaya", seed);
            PlayerPrefs.Save();
        }
        seed = PlayerPrefs.GetInt("seedPlaya");
        Random.seed = seed;
        Debug.Log(seed);
        
        NpcsPlaya = GameObject.Find("NpcsPlaya");
        contenedorBasura = GameObject.Find("BasuraPlayaCont");
        contenedorCiudad = new GameObject("contenedorCiudad");
        contenedorAnimales = new GameObject("contenedorAnimales");

        BeachTerrain();

        CityTerrainDetails();

        PlaceSeaDetails();
        PetroleoZona();
        SeaTerrain();
        PlaceBeachDetails();
        PlaceSeaTrash();
        CloudsGen();
        //ExtraTerrains();

        Invoke("GenerarGaviotasVolando", Random.Range(0,5));

        SpawnTransform();

        misionBasuraPlaya.transform.position = SearchNotBuildableZone();
        misionBasuraPlaya.GetComponent<BasuraPlaya>().width = beachWidth;
        misionBasuraPlaya.GetComponent<BasuraPlaya>().height = beachHeight;
        misionBasuraPlaya.GetComponent<BasuraPlaya>().center_x = 0;
        misionBasuraPlaya.GetComponent<BasuraPlaya>().center_y = 0;
        misionBasuraPlaya.SetActive(true);

        misionTriviaPlaya.transform.position = SearchNotBuildableZone();
        misionTriviaPlaya.SetActive(true);
    }

    void BeachTerrain()
    {
        
        centrosArena = new Vector2[Random.Range(1,3)];

        for (int i = 0; i < centrosArena.Length; i++)
        {
            centrosArena[i] = new Vector2(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        }
            
        GameObject planoTierra = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Terrain");
        var temp = Instantiate(planoTierra, new Vector3(0,-77.6f,0), Quaternion.identity);
        temp.GetComponent<TG>().width = beachWidth;
        temp.GetComponent<TG>().height = beachHeight;
        temp.SetActive(true);
    }

    void SeaTerrain()
    {
        GameObject planoAgua = Resources.Load<GameObject>("ProceduralPlaya/Mar/PlanoAguaPetroleo");
        var temp = Instantiate(planoAgua, new Vector3(beachWidth/2,0,beachHeight/2), Quaternion.identity);
        Renderer rend = temp.GetComponent<Renderer> ();
        rend.material.SetFloat("_Perx", -1 * (petroleo_pos.x - temp.transform.position.x) / 1200f);
        rend.material.SetFloat("_Pery", -1 * (petroleo_pos.z - temp.transform.position.z) / 1200f);
    }

    private Vector3 ray_debug = new Vector3(0,0,0);
    private Vector3 petroleo_pos = new Vector3(0,0,0);
    void PetroleoZona()
    {

        Vector3 posPetroleo = SearchPosDistBeach2(64);
        petroleo_pos = posPetroleo;

        ray_debug = posPetroleo;

        var temp = Instantiate(Resources.Load<GameObject>("ProceduralPlaya/Prefabs/ArenaPetroleo"), 
                                new Vector3(posPetroleo.x -64f,-77.6f,posPetroleo.z - 64f), 
                                Quaternion.identity);

        temp.GetComponent<TG>().width = 128;
        temp.GetComponent<TG>().height = 128;
        temp.SetActive(true);

        bool HP_posicionado = false;
        bool MP_posicionado = false;

        while(!HP_posicionado)
        {
            Ray ray = new Ray( new Vector3( Random.Range(posPetroleo.x - 64f, posPetroleo.x + 64f), 
                                        100f, 
                                        Random.Range(posPetroleo.z - 64f,  posPetroleo.z + 64f)), Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "ArenaPetroleo(Clone)" && hit.point.y > 2.5f)
            {
                GameObject hombree = GameObject.Find("Prefab_HP1");
                hombree.transform.position = hit.point + new Vector3(0,-1,0) ;
                hombree.transform.rotation = Quaternion.Euler(0,Random.Range(0,360),0);
                HP_posicionado = true;
            }
        }

        while(!MP_posicionado)
        {
            Ray ray = new Ray( new Vector3( Random.Range(posPetroleo.x - 64f, posPetroleo.x + 64f), 
                                        100f, 
                                        Random.Range(posPetroleo.z - 64f,  posPetroleo.z + 64f)), Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "ArenaPetroleo(Clone)" && hit.point.y > 2.5f)
            {
                GameObject mujerr = GameObject.Find("Prefab_MP1");
                mujerr.transform.position = hit.point + new Vector3(0,-1,0) ;
                mujerr.transform.rotation = Quaternion.Euler(0,Random.Range(0,360),0);
                MP_posicionado = true;
            }
        }


        Object[] anims_petro = Resources.LoadAll("ProceduralPlaya/Prefabs/AnimalesPetroleo");

        int conttt = 0;

        while(conttt < 30)
        {
            Ray ray = new Ray( new Vector3( Random.Range(posPetroleo.x - 64f, posPetroleo.x + 64f), 
                                        100f, 
                                        Random.Range(posPetroleo.z - 64f,  posPetroleo.z + 64f)), Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "ArenaPetroleo(Clone)" && hit.point.y > 2.5f)
            {
                Instantiate(anims_petro[Random.Range(0,anims_petro.Length)],
                                            hit.point , Quaternion.Euler(0,Random.Range(0,360),0));
                conttt ++;

            }

        }

        
    }

    private GameObject testtttt;

    private Vector3 pos_rio = new Vector3(0,0,0);
    private Quaternion rot_rio = Quaternion.Euler(0, 0, 0);
    private int orientation_city = 0;
    void CityTerrainDetails()
    {
        posicionOptima = SearchPosDistBeach(cellsSize * cityWidth/2);
        GameObject cuboBase = Instantiate(Resources.Load<GameObject>("ProceduralPlaya/Prefabs/CuboBase"),
                                        posicionOptima + new Vector3(0,-1,0) * posicionOptima.y + new Vector3(0,-44f,0), Quaternion.identity);
        cuboBase.transform.localScale = new Vector3(cityWidth * cellsSize, 100,cityHeight * cellsSize); 
        
        //PlaceComplementaryTerrain();
        grid = new Grid(cityWidth, cityHeight, cellsSize, posicionOptima.x, posicionOptima.z);
        //PlaceQuestSquare();
        Instantiate(Resources.Load<GameObject>("ProceduralPlaya/Mar/rioPrefab"),
                    pos_rio + new Vector3(0, -102f, 0),
                    rot_rio);

                
        int xx = 0, yy = 0;

        if (orientation_city == 0)
        {
            Debug.Log("OR0");
            xx = cityWidth - 5;
            yy = Random.Range(3,cityHeight - 4);
            Instantiate(CUBOMISION,
                        grid.GetCellCenter(xx, yy) + new Vector3(0,6,0),
                        Quaternion.Euler(0,90,0));
        } else if (orientation_city == 1)
        {
            Debug.Log("OR1");
            xx = 4;
            yy = Random.Range(3,cityHeight - 4);
            Instantiate(CUBOMISION,
                        grid.GetCellCenter(xx, yy) + new Vector3(0,6,0),
                        Quaternion.Euler(0,270,0));
        } else if (orientation_city == 2)
        {
            Debug.Log("OR2");
            xx = Random.Range(3,cityWidth-4);
            yy = cityHeight - 5;
            Instantiate(CUBOMISION,
                        grid.GetCellCenter(xx, yy) + new Vector3(0,6,0),
                        Quaternion.identity);
        } else if (orientation_city == 3)
        {
            Debug.Log("OR3");
            xx = Random.Range(3,cityWidth-4);
            yy = 4;
            Instantiate(CUBOMISION,
                        grid.GetCellCenter(xx, yy) + new Vector3(0,6,0),
                        Quaternion.Euler(0,180,0));
        }

        grid.SetCellDontBuild(xx, yy);
        grid.SetCellDontBuild(xx + 1, yy);
        grid.SetCellDontBuild(xx - 1, yy);
        grid.SetCellDontBuild(xx + 2, yy);
        grid.SetCellDontBuild(xx - 2, yy);
        grid.SetCellDontBuild(xx, yy + 1);
        grid.SetCellDontBuild(xx + 1, yy + 1);
        grid.SetCellDontBuild(xx - 1, yy + 1);
        grid.SetCellDontBuild(xx + 2, yy + 1);
        grid.SetCellDontBuild(xx - 2, yy + 1);
        grid.SetCellDontBuild(xx, yy - 1);
        grid.SetCellDontBuild(xx + 1, yy - 1);
        grid.SetCellDontBuild(xx - 1, yy - 1);
        grid.SetCellDontBuild(xx + 2, yy - 1);
        grid.SetCellDontBuild(xx - 2, yy - 1);
        grid.SetCellDontBuild(xx, yy - 2);
        grid.SetCellDontBuild(xx + 1, yy - 2);
        grid.SetCellDontBuild(xx - 1, yy - 2);
        grid.SetCellDontBuild(xx + 2, yy - 2);
        grid.SetCellDontBuild(xx - 2, yy - 2);
        grid.SetCellDontBuild(xx, yy + 2);
        grid.SetCellDontBuild(xx + 1, yy + 2);
        grid.SetCellDontBuild(xx - 1, yy + 2);
        grid.SetCellDontBuild(xx + 2, yy + 2);
        grid.SetCellDontBuild(xx - 2, yy + 2);

        AddDetailsUp();


        //ray_debug = pos_rio;
    }

    void PlaceQuestSquare()
    {
        int x = Random.Range(0,cityWidth);
        int y = Random.Range(0,cityHeight);
        grid.SetCellDontBuild(x,y);
        grid.SetCellDontBuild(x+1,y);
        grid.SetCellDontBuild(x-1,y);
        grid.SetCellDontBuild(x,y+1);
        grid.SetCellDontBuild(x-1,y+1);
        grid.SetCellDontBuild(x+1,y+1);
        grid.SetCellDontBuild(x,y-1);
        grid.SetCellDontBuild(x-1,y-1);
        grid.SetCellDontBuild(x+1,y-1);
        Instantiate(CUBOMISION, grid.GetCellCenter(x,y), Quaternion.identity);
    }

    void ExtraTerrains()
    {
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

            //temp = Instantiate(planoTierra, new Vector3(posx, -77.6f, posy), Quaternion.identity);
            //temp.transform.localScale = new Vector3(Random.Range(2,4),1,Random.Range(2,4));
            //1 ShapeTerrain(temp, 64f * i);

        }
    }

    void CloudsGen()
    {
        nubes = Resources.LoadAll("ProceduralPlaya/Prefabs/Nubes");

        for (int i = 0; i < cantidadnubes; i++)
        {
            nubesLista.Add((GameObject)Instantiate(nubes[Random.Range(0, nubes.Length)], new Vector3(Random.Range(-400,400), Random.Range(90,120), Random.Range(-400,400)), Quaternion.identity));
            nubesActuales ++;
        }  
    }

    void SpawnTransform()
    {
        GameObject pl = GameObject.Find("Player");
        if (pl != null)
        {
            if(PlayerPrefs.HasKey("playaProceduralVolver"))
            {
                if(PlayerPrefs.GetInt("playaProceduralVolver") == 0)
                    pl.transform.position = SearchNotBuildableZone(); // spawn pos.
                else
                {
                    pl.transform.position = new Vector3( PlayerPrefs.GetFloat("playaProceduralPosx"),
                                                         PlayerPrefs.GetFloat("playaProceduralPosy"),
                                                         PlayerPrefs.GetFloat("playaProceduralPosz"));
                    PlayerPrefs.SetInt("playaProceduralVolver", 0);
                }
            } else
            pl.transform.position = SearchNotBuildableZone(); // spawn pos.
        }
    }

    Vector3 SearchNotBuildableZone()
    {   
        Vector3 pos = new Vector3(0,0,0);
        bool pos_encontrada = false;
        int trys = 0;
    
            while(!pos_encontrada)
            {
                int x_r = Random.Range(1,cityWidth - 2);
                int y_r = Random.Range(1,cityHeight - 2);

                if(!grid.GetCellDontBuild(x_r, y_r))
                {
                    pos = grid.GetCellCenter(x_r, y_r) + new Vector3(0, 8f, 0);
                    grid.SetCellDontBuild(x_r,y_r);
                    pos_encontrada = true;
                }

                if (trys == 1000) 
                {
                    pos = new Vector3(0, 8f, 0);
                    break;
                }
                trys ++;
            }
        return pos;
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

        /*
        Vector3 aaaa = new Vector3(300f,100f,300f);
        Ray ray = new Ray( aaaa, -1 * transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit) )
                {
                    Debug.Log(hit.transform.name);
                }
        */
        Debug.DrawRay(ray_debug, -1*transform.up * 1000, Color.magenta);
        //Debug.DrawRay(aaaa, -1*transform.up * 1000, Color.magenta);


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

    
    Vector3 SearchPosDistBeach2(float dist_mult = 1, int trys = 1000)
    {
        bool posicionarNuevo = false;
        int ac_try = 0;
        Vector3 pos_retn = new Vector3(0,0,0);

        string name_playa = "Terrain(Clone)";
        float height_hit = 0f;

        while(!posicionarNuevo)
        {
            Ray ray = new Ray(new Vector3(  Random.Range(beach_pos_x, beach_pos_x+beachWidth),
                                            100,
                                            Random.Range(beach_pos_y, beach_pos_y+beachHeight)),
                                -1 * transform.up);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == name_playa && hit.point.y > height_hit)
            {
                Ray ray2 = new Ray(ray.origin + new Vector3(1,0,0) * dist_mult, -1 * transform.up);
                if (Physics.Raycast(ray2, out RaycastHit hit2) && hit2.transform.name == name_playa && hit2.point.y < height_hit)
                {
                    pos_retn = ray2.origin;
                    posicionarNuevo = true;
                    Debug.Log("h2");
                    pos_rio = pos_retn + new Vector3(cityWidth*cellsSize, 0, 0);
                    break;
                }

                Ray ray3 = new Ray(ray.origin - new Vector3(1,0,0) * dist_mult, -1 * transform.up);
                if (Physics.Raycast(ray3, out RaycastHit hit3) && hit3.transform.name == name_playa && hit3.point.y < height_hit)
                {
                    pos_retn = ray3.origin;
                    posicionarNuevo = true;
                    Debug.Log("h3");
                    pos_rio = pos_retn + new Vector3(- cityWidth*cellsSize, 0, 0);
                    break;
                }

                Ray ray4 = new Ray(ray.origin + new Vector3(0,0,1) * dist_mult, -1 * transform.up);
                if (Physics.Raycast(ray4, out RaycastHit hit4) && hit4.transform.name == name_playa && hit4.point.y < height_hit)
                {
                    pos_retn = ray4.origin;
                    posicionarNuevo = true;
                    Debug.Log("h4");
                    pos_rio = pos_retn + new Vector3(0, 0, cityWidth*cellsSize);
                    break;
                }

                Ray ray5 = new Ray(ray.origin - new Vector3(0,0,1) * dist_mult, -1 * transform.up);
                if (Physics.Raycast(ray5, out RaycastHit hit5) && hit5.transform.name == name_playa && hit5.point.y < height_hit)
                {
                    pos_retn = ray5.origin;
                    posicionarNuevo = true;
                    Debug.Log("h5");
                    pos_rio = pos_retn + new Vector3(0, 0, -cityWidth*cellsSize);
                    break;
                }

            }

            ac_try ++;

        }

        return pos_retn;
    }



    Vector3 SearchPosDistBeach(float dist_mult = 1, int trys = 1000)
    {
        bool posicionarNuevo = false;
        int ac_try = 0;
        Vector3 pos_retn = new Vector3(0,0,0);

        string name_playa = "Terrain(Clone)";
        float height_hit = 0f;

        while(!posicionarNuevo)
        {
            Ray ray = new Ray(new Vector3(  Random.Range(beach_pos_x, beach_pos_x+beachWidth),
                                            100,
                                            Random.Range(beach_pos_y, beach_pos_y+beachHeight)),
                                -1 * transform.up);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == name_playa && hit.point.y > height_hit)
            {
                Ray ray2 = new Ray(ray.origin + new Vector3(1,0,0) * dist_mult, -1 * transform.up);
                if (Physics.Raycast(ray2, out RaycastHit hit2) && hit2.transform.name == name_playa && hit2.point.y < height_hit)
                {
                    pos_retn = ray2.origin;
                    posicionarNuevo = true;
                    Debug.Log("h2");
                    pos_rio = pos_retn + new Vector3(cityWidth*cellsSize, 0, 0);
                    rot_rio = Quaternion.Euler(0, 270, 0);
                    orientation_city = 0;
                    break;
                }

                Ray ray3 = new Ray(ray.origin - new Vector3(1,0,0) * dist_mult, -1 * transform.up);
                if (Physics.Raycast(ray3, out RaycastHit hit3) && hit3.transform.name == name_playa && hit3.point.y < height_hit)
                {
                    pos_retn = ray3.origin;
                    posicionarNuevo = true;
                    Debug.Log("h3");
                    pos_rio = pos_retn + new Vector3(-cityWidth*cellsSize, 0, 0);
                    rot_rio = Quaternion.Euler(0, 90, 0);
                    orientation_city = 1;
                    break;
                }

                Ray ray4 = new Ray(ray.origin + new Vector3(0,0,1) * dist_mult, -1 * transform.up);
                if (Physics.Raycast(ray4, out RaycastHit hit4) && hit4.transform.name == name_playa && hit4.point.y < height_hit)
                {
                    pos_retn = ray4.origin;
                    posicionarNuevo = true;
                    Debug.Log("h4");
                    pos_rio = pos_retn + new Vector3(0, 0, cityWidth*cellsSize);
                    rot_rio = Quaternion.Euler(0, 180, 0);
                    orientation_city = 2;
                    break;
                }

                Ray ray5 = new Ray(ray.origin - new Vector3(0,0,1) * dist_mult, -1 * transform.up);
                if (Physics.Raycast(ray5, out RaycastHit hit5) && hit5.transform.name == name_playa && hit5.point.y < height_hit)
                {
                    pos_retn = ray5.origin;
                    posicionarNuevo = true;
                    Debug.Log("h5");
                    pos_rio = pos_retn + new Vector3(0, 0, -cityWidth*cellsSize);
                    rot_rio = Quaternion.Euler(0, 0, 0);
                    orientation_city = 3;
                    break;
                }

            }

            ac_try ++;

        }

        return pos_retn;
    }



    void PlaceComplementaryTerrain()
    {
        bool posicionarNuevo = false;
        int trys = 0;
        while(!posicionarNuevo)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Terrain(Clone)" && hit.point.y > 0)
            {
                Ray ray2 = new Ray(ray.origin + new Vector3(1,0,0) * cellsSize * cityWidth/2, ray.direction);

                if (Physics.Raycast(ray2, out RaycastHit hit2) && hit2.transform.name == "Terrain(Clone)" && hit2.point.y < 0)
                {
                    posicionOptima = ray2.origin;
                    posicionarNuevo = true;
                    break;
                }

                Ray ray3 = new Ray(ray.origin - new Vector3(1,0,0) * cellsSize * cityWidth/2, ray.direction);
                if (Physics.Raycast(ray3, out RaycastHit hit3) && hit3.transform.name == "Terrain(Clone)" && hit3.point.y < 0)
                {
                    posicionOptima = ray3.origin;
                    posicionarNuevo = true;
                    break;
                }

                Ray ray4 = new Ray(ray.origin + new Vector3(0,0,1) * cellsSize * cityWidth/2, ray.direction);
                if (Physics.Raycast(ray4, out RaycastHit hit4) && hit4.transform.name == "Terrain(Clone)" && hit4.point.y < 0)
                {
                    posicionOptima = ray4.origin;
                    posicionarNuevo = true;
                    break;
                }

                Ray ray5 = new Ray(ray.origin - new Vector3(0,0,1) * cellsSize * cityWidth/2, ray.direction);
                if (Physics.Raycast(ray5, out RaycastHit hit5) && hit5.transform.name == "Terrain(Clone)" && hit5.point.y < 0)
                {
                    posicionOptima = ray5.origin;
                    posicionarNuevo = true;
                    break;
                }

                

            }

            if (trys == 1000) 
            {
                posicionOptima = new Vector3(0,0,0);
                break;
            }
                trys ++;

        }

        GameObject cuboBase = Instantiate(Resources.Load<GameObject>("ProceduralPlaya/Prefabs/CuboBase"));
        cuboBase.transform.position = posicionOptima + new Vector3(0,-1,0) * posicionOptima.y + new Vector3(0,-44,0);
        cuboBase.transform.localScale = new Vector3(cityWidth * cellsSize, 100,cityHeight * cellsSize); 
    }

    float probEscalera = 0.2f;

    int n_escalera = 0;
    void AddDetailsUp()
    {
        GameObject reja1 = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Reja1");
        GameObject reja2 = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Reja2");
        GameObject escaleras = Resources.Load<GameObject>("ProceduralPlaya/Prefabs/Escaleras");

        for (int i = 0; i < cityWidth; i++)
        {
            for(int j = 0; j < cityHeight; j++)
            {

                if(i == 0 && j == 0) 
                {
                    Instantiate(reja2, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,90,0));
                    //grid.SetCellDontBuild(i,j);
                }
                else if (i == 0 && j == cityHeight - 1)
                {
                    Instantiate(reja2, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,180,0));
                    //grid.SetCellDontBuild(i,j);
                }
                else if (i == cityWidth - 1 && j == 0)
                {
                    Instantiate(reja2, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,0,0));
                    //grid.SetCellDontBuild(i,j);
                }
                else if (i == cityWidth - 1 && j == cityHeight - 1)
                {
                    Instantiate(reja2, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,270,0));
                    //grid.SetCellDontBuild(i,j);
                }
                else if( (i == 0 || i == cityWidth - 1) || (j == 0 || j == cityHeight - 1))
                {
                    Vector3 centro1;
                    Vector3 centro2;
                    if (j == 0)
                    {
                        centro1 = grid.GetCellCenter(i,j);
                        centro2 = grid.GetCellCenter(i,j+1);
                        Vector3 diferencia = centro1 - centro2;

                        Ray ray = new Ray( centro1 + (0.7f * diferencia) + new Vector3(0,100f,0), -1 * transform.up);

                        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Terrain(Clone)" && hit.point.y > 2f)
                        {
                            if (Random.Range(0f,1f) < probEscalera)
                            {
                                Instantiate(escaleras, new Vector3((centro1+diferencia).x, 4.27f, (centro1+diferencia).z + 2.1f), Quaternion.Euler(0,270,0));
                                vectorProhibido.Add(new Vector2(i,j));
                            } else{
                                Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,270,0));
                            }
                            n_escalera++;
                            // Debug.Log("j0 No hay arena enfrente.");
                        } else {
                            Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,270,0));
                        }

                    }
                    else if (j == cityHeight - 1)
                    {
                        centro1 = grid.GetCellCenter(i,j);
                        centro2 = grid.GetCellCenter(i,j-1);
                        Vector3 diferencia = centro1 - centro2;

                        Ray ray = new Ray( centro1 + (2*diferencia) + new Vector3(0,100f,0), -1 * transform.up);

                        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Terrain(Clone)" && hit.point.y > 0)
                        {
                            if (Random.Range(0f,1f) < probEscalera)
                            {
                                Instantiate(escaleras, new Vector3((centro1+diferencia).x, 4.27f, (centro1+diferencia).z - 2.1f), Quaternion.Euler(0,90,0));
                                vectorProhibido.Add(new Vector2(i,j));
                            } else{
                                Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,90,0));
                            }
                            // Debug.Log("ja No hay arena enfrente.");
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

                        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Terrain(Clone)" && hit.point.y > 0)
                        {
                            if (Random.Range(0f,1f) < probEscalera)
                            {
                                Instantiate(escaleras, new Vector3((centro1+diferencia).x + 2.1f, 4.27f, (centro1+diferencia).z), Quaternion.Euler(0,0,0));
                                vectorProhibido.Add(new Vector2(i,j));
                            } else{
                                Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,0,0));
                            }
                            // Debug.Log("i0 No hay arena enfrente.");
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

                        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Terrain(Clone)" && hit.point.y > 0)
                        {
                            if (Random.Range(0f,1f) < probEscalera)
                            {
                                Instantiate(escaleras, new Vector3((centro1+diferencia).x - 2.1f, 4.27f, (centro1+diferencia).z), Quaternion.Euler(0,180,0));
                                vectorProhibido.Add(new Vector2(i,j));
                            } else{
                                Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,180,0));
                            }
                            // Debug.Log("ia No hay arena enfrente.");
                        } else {
                            Instantiate(reja1, grid.GetCellCenter(i, j) + new Vector3 (0,6.2f,0), Quaternion.Euler(0,180,0));
                        }
                    } 

                    //grid.SetCellDontBuild(i,j);
                        
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


        Vector2 centro_xyP = new Vector2(Random.Range(1 + (int) Mathf.Ceil((largoo)/ 2),cityWidth - 2 - (int) Mathf.Ceil((largoo)/ 2)), 
                                        Random.Range(1 + (int) Mathf.Ceil((anchoo)/ 2), cityHeight - 2 - (int) Mathf.Ceil((anchoo)/ 2))); 

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
                        if(centro_xyP.x + a < cityWidth - 2)
                            grid.SetCellDontBuild((int)centro_xyP.x + a, (int)centro_xyP.y - j);
                        if(centro_xyP.x - a > 1)
                            grid.SetCellDontBuild((int)centro_xyP.x - a, (int)centro_xyP.y - j);
                    }
                    if(centro_xyP.y + j < cityHeight -2)
                    {
                        if(centro_xyP.x + a < cityWidth - 2)
                            grid.SetCellDontBuild((int)centro_xyP.x + a, (int)centro_xyP.y + j);
                        if(centro_xyP.x - a > 1)
                            grid.SetCellDontBuild((int)centro_xyP.x - a, (int)centro_xyP.y + j);
                    }
                }
                if(centro_xyP.y - j > 1)
                    grid.SetCellDontBuild((int)centro_xyP.x , (int)centro_xyP.y - j);
                if(centro_xyP.y + j < cityHeight -2)
                    grid.SetCellDontBuild((int)centro_xyP.x , (int)centro_xyP.y + j);
            }

            for (int a = 1; a < anchoo + 1; a++)
            {
                if(centro_xyP.x + a < cityWidth - 2)
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
            Vector2 centro_xy = new Vector2(Random.Range(1,cityWidth - 2), Random.Range(1, cityHeight - 2)); 
            int largo = Random.Range(2,4);
            int ancho = Random.Range(2,4);

            if (grid.GetCellDontBuild( (int)centro_xy.x , (int)centro_xy.y ) != true)
            {
            Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x , (int)centro_xy.y ) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
            PonerPalmera(prob_palmera, (int)centro_xy.x , (int)centro_xy.y, altura_palmera);
            grid.SetCellDontBuild((int)centro_xy.x , (int)centro_xy.y);
            }


            for (int j = 1; j < largo + 1; j++)
            {

                for (int a = 1; a < ancho + 1; a++)
                {
                    if(centro_xy.y - j > 1)
                    {
                        if(centro_xy.x + a < cityWidth - 2 && grid.GetCellDontBuild( (int)centro_xy.x + a, (int)centro_xy.y - j ) != true)
                        {
                            Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x + a, (int)centro_xy.y - j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                            PonerPalmera(prob_palmera, (int)centro_xy.x + a , (int)centro_xy.y - j, altura_palmera);
                            grid.SetCellDontBuild((int)centro_xy.x + a, (int)centro_xy.y - j);
                        }
                        if(centro_xy.x - a > 1  && grid.GetCellDontBuild( (int)centro_xy.x - a, (int)centro_xy.y - j ) != true)
                        {
                            Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x - a, (int)centro_xy.y - j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                            PonerPalmera(prob_palmera, (int)centro_xy.x - a , (int)centro_xy.y - j, altura_palmera);
                            grid.SetCellDontBuild((int)centro_xy.x - a, (int)centro_xy.y - j);
                        }
                    }
                    if(centro_xy.y + j < cityHeight -2)
                    {
                        if(centro_xy.x + a < cityWidth - 2  && grid.GetCellDontBuild( (int)centro_xy.x + a, (int)centro_xy.y + j ) != true)
                        {
                            Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x + a, (int)centro_xy.y + j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                            PonerPalmera(prob_palmera, (int)centro_xy.x + a , (int)centro_xy.y + j, altura_palmera);
                            grid.SetCellDontBuild((int)centro_xy.x + a, (int)centro_xy.y + j);
                        }
                        if(centro_xy.x - a > 1  && grid.GetCellDontBuild( (int)centro_xy.x - a, (int)centro_xy.y + j ) != true)
                        {
                            Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x - a, (int)centro_xy.y + j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                            PonerPalmera(prob_palmera, (int)centro_xy.x - a , (int)centro_xy.y + j, altura_palmera);
                            grid.SetCellDontBuild((int)centro_xy.x - a, (int)centro_xy.y + j);
                        }
                    }
                }
                if(centro_xy.y - j > 1  && grid.GetCellDontBuild( (int)centro_xy.x, (int)centro_xy.y - j ) != true)
                {   
                Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x, (int)centro_xy.y - j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                PonerPalmera(prob_palmera, (int)centro_xy.x , (int)centro_xy.y - j, altura_palmera);
                grid.SetCellDontBuild((int)centro_xy.x , (int)centro_xy.y - j);
                }
                if(centro_xy.y + j < cityHeight - 2  && grid.GetCellDontBuild( (int)centro_xy.x, (int)centro_xy.y + j ) != true)
                {
                Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x, (int)centro_xy.y + j) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                PonerPalmera(prob_palmera, (int)centro_xy.x , (int)centro_xy.y + j, altura_palmera);
                grid.SetCellDontBuild((int)centro_xy.x , (int)centro_xy.y + j);
                }
            }

            for (int a = 1; a < ancho + 1; a++)
            {
                if(centro_xy.x + a < cityWidth - 2  && grid.GetCellDontBuild( (int)centro_xy.x + a, (int)centro_xy.y ) != true)
                {
                    Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x + a, (int)centro_xy.y) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                    PonerPalmera(prob_palmera, (int)centro_xy.x + a, (int)centro_xy.y, altura_palmera);
                    grid.SetCellDontBuild((int)centro_xy.x + a , (int)centro_xy.y);
                }
                if(centro_xy.x - a > 1  && grid.GetCellDontBuild( (int)centro_xy.x - a, (int)centro_xy.y ) != true)
                {
                    Instantiate(detallesPlaza[Random.Range(0,detallesPlaza.Length)], grid.GetCellCenter((int)centro_xy.x - a, (int)centro_xy.y) + new Vector3(0,altura,0), Quaternion.Euler(0,0,0));
                    PonerPalmera(prob_palmera, (int)centro_xy.x - a, (int)centro_xy.y, altura_palmera);
                    grid.SetCellDontBuild((int)centro_xy.x - a , (int)centro_xy.y);
                }       
            }

        }





        float prob_silla = 0.2f;
        float prob_basura = 0.1f;
        UnityEngine.Object[] sillas = Resources.LoadAll("ProceduralPlaya/Prefabs/Detalles/Borde");
        UnityEngine.Object[] basureros = Resources.LoadAll("ProceduralPlaya/Prefabs/Detalles/BasurerosBorde");

        for (int i = 0; i < cityWidth; i++)
        {
            for(int j = 0; j < cityHeight ; j++)
            {

                if(i == 0 && j == 0) 
                    continue;
                else if (i == 0 && j == cityHeight - 1)
                    continue;
                else if (i == cityWidth - 1 && j == 0)
                    continue;
                else if (i == cityWidth - 1 && j == cityHeight - 1)
                    continue;
                else if( (i == 0 || i == cityWidth - 1) || (j == 0 || j == cityHeight - 1))
                {
                    if (j == 0)
                    {
                        if (Random.Range(0f,1f) < prob_silla && !vectorProhibido.Contains(new Vector2(i,j)))
                        {
                            variarhumanos(
                                Instantiate(sillas[Random.Range(0, sillas.Length)], grid.GetCellCenter(i,j) + new Vector3(0,6,0), Quaternion.Euler(0,180,0), NpcsPlaya.transform)
                            as GameObject);
                        } else if (Random.Range(0f,1f) < prob_basura && !grid.GetCellDontBuild(i,j))
                            Instantiate(basureros[Random.Range(0, basureros.Length)], grid.GetCellCenter(i,j) + new Vector3(0,7,0), Quaternion.Euler(0,270,0));

                    }
                    else if (j == cityHeight - 1)
                    {
                       
                        if (Random.Range(0f,1f) < prob_silla && !vectorProhibido.Contains(new Vector2(i,j)))
                        {
                            variarhumanos(
                            Instantiate(sillas[Random.Range(0, sillas.Length)], grid.GetCellCenter(i,j) + new Vector3(0,6,0), Quaternion.Euler(0,0,0), NpcsPlaya.transform)
                            as GameObject);
                        } else if (Random.Range(0f,1f) < prob_basura && !grid.GetCellDontBuild(i,j))
                            Instantiate(basureros[Random.Range(0, basureros.Length)], grid.GetCellCenter(i,j) + new Vector3(0,7,0), Quaternion.Euler(0,90,0));
                        
                    }
                    else if (i == 0)
                    {
                        
                        if (Random.Range(0f,1f) < prob_silla && !vectorProhibido.Contains(new Vector2(i,j)))
                        {
                            variarhumanos(
                            Instantiate(sillas[Random.Range(0, sillas.Length)], grid.GetCellCenter(i,j) + new Vector3(0,6,0), Quaternion.Euler(0,270,0), NpcsPlaya.transform)
                            as GameObject);
                        } else if (Random.Range(0f,1f) < prob_basura && !grid.GetCellDontBuild(i,j))
                            Instantiate(basureros[Random.Range(0, basureros.Length)], grid.GetCellCenter(i,j) + new Vector3(0,7,0), Quaternion.Euler(0,0,0));
                       
                    }
                        
                    else
                    {
                        if (Random.Range(0f,1f) < prob_silla && !vectorProhibido.Contains(new Vector2(i,j)))
                        {
                            variarhumanos(
                            Instantiate(sillas[Random.Range(0, sillas.Length)], grid.GetCellCenter(i,j) + new Vector3(0,6,0), Quaternion.Euler(0,90,0), NpcsPlaya.transform)
                            as GameObject);
                        } else if (Random.Range(0f,1f) < prob_basura && !grid.GetCellDontBuild(i,j))
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
            Vector3 centroAAA = grid.GetCellCenter(x , y);
            Ray ray = new Ray(new Vector3(centroAAA.x,100,centroAAA.z), -1 * transform.up);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Instantiate(palmera[Random.Range(0,palmera.Length)], grid.GetCellCenter(x , y) + new Vector3(0,hit.point.y-1f,0), Quaternion.Euler(0,Random.Range(0,360),0));
                grid.SetCellDontBuild(x,y);

            }
                

        }
    }


    void PlaceSeaDetails()
    {
        detallesMar = Resources.LoadAll("ProceduralPlaya/Prefabs/DetallesMar");

        Debug.Log(detallesMar.Length);
        
        for (int i = 0; i < cantidadDetallesMar; i++)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit*2.5f, right_limit*2.5f),100,Random.Range(down_limit*2.5f,up_limit*2.5f)), -1 * transform.up);

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
                if (hit.transform.name == "Terrain(Clone)" && hit.point.y < 0.5f)
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
        int trys = 0;

        float hitpoint_limit = 2f;

        while (cont < cantidadBasura)
        {
            Ray ray = new Ray(new Vector3(Random.Range(beach_pos_x, beach_pos_x+beachWidth), 100, Random.Range(beach_pos_y, beach_pos_y+beachHeight)), -1 * transform.up);

            // Perform a raycast using the ray.
            if  (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Terrain(Clone)" && hit.point.y > hitpoint_limit)
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

            if (trys == 1000)
            {
                break;
            }
            trys ++;
        }


        UnityEngine.Object[] detallesHumanos = Resources.LoadAll("ProceduralPlaya/Prefabs/DetallesPlaya/NPC");
        int contHum = 0;
        int trys2 = 0;

        while (contHum < cantidadPersonas)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if  (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Terrain(Clone)" && hit.point.y > hitpoint_limit)
            {
                Object insta = Instantiate(detallesHumanos[Random.Range(0,detallesHumanos.Length)], hit.point, Quaternion.Euler(0, Random.Range(0,360), 0), NpcsPlaya.transform);

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

            if (trys2 == 1000)
            {
                break;
            }
            trys2++;
        }

        UnityEngine.Object[] animales = Resources.LoadAll("ProceduralPlaya/Prefabs/Animales");

        int cont2 = 0;
        int trys3 = 0;

        while (cont2 < 100)
        {
            Ray ray = new Ray(new Vector3(Random.Range(left_limit, right_limit),100,Random.Range(down_limit,up_limit)), -1 * transform.up);

            // Perform a raycast using the ray.
            if  (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.name == "Terrain(Clone)" && hit.point.y > hitpoint_limit)
            {
                Instantiate(animales[Random.Range(0,animales.Length)], hit.point, Quaternion.Euler(0, Random.Range(0,360), 0), contenedorAnimales.transform);
                cont2 ++;
            }

            if (trys3 == 1000)
            {
                break;
            }
            trys3 ++;
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
