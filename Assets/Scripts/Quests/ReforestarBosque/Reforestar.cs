using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Reforestar : MonoBehaviour
{
    // Estado Mision
    private bool Quest_started = false; // Mision en curso
    public bool missionDone = false; // Mision ya hecha

    public GameObject questGiver;
    public GameObject infoGiver;
    public GameObject arbolesMision;
    
    public GameObject palaPrefab, semillaPrefab, regaderaPrefab;
    public GameObject tumultoPrefab, plantaPrefab, arbolPrefab;
    public GameObject tumultoContainter, plantasContainer;
    GameObject pala, semillas, regadera;


    int asignador = 0;

    int arbolesPlantados = 0;

    // UI
    public GameObject questTracker;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questText;

    // Personaje - Camara
    Vector3 posCamara;
    GameObject CamaraO;
    Vector3 VectorPartida;
    public bool inputTrue = false;
    public GameObject Jugador;
    public GameObject centroBosque;

    public float orbitSpeed = 5f;

    bool avRotate = false;

    public float tiempoLimite = 4f;

    Quaternion rotOriginal;

    public GameObject minimapaSenal;
    public GameObject arbolesSenalPrefab;
    List <GameObject> senalesArboles = new List <GameObject>();

    public GameObject infoSenal;
    public GameObject npcInfo;




    void Start()
    {
        Jugador = GameObject.Find("Player");
        CamaraO = GameObject.Find("Camera");

        AgregarComponente(arbolesMision);
        questTracker.SetActive(false);
        infoGiver.SetActive(false);
        if(!missionDone)
        {
            minimapaSenal.SetActive(true);
            infoSenal.SetActive(false);
        } else{
            infoSenal.SetActive(true);
        }
    }

    void Update()
    {


        // Terminar mision

        if (arbolesPlantados >= 3 && !missionDone)
        {
            //arbolesSenal.SetActive(false);
            OnOffPlayer();
            missionDone = true;
            Quest_started = false;
            questTitle.text = "Mision Terminada!";
            questText.text = "El bosque vuelve a la vida!";
            Invoke("ReemplazarRestoArboles",2f);

            npcInfo.gameObject.layer = LayerMask.NameToLayer("dialogable");

            foreach(var senal in senalesArboles)
            {
                senal.SetActive(false);
            }

            infoSenal.SetActive(true);
            infoGiver.SetActive(true);
            RotarCamaraArboles(orbitSpeed, 15f, 8f);
            Invoke("OnOffPlayer", 8f);
            Invoke("desactivarTexto",3f);



        }


        // Apenas se inicia la mision
        if (!Quest_started && !(missionDone) && questGiver.GetComponent<Quest_Starter>().misionAceptada )
        {
            minimapaSenal.SetActive(false);
            Quest_started = true;
            questGiver.SetActive(false);
            //arbolesSenal.SetActive(true);
            
            pala = Instantiate(palaPrefab, new Vector3(1,3,-40), Quaternion.Euler(0,0,0));
            semillas = Instantiate(semillaPrefab, new Vector3(2,3,-40), Quaternion.Euler(0,0,0));
            regadera = Instantiate(regaderaPrefab, new Vector3(3,3,-40), Quaternion.Euler(0,0,0));
            AsignarObjeto(arbolesMision, palaPrefab);

            OnOffPlayer();

            Vector3 pos = semillas.transform.position - Jugador.transform.position;
            pos += new Vector3 (-5,5,-5);

            rotOriginal = CamaraO.transform.rotation;

            CamaraO.transform.Translate(pos, Space.World);
            CamaraO.transform.LookAt(semillas.transform);

            Invoke("OnOffPlayer", 2f);
            questTracker.SetActive(true);



        }

        // Ciclo de mision
        if (Quest_started)
        {
            questText.text = "¡Planta y haz crecer 3 arboles!\n Llevas "+ arbolesPlantados +"/3.";
            
            for (int i = 0; i < arbolesMision.transform.childCount; i++)
            {
                if(arbolesMision.transform.GetChild(i).gameObject.GetComponent<ArbolReforestar>().to_remove)
                {
                    Vector3 pos = arbolesMision.transform.GetChild(i).gameObject.transform.position;
                    pos.y = pos.y - 2;

                    var tumulto = Instantiate(tumultoPrefab, pos, Quaternion.Euler(0,0,0));

                    tumulto.GetComponent<TumultoReforestar>().semillaPrefab = semillaPrefab;
                    tumulto.GetComponent<TumultoReforestar>().indice = asignador;
                    tumulto.gameObject.layer = LayerMask.NameToLayer("interactable");
                    tumulto.transform.parent = tumultoContainter.transform;


                    asignador ++;
                    arbolesMision.transform.GetChild(i).gameObject.GetComponent<ArbolReforestar>().to_remove = false;
                    arbolesMision.transform.GetChild(i).gameObject.SetActive(false);
                }
                
            }

            for (int i = 0; i < tumultoContainter.transform.childCount; i++)
            {
                if(tumultoContainter.transform.GetChild(i).gameObject.GetComponent<TumultoReforestar>().plantar)
                {
                    Vector3 pos = tumultoContainter.transform.GetChild(i).gameObject.transform.position;
                    pos.y += 1.5f;

                    var plantita = Instantiate(plantaPrefab, pos, Quaternion.Euler(0,0,0));
    
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
                if(plantasContainer.transform.GetChild(i).gameObject.GetComponent<ArbolitoReforestar>().to_remove)
                {
                    Vector3 pos = plantasContainer.transform.GetChild(i).gameObject.transform.position;
                    pos.y -= 0.5f;

                    Instantiate(arbolPrefab, pos, Quaternion.Euler(0,0,0));



                    plantasContainer.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
                    plantasContainer.transform.GetChild(i).gameObject.GetComponent<ArbolitoReforestar>().to_remove = false;
                    EliminarTumultoArbolito(plantasContainer.transform.GetChild(i).gameObject.GetComponent<ArbolitoReforestar>().indice);
                    arbolesPlantados ++;
                    plantasContainer.transform.GetChild(i).gameObject.SetActive(false);
                }
                
            }


        }

        if (avRotate && tiempoLimite > 0)
        {
            CamaraO.transform.RotateAround( centroBosque.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
            tiempoLimite -= Time.deltaTime;
        }

    }

    public void RotarCamaraArboles(float orbitSpeed, float distancia, float tiempoLimite)
    {

        Vector3 pos = centroBosque.transform.position - Jugador.transform.position;
        pos += new Vector3 (distancia,distancia,distancia);

        CamaraO.transform.Translate(pos, Space.World);
        CamaraO.transform.LookAt(centroBosque.transform);

        avRotate = true;

    }

    public void RotarCamara(){
        int grados = 0;
        while(grados < 360)
        {
            CamaraO.transform.Rotate(0, -10 * Time.deltaTime, 0);
            grados ++;
            Debug.Log("Rotando camara");
        }
    }

    public void OnOffPlayer(){
        inputTrue = !inputTrue; // se vuelve a true el valor // inicia false en el start
        //Jugador.GetComponent<Rigidbody>().isKinematic = inputTrue;
        Jugador.GetComponent<CharacterMovement>().AnimacionOn = !inputTrue; // frezea al player mientras esta la animacion
        Jugador.GetComponent<FpsCamera>().animacionOn = !inputTrue;

        if(inputTrue==false){ // si ya se inicio la mision y se esta terminando / devolviendo controles al player
        // hay que desactivar que la mision pueda ser tomada
            //referenciaExclamacion.SetActive(inputTrue);
            //controller.SetTrigger("End3");
            //Debug.Log("volvieron los controles");
            // referenciaExclamacion.SetActive(false);
            //this.GetComponent<MeshRenderer>().material.color=Color.green;

            CamaraO.transform.localPosition = new Vector3(0,0.69f,0);
            CamaraO.transform.rotation = rotOriginal;
            
            
        }

    }





    public void AgregarComponente(GameObject contenedor)
    {
        for (int i = 0; i < contenedor.transform.childCount; i++)
        {
            contenedor.transform.GetChild(i).gameObject.AddComponent<ArbolReforestar>();
            
        }
    }

    public void AsignarObjeto(GameObject contenedor, GameObject objeto)
    {
        for (int i = 0; i < contenedor.transform.childCount; i++)
        {
            
            contenedor.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("interactable");
            contenedor.transform.GetChild(i).gameObject.GetComponent<ArbolReforestar>().palaPrefab = objeto;

            if(contenedor.transform.GetChild(i).gameObject.GetComponent<Collider>() != null)
            {
                senalesArboles.Add(Instantiate(arbolesSenalPrefab, contenedor.transform.GetChild(i).gameObject.transform.position + new Vector3(0,5,0), Quaternion.Euler(-90,0,0) ));
            }

        }
    }

    public void EliminarTumultoArbolito(int indice)
    {
        for (int i = 0; i < tumultoContainter.transform.childCount; i++)
        {
            if (tumultoContainter.transform.GetChild(i).gameObject.GetComponent<TumultoReforestar>().indice == indice)
            {
                tumultoContainter.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void desactivarTexto(){ 
        questTracker.SetActive(false);
    }

    public void ReemplazarRestoArboles()
    {
        Debug.Log("Reemplazando");
        for (int i = 0; i < arbolesMision.transform.childCount; i++)
        {
            Vector3 pos = arbolesMision.transform.GetChild(i).gameObject.transform.position;
            pos.y = pos.y - 2;

            var tumulto = Instantiate(tumultoPrefab, pos, Quaternion.Euler(0,0,0));

            tumulto.GetComponent<TumultoReforestar>().indice = asignador;
            tumulto.transform.parent = tumultoContainter.transform;

            arbolesMision.transform.GetChild(i).gameObject.SetActive(false);
        }


        Invoke("PlantarArbolitos", 2f);


    }


    public void PlantarArbolitos()
    {
        Debug.Log("Plantando");

        for (int i = 0; i < tumultoContainter.transform.childCount; i++)
        {
            Vector3 pos = tumultoContainter.transform.GetChild(i).gameObject.transform.position;
            pos.y += 1.5f;

            var plantita = Instantiate(plantaPrefab, pos, Quaternion.Euler(0,0,0));
            plantita.transform.parent = plantasContainer.transform;

        }

        Invoke("CrecerArboles", 2f);
    }

    public void CrecerArboles()
    {
        Debug.Log("Creciendo");

        for (int i = 0; i < plantasContainer.transform.childCount; i++)
        {
            Vector3 pos = plantasContainer.transform.GetChild(i).gameObject.transform.position;
            pos.y -= 0.5f;

            Instantiate(arbolPrefab, pos, Quaternion.Euler(0,0,0));

            for (int j = 0; j < tumultoContainter.transform.childCount; j++)
            {
                tumultoContainter.transform.GetChild(j).gameObject.SetActive(false);
            }

            for (int j = 0; j < plantasContainer.transform.childCount; j++)
            {
                plantasContainer.transform.GetChild(j).gameObject.SetActive(false);
            }


        }

        
    }



    

}
