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
    public bool inputTrue = false;
    Vector3 posCamara;

    [SerializeField] private GameObject[] animales;
    [SerializeField] private GameObject[] dunas;
    private List<GameObject> objetosMision;

    public GameObject radarPrefab;


    public GameObject questTracker;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questText;


    int asignador = 0;
    int animalesAyudados = 0;


    private float tiempoLimite;

    private Vector3 playerPosition;
    private Vector3 npcPosition;

    private GameObject puntoEntorno;




    void Start()
    {
        jugador = GameObject.Find("Player");
        camara = GameObject.Find("Camera");
        npcPosition = transform.position;

        questTracker.SetActive(false);
    }



    void Update()
    {

        // Comienza mision
        if (!questStarted && !(missionDone) && transform.GetChild(2).GetComponent<Quest_Starter>().misionAceptada) //  
        {
            OnOffPlayer();

            CambiarMapaInicio();
            ObjetivosPantallaON();
            //RotarCamaraEntorno();

            Invoke("OnOffPlayer", 2f);
            questTracker.SetActive(true);


            /*

            Vector3 pos = semillas.transform.position - Jugador.transform.position;
            pos += new Vector3(3, 4, 4);

            rotOriginal = CamaraO.transform.rotation;

            CamaraO.transform.Translate(pos, Space.World);
            CamaraO.transform.LookAt(semillas.transform);

            Invoke("OnOffPlayer", 2f);
            questTracker.SetActive(true);

            */
        }


        // durante mision

        if (questStarted)
        {

            /*
            questText.text = "¡Planta y haz crecer 3 arboles!\n Llevas " + arbolesPlantados + "/3.";

            for (int i = 0; i < arbolesMision.transform.childCount; i++)
            {
                if (arbolesMision.transform.GetChild(i).gameObject.GetComponent<ArbolReforestar>().to_remove)
                {
                    Vector3 pos = arbolesMision.transform.GetChild(i).gameObject.transform.position;
                    pos.y = pos.y - 2;

                    var tumulto = Instantiate(tumultoPrefab, pos, Quaternion.Euler(0, 0, 0));

                    tumulto.GetComponent<TumultoReforestar>().semillaPrefab = semillaPrefab;
                    tumulto.GetComponent<TumultoReforestar>().indice = asignador;
                    tumulto.gameObject.layer = LayerMask.NameToLayer("interactable");
                    tumulto.transform.parent = tumultoContainter.transform;


                    asignador++;
                    arbolesMision.transform.GetChild(i).gameObject.GetComponent<ArbolReforestar>().to_remove = false;
                    arbolesMision.transform.GetChild(i).gameObject.SetActive(false);
                }
            
            }

            for (int i = 0; i < tumultoContainter.transform.childCount; i++)
            {
                if (tumultoContainter.transform.GetChild(i).gameObject.GetComponent<TumultoReforestar>().plantar)
                {
                    Vector3 pos = tumultoContainter.transform.GetChild(i).gameObject.transform.position;
                    pos.y += 1.5f;

                    var plantita = Instantiate(plantaPrefab, pos, Quaternion.Euler(0, 0, 0));

                    plantita.transform.parent = plantasContainer.transform;
                    plantita.gameObject.layer = LayerMask.NameToLayer("interactable");
                    plantita.GetComponent<ArbolitoReforestar>().regaderaPrefab = regaderaPrefab;
                    plantita.GetComponent<ArbolitoReforestar>().indice = tumultoContainter.transform.GetChild(i).gameObject.GetComponent<TumultoReforestar>().indice;


                    tumultoContainter.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
                    tumultoContainter.transform.GetChild(i).gameObject.GetComponent<TumultoReforestar>().plantar = false;
                }

            }

            for (int i = 0; i < plantasContainer.transform.childCount; i++)
            {
                if (plantasContainer.transform.GetChild(i).gameObject.GetComponent<ArbolitoReforestar>().to_remove)
                {
                    Vector3 pos = plantasContainer.transform.GetChild(i).gameObject.transform.position;
                    pos.y -= 0.5f;

                    Instantiate(arbolPrefab, pos, Quaternion.Euler(0, 0, 0));



                    plantasContainer.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
                    plantasContainer.transform.GetChild(i).gameObject.GetComponent<ArbolitoReforestar>().to_remove = false;
                    EliminarTumultoArbolito(plantasContainer.transform.GetChild(i).gameObject.GetComponent<ArbolitoReforestar>().indice);
                    arbolesPlantados++;
                    plantasContainer.transform.GetChild(i).gameObject.SetActive(false);
                }

            }*/


        }




        // Termina mision
        if (animalesAyudados >= 3 && missionDone)
        {
            CambiarMapaFinal();
            ObjetivosPantallaOFF();
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

        Vector3 posJugador = jugador.transform.position;

        GameObject animales = GameObject.Find("Animales");
        animales.SetActive(false);
        transform.gameObject.SetActive(false);

        //RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 2f;
        RenderSettings.fogEndDistance = 160f;
        RenderSettings.fogDensity = 0.1f;
        //RenderSettings.fogColor = Color.blue;

        GameObject aux;

        for (int i = -1200; i < 1200; i = i + 100)
        {
            for (int j = -1200; j < 1200; j = j + 100)
            {
                aux = Instantiate(dunas[Random.Range(0,4)], new Vector3(i, 10, j), Quaternion.identity);
                aux.transform.localScale = new Vector3(20, 25, 20);
                objetosMision.Add(aux);

                i = i + Random.Range(0, 25);
                j = j + Random.Range(0, 25);
            }
                
        }
    }

    private void ObjetivosPantallaON()
    {

    }

    private void ObjetivosPantallaOFF()
    {

    }

    private void CambiarMapaFinal()
    {
        OnOffPlayer();

        RenderSettings.fog = false;

        missionDone = true;
        questStarted = false;
        questTitle.text = "Mision Terminada!";
        questText.text = "El bosque vuelve a la vida!";
        Invoke("ReemplazarRestoArboles", 2f);

        this.gameObject.layer = LayerMask.NameToLayer("dialogable");

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(true);

        objetosMision.Clear();

        GameObject animales = GameObject.Find("Animales");
        animales.SetActive(true);


        //RotarCamaraArboles(orbitSpeed, 15f, 8f);
        Invoke("OnOffPlayer", 8f);
        Invoke("desactivarTexto", 3f);


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
