using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreservarFauna : MonoBehaviour
{
    [SerializeField] private bool testFinalMision = false;

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


    int pistasHayadas = 0;
    public static int animalesAyudados = 0;

    private GameObject puntoEntorno;

    private float distanciaZonas = 700;
    private float radioZonas = 100f;

    private GameObject zonaOrca;
    private GameObject zonaPengu;
    private GameObject zonaFoca;
    private GameObject animales, dunes;

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
        questTracker.SetActive(false);

        animales = GameObject.Find("Animales");
        dunes = GameObject.Find("ObjetosMision");

        
    }


    GameObject pengu;
    GameObject orca;
    GameObject foca;

    GameObject borrarPistas, borrarAnimales;

    void Update()
    {
        //Debug.Log("logrados " + animalesAyudados);

        // Comienza mision
        if (!questStarted && !(missionDone) && GetComponent<QuestStarterPolo>().misionAceptada) //  
        {
            //OnOffPlayer();
            CambiarMapaInicio();
            //RotarCamaraEntorno();

            //Invoke("OnOffPlayer", 2f);
            questTracker.SetActive(true);

            if (testFinalMision)
            {
                animalesAyudados = 2;
                pistasHayadas = 2;
            }
        }


        // durante mision
        if (questStarted && animalesAyudados < 3)
        {
            canvas.transform.GetChild(7).transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ".- Animales asistidos: " + animalesAyudados + " / 3";


            zonaPengu = GameObject.Find("zonaPengu");
            zonaOrca = GameObject.Find("zonaOrca");
            zonaFoca = GameObject.Find("zonaFoca");

            if (zonaPengu != null && !penguOn)
            {
                penguOn = true;

                borrarPistas = GameObject.Find("pluma(Clone)");

                pistasHayadas++;
                canvas.transform.GetChild(7).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ".- Pistas encontradas: " + pistasHayadas + " / 3";

                Vector3 centroPengu = zonaPengu.transform.position;
                Vector3 posPengu = new Vector3(Random.Range(centroPengu.x - 100, centroPengu.x + 100), 50, Random.Range(centroPengu.z - 100, centroPengu.z + 100));

                pengu = Instantiate(animalesMision[0], posPengu, Quaternion.identity);
                //pengu.transform.parent = ObjMision.transform;
                objetosMision.Add(pengu);
                Radar.targets.Add(pengu.transform);
                Destroy(borrarPistas, 5f);
            }

            if(penguOn && pengu != null)
            {
                 if (pengu.transform.GetComponent<InteractuarAnimales>().animalAyudado && !sumAnimalOnceP)
                {
                    sumAnimalOnceP = true;
                    animalesAyudados++;

                    borrarAnimales = GameObject.Find("pinguino(Clone)");
                    Destroy(borrarAnimales);

                    Destroy(zonaPengu);
                }

            }

            if (zonaOrca != null && !orcaOn)
            {
                orcaOn = true;

                borrarPistas = GameObject.Find("ninia(Clone)");

                pistasHayadas++;
                canvas.transform.GetChild(7).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ".- Pistas encontradas: " + pistasHayadas + " / 3";

                Vector3 centroOrca = zonaOrca.transform.position;
                Vector3 posOrca = new Vector3(Random.Range(centroOrca.x - 100, centroOrca.x + 100), 50, Random.Range(centroOrca.z - 100, centroOrca.z + 100));

                orca = Instantiate(animalesMision[1], posOrca, Quaternion.identity);
                //orca.transform.parent = ObjMision.transform;
                objetosMision.Add(orca);
                Radar.targets.Add(orca.transform);

                Destroy(borrarPistas, 5f);
            }

            if(orcaOn && orca != null)
            {
                if (orca.transform.GetComponent<InteractuarAnimales>().animalAyudado && !sumAnimalOnceO)
                {
                    sumAnimalOnceO = true;
                    animalesAyudados++;

                    borrarAnimales = GameObject.Find("orca(Clone)");
                    Destroy(borrarAnimales);

                    Destroy(zonaOrca);
                }
            }

            if (zonaFoca != null && !focaOn)
            {
                focaOn = true;

                borrarPistas = GameObject.Find("huesoPescado(Clone)");

                pistasHayadas++;
                canvas.transform.GetChild(7).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ".- Pistas encontradas: " + pistasHayadas + " / 3";

                Vector3 centroFoca = zonaFoca.transform.position;
                Vector3 posFoca = new Vector3(Random.Range(centroFoca.x - 100, centroFoca.x + 100), 50, Random.Range(centroFoca.z - 100, centroFoca.z + 100));

                foca = Instantiate(animalesMision[2], posFoca, Quaternion.identity);
                //foca.transform.parent = ObjMision.transform;
                objetosMision.Add(foca);
                Radar.targets.Add(foca.transform);
                Destroy(borrarPistas, 5f);
            }

            if(focaOn && foca != null)
            {
                if (foca.transform.GetComponent<InteractuarAnimales>().animalAyudado && !sumAnimalOnceF)
                {
                    sumAnimalOnceF = true;
                    animalesAyudados++;

                    borrarAnimales = GameObject.Find("foca(Clone)");
                    Destroy(borrarAnimales);

                    Destroy(zonaFoca);
                }
            }
            
        }

        // Termina mision
        if (animalesAyudados >= 3 && !missionDone)
        {
            missionDone = true;
            Debug.Log("Mision terminada");
            CambiarMapaFinal();
            //ObjetivosPantallaOFF();
        }

    }

    private void CambiarMapaInicio()
    {
        questStarted = true;
        //tiempoLimite = 4f;
        musicaAmbiente.enabled = false;

        Vector3 min = new Vector3(-1200 + radioZonas, 50, -1200 +radioZonas);
        Vector3 max = new Vector3(1200 - radioZonas, 50, 1200 - radioZonas);

        Vector3 posJugador = jugador.transform.position;

        
        Vector3[] posicionesAnimales = SpawnFinder.GetPoints(distanciaZonas,4,posJugador,min,max);

        Vector3 focaPos = posicionesAnimales[1];
        Vector3 orcaPos = posicionesAnimales[2];
        Vector3 penguPos = posicionesAnimales[3];
        
        
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
        musicaAmbiente.enabled = true;

        transform.GetChild(1).gameObject.SetActive(true); // excla final
        transform.GetChild(2).gameObject.SetActive(true); // mujer
        transform.GetChild(4).gameObject.SetActive(true); // excla final minimapa

        transform.GetComponent<BoxCollider>().enabled = !(transform.GetComponent<BoxCollider>().enabled);

        

        RenderSettings.fog = false;

        jugador.transform.GetChild(6).gameObject.SetActive(true); // nieve
        jugador.transform.GetChild(7).gameObject.SetActive(false); // ventisca

        jugador.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false); // radar

        missionDone = true;
        questStarted = false;
        questTitle.text = "Mision Terminada!";
        canvas.transform.GetChild(7).transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "La fauna se fortalece!";
        
        this.gameObject.layer = LayerMask.NameToLayer("dialogable");

        objetosMision.Clear();

        animales.SetActive(true);

        dunes.SetActive(false);

        foreach (GameObject dunita in objetosMision)
        {
            Destroy(dunita);
        }

        questTracker.SetActive(false);
        transform.GetComponent<PreservarFauna>().enabled = !(transform.GetComponent<PreservarFauna>().enabled); // apaga el escript de la misión 

        
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