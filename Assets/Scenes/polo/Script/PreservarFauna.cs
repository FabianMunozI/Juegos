using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PreservarFauna : MonoBehaviour
{
    
    private bool questStarted = false; // Mision en curso
    private bool missionDone = false; // Mision ya hecha

    private GameObject jugador;
    private GameObject camara;
    private GameObject canvas;
    private AudioSource musicaAmbiente;
    public bool inputTrue = false;
    Vector3 posCamara;

    [SerializeField] private GameObject[] animalesMision;
    [SerializeField] private GameObject[] pistas;
    [SerializeField] private GameObject[] dunas;
    private List<GameObject> objetosMision = new List<GameObject>();

   private GameObject ObjMision;

    private GameObject questTracker;
    private TextMeshProUGUI questTitle;
    private TextMeshProUGUI questText;


    int logrados = 0;
    int animalesAyudados = 0;


    private float tiempoLimite;

    private GameObject puntoEntorno;

    private float distanciaZonas = 700;
    private float radioZonas = 100f;

    private GameObject zonaOrca;
    private GameObject zonaPengu;
    private GameObject zonaFoca;

    private bool focaOn = false, penguOn = false , orcaOn = false;

    private bool sumAnimalOnceP = false, sumAnimalOnceO = false, sumAnimalOnceF = false;

    void Start()
    {
        jugador = GameObject.Find("Player");
        camara = GameObject.Find("Camera");
        canvas = GameObject.Find("Canvas");
        ObjMision = GameObject.Find("ObjetosMision");
        musicaAmbiente = GameObject.Find("Generador").GetComponent<AudioSource>();

        questTracker = canvas.transform.GetChild(7).gameObject; ;
        questTitle = canvas.transform.GetChild(7).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        questText = canvas.transform.GetChild(7).transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        
        questTracker.SetActive(false);
    }



    void Update()
    {

        // Comienza mision
        if (!questStarted && !(missionDone) && GetComponent<QuestStarterPolo>().misionAceptada) //  
        {
            //OnOffPlayer();

            CambiarMapaInicio();
            //RotarCamaraEntorno();

            //Invoke("OnOffPlayer", 2f);
            questTracker.SetActive(true);

            /*
            Vector3 pos = semillas.transform.position - Jugador.transform.position;
            pos += new Vector3(3, 4, 4);

            rotOriginal = CamaraO.transform.rotation;

            CamaraO.transform.Translate(pos, Space.World);
            CamaraO.transform.LookAt(semillas.transform);

            */
        }


        // durante mision


        if (questStarted && logrados <3)
        {
            Debug.Log("entré");
            zonaPengu = GameObject.Find("zonaPengu");
            zonaOrca = GameObject.Find("zonaOrca");
            zonaFoca = GameObject.Find("zonaFoca");


            print(zonaPengu != null);

            if (zonaPengu != null && !penguOn)
            {
                penguOn = true;

                if (GameObject.Find("pinguino(Clone)").transform.GetComponent<InteractuarAnimales>().animalAyudado && !sumAnimalOnceP)
                {
                    sumAnimalOnceP = true;
                    logrados++;
                }

                Debug.Log("yes");
                GameObject pengu;
                Vector3 centroPengu = zonaPengu.transform.position;
                Vector3 posPengu = new Vector3(Random.Range(centroPengu.x - 100, centroPengu.x + 100), 50, Random.Range(centroPengu.z - 100, centroPengu.z + 100));

                pengu = Instantiate(animalesMision[0], posPengu, Quaternion.identity);
                //pengu.transform.parent = ObjMision.transform;
                objetosMision.Add(pengu);
                Radar.targets.Add(pengu.transform);
            }

            if (zonaOrca != null && !orcaOn)
            {
                orcaOn = true;

                if (GameObject.Find("orca(Clone)").transform.GetComponent<InteractuarAnimales>().animalAyudado && !sumAnimalOnceO)
                {
                    sumAnimalOnceO = true;
                    logrados++;
                }

                GameObject orca;
                Vector3 centroOrca = zonaOrca.transform.position;
                Vector3 posOrca = new Vector3(Random.Range(centroOrca.x - 100, centroOrca.x + 100), 50, Random.Range(centroOrca.z - 100, centroOrca.z + 100));

                orca = Instantiate(animalesMision[1], posOrca, Quaternion.identity);
                //orca.transform.parent = ObjMision.transform;
                objetosMision.Add(orca);
                Radar.targets.Add(orca.transform);
            }

            if (zonaFoca != null && !focaOn)
            {
                focaOn = true;

                if (GameObject.Find("foca(Clone)").transform.GetComponent<InteractuarAnimales>().animalAyudado && !sumAnimalOnceF)
                {
                    sumAnimalOnceF = true;
                    logrados++;
                }

                GameObject foca;
                Vector3 centroFoca = zonaOrca.transform.position;
                Vector3 posFoca = new Vector3(Random.Range(centroFoca.x - 100, centroFoca.x + 100), 50, Random.Range(centroFoca.z - 100, centroFoca.z + 100));

                foca = Instantiate(animalesMision[2], posFoca, Quaternion.identity);
                //foca.transform.parent = ObjMision.transform;
                objetosMision.Add(foca);
                Radar.targets.Add(foca.transform);
            }


        }




        // Termina mision
        if (animalesAyudados >= 3 && missionDone)
        {
            //CambiarMapaFinal();
            //ObjetivosPantallaOFF();
        }


        //Camara final misión 
        /*
        if (avRotate && tiempoLimite > 0)
        {
            CamaraO.transform.RotateAround(centroBosque.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
            tiempoLimite -= Time.deltaTime;
        }
        */

    }

    private void CambiarMapaInicio()
    {
        questStarted = true;
        tiempoLimite = 4f;
        musicaAmbiente.enabled = false;

        Vector3 min = new Vector3(-1200 + radioZonas, 50, -1200 +radioZonas);
        Vector3 max = new Vector3(1200 - radioZonas, 50, 1200 - radioZonas);

        Vector3 posJugador = jugador.transform.position;

        
        Vector3[] posicionesAnimales = SpawnFinder.GetPoints(distanciaZonas,4,posJugador,min,max);

        Vector3 focaPos = posicionesAnimales[1];
        Vector3 orcaPos = posicionesAnimales[2];
        Vector3 penguPos = posicionesAnimales[3];
        
        GameObject animales = GameObject.Find("Animales");
        animales.SetActive(false);

        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 2f;
        RenderSettings.fogEndDistance = 110f;
        RenderSettings.fogDensity = 0.1f;

        jugador.transform.GetChild(6).gameObject.SetActive(false);
        jugador.transform.GetChild(7).gameObject.SetActive(true);
        jugador.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true); // radar


        GameObject aux;

        for (int i = -1200; i < 1200; i = i + 100)
        {
            for (int j = -1200; j < 1200; j = j + 100)
            {
                aux = Instantiate(dunas[Random.Range(0,3)], new Vector3(i, 30, j), Quaternion.identity);
                aux.transform.localScale = new Vector3(30, 45, 30);
                aux.transform.parent = ObjMision.transform;
                objetosMision.Add(aux);

                i = i + Random.Range(0, 25);
                j = j + Random.Range(0, 25);
            }
                
        }

        //Pengu
        //Orca
        //Foca

        aux = Instantiate(pistas[0], penguPos, Quaternion.identity);
        objetosMision.Add(aux);
        aux.GetComponent<SpawnFloorFinderFauna>().centroZona = penguPos;
        Radar.targets.Add(aux.transform);

        aux = Instantiate(pistas[1], orcaPos, Quaternion.identity);
        objetosMision.Add(aux);
        aux.GetComponent<SpawnFloorFinderFauna>().centroZona = orcaPos;
        Radar.targets.Add(aux.transform);

        aux = Instantiate(pistas[2], focaPos, Quaternion.identity);
        objetosMision.Add(aux);
        aux.GetComponent<SpawnFloorFinderFauna>().centroZona = focaPos;
        Radar.targets.Add(aux.transform);

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);

        transform.GetComponent<BoxCollider>().enabled = !(transform.GetComponent<BoxCollider>().enabled);
        GetComponent<QuestStarterPolo>().enabled = !(GetComponent<QuestStarterPolo>().enabled);


    }

    private void CambiarMapaFinal()
    {
        //OnOffPlayer();

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);

        transform.GetComponent<BoxCollider>().enabled = !(transform.GetComponent<BoxCollider>().enabled);

        RenderSettings.fog = false;

        jugador.transform.GetChild(6).gameObject.SetActive(true);
        jugador.transform.GetChild(7).gameObject.SetActive(false);

        jugador.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);

        missionDone = true;
        questStarted = false;
        questTitle.text = "Mision Terminada!";
        questText.text = "La fauna se fortalece!";
        
        this.gameObject.layer = LayerMask.NameToLayer("dialogable");

        objetosMision.Clear();

        GameObject animales = GameObject.Find("Animales");
        animales.SetActive(true);


        //RotarCamaraArboles(orbitSpeed, 15f, 8f);
        //Invoke("OnOffPlayer", 8f);
        Invoke("desactivarTexto", 3f);

        musicaAmbiente.enabled = true;
    }

    public void RotarCamaraEntorno(float orbitSpeed, float distancia, float tiempoLimite)
    {
/*
        Vector3 pos = centroBosque.transform.position - Jugador.transform.position;
        pos += new Vector3(distancia, distancia, distancia);

        CamaraO.transform.Translate(pos, Space.World);
        CamaraO.transform.LookAt(centroBosque.transform);

        avRotate = true;
*/

    }


    public void OnOffPlayer()
    {
        inputTrue = !inputTrue;
        jugador.GetComponent<CharacterMovement>().AnimacionOn = !inputTrue; // frezea al player mientras esta la animacion
        jugador.GetComponent<FpsCamera>().animacionOn = !inputTrue;

        if (inputTrue == false)
        { // si ya se inicio la mision y se esta terminando / devolviendo controles al player
          // hay que desactivar que la mision pueda ser tomada
          //referenciaExclamacion.SetActive(inputTrue);
          //controller.SetTrigger("End3");
          //Debug.Log("volvieron los controles");
          // referenciaExclamacion.SetActive(false);
          //this.GetComponent<MeshRenderer>().material.color=Color.green;

            camara.transform.localPosition = new Vector3(0, 0.69f, 0);
            //camara.transform.rotation = rotOriginal;


        }

    }
}


public static class SpawnFinder
{

    private static bool CompareTwo(Vector3 a, Vector3 b, float distanciaZonas)
    {
        float x = a.x - b.x;
        float z = a.z - b.z;

        if ((x * x + z * z) < (distanciaZonas * distanciaZonas))
            return false;

        return true;
    }

    public static bool CompareAll(float minDist, Vector3[] puntos)
    {
        for (int i = 0; i < puntos.Length; i++)
        {
            Vector3 pa = puntos[i];
            for (int j = i + 1; j < puntos.Length; j++)
            {
                Vector3 pb = puntos[j];
                if (!CompareTwo(pa, pb, minDist))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static Vector3[] GetPoints(float minDist, int nOP, Vector3 player, Vector3 min, Vector3 max)
    {
        Vector3[] points = new Vector3[nOP];
        points[0] = player;
        
        do 
        {
            for (int i = 1; i<nOP; i++)
            {
                points[i] = new Vector3(Random.Range(min.x, max.x), min.y, Random.Range(min.z, max.z));
            }
        } while (!CompareAll(minDist, points)) ;

        return points;
    }
}