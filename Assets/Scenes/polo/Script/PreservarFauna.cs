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
    Vector3 posCamara;

    [SerializeField] private GameObject[] animales;
    [SerializeField] private GameObject[] dunas;
    private List<GameObject> objetosMision;

    public GameObject dialogoInicio; //dialogoObjetivo

    public GameObject radarPrefab;


    public GameObject questTracker;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questText;


    int asignador = 0;
    int animalesAyudados = 0;


    private float tiempoLimite = 4f;

    private Vector3 playerPosition;
    private Vector3 npcPosition;

    private GameObject puntoEntorno;




    void Start()
    {
        jugador = GameObject.Find("Player");
        camara = GameObject.Find("Camera");

        CambiarMapaInicio();
        questTracker.SetActive(false);
    }



    void Update()
    {

        // Comienza mision
        if (!questStarted && !(missionDone)) //  && questGiver.GetComponent<Quest_Starter>().misionAceptada
        {
            CambiarMapaInicio();
            ObjetivosPantallaON();

            /*
            OnOffPlayer();

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

        GameObject animales = GameObject.Find("Animales");
        animales.SetActive(false);

        RenderSettings.fog = true;
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

       
        //OnOffPlayer();

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


    /*
    public override void Interact()
    {
        base.Interact();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
        playerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
        npcPosition = new Vector3(transform.position.x, 1, transform.position.z);

        /////////Rotacion del NPC al jugador//////////////////////
        Quaternion rotation = Quaternion.LookRotation(playerPosition - transform.position);
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;
        //////////////////////////////////////////////////////////

        ///////////Rotacion del jugador al NPC////////////////////
        rotation = Quaternion.LookRotation(npcPosition - player.transform.position);
        rotation.x = 0f;
        rotation.z = 0f;
        player.transform.rotation = rotation;
        //////////////////////////////////////////////////////////

        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        NpcNav.isInDialogue = true;
        Cursor.lockState = CursorLockMode.Confined;

        dialogoObjetivo.SetActive(true);

    }
    */
}
