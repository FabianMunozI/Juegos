using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasuraPlaya : MonoBehaviour
{
    [Header("Parametros Mision")]
    public int basurasARecoger = 3;
    int basurasRecogidas = 0;
    public bool missionDone = false; // Mision ya hecha


    [Header("Config Camara Final")]
    public float orbitSpeed = 20f;
    public float tiempoLimiteFinal = 6f;
    public float tiempoLimiteInicio = 6f;
    
    bool avRotate = false;
    bool avRotate0 = false;

    Quaternion rotOriginal;

    [Header("Asignaciones")]
    private bool Quest_started = false; // Mision en curso
    

    public GameObject questGiver;
    public GameObject infoGiver;


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



    public GameObject minimapaSenal;
    public GameObject infoSenal;
    public GameObject npcInfo;


    public GameObject npcMision;
    public GameObject cosaParaAgarrar;

    [Header("Ignorar")]

    public int width = 256;
    public int height = 256;
    public int center_x = 0;
    public int center_y = 0;

    GameObject playa_terrain;
    GameObject basuras_container;
    GameObject basura_recogida_container;
        private Vector3 centroPlaya;


    void Start()
    {
        playa_terrain = GameObject.Find("Terrain(Clone)");
        Jugador = GameObject.Find("Player");
        CamaraO = GameObject.Find("Camera");
        basuras_container = GameObject.Find("BasuraPlayaCont");
        basura_recogida_container = GameObject.Find("BasuraRecogida");
        centroPlaya = new Vector3 (center_x + width/2, 0f, center_y + height/2);

        //AgregarComponente(arbolesMision);
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
        if (basurasRecogidas >= basurasARecoger && !missionDone)
        {
            OnOffPlayer();
            missionDone = true;
            Quest_started = false;
            questTitle.text = "Mision Terminada!";
            questText.text = "La playa se ha limpiado!";

            npcInfo.gameObject.layer = LayerMask.NameToLayer("dialogable");

            infoSenal.SetActive(true);
            infoGiver.SetActive(true);
            basuras_container.SetActive(false);
            RotarCamaraArboles(orbitSpeed, 50f, 8f);
            Invoke("OnOffPlayer", 8f);
            Invoke("desactivarTexto",3f);

        }


        // Apenas se inicia la mision
        if (!Quest_started && !(missionDone) && questGiver.GetComponent<Quest_Starter>().misionAceptada )
        {
            minimapaSenal.SetActive(false);
            Quest_started = true;
            questGiver.SetActive(false);
            
            OnOffPlayer();
            avRotate0 = true;
            questTracker.SetActive(true);

            RotarCamaraArboles2(orbitSpeed, 50f, 8f);
            Invoke("OnOffPlayer", tiempoLimiteInicio + 1f);

            npcMision.GetComponent<Animator>().SetBool("passItem", true);

            cosaParaAgarrar.SetActive(true);
            cosaParaAgarrar.GetComponent<Outline>().enabled = true;

            // Cambiar a basura a interaccion
            for (int i = 0; i < basuras_container.transform.childCount; i++)
            {
                basuras_container.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("interactable");
            }

            

        }

        // Ciclo de mision
        if (Quest_started)
        {
            basurasRecogidas = basura_recogida_container.transform.childCount;
            if(!cosaParaAgarrar.transform.IsChildOf(transform))
            {
                npcMision.GetComponent<Animator>().SetBool("passItem", false);
            }

            questText.text = "¡Recoge "+ basurasARecoger +" basuras en la playa!\n Llevas "+ basurasRecogidas +"/"+ basurasARecoger+ ".";


        }

        if (avRotate && tiempoLimiteFinal > 0)
        {
            CamaraO.transform.RotateAround(centroPlaya, Vector3.up, orbitSpeed * Time.deltaTime);
            tiempoLimiteFinal -= Time.deltaTime;
        }

        if (avRotate0 && tiempoLimiteInicio > 0)
        {
            CamaraO.transform.RotateAround(centroPlaya, Vector3.up, orbitSpeed * Time.deltaTime);
            tiempoLimiteInicio -= Time.deltaTime;
        }

    }

    private int contador_basura = 0;

    void CameraToTrash()
    {

        if (contador_basura == 1)
        {
            OnOffPlayer();
        }
        if (contador_basura == 0){
            
            Transform child = GameObject.Find("BasuraPlayaCont").transform.GetChild(contador_basura);

            Vector3 pos = child.position - Jugador.transform.position;
            pos += new Vector3 (3,4,4);
            rotOriginal = CamaraO.transform.rotation;
            CamaraO.transform.Translate(pos, Space.World);
            CamaraO.transform.LookAt(child.position);

            contador_basura++;
            Invoke("CameraToTrash", 2f);
        }

    }

    public void RotarCamaraArboles2(float orbitSpeed, float distancia, float tiempoLimiteFinal)
    {
        Vector3 pos = centroPlaya - Jugador.transform.position;
        pos += new Vector3 (distancia,3*distancia/4,distancia);

        CamaraO.transform.Translate(pos, Space.World);
        CamaraO.transform.LookAt(centroPlaya);

        avRotate0 = true;

    } 

    public void RotarCamaraArboles(float orbitSpeed, float distancia, float tiempoLimiteFinal)
    {
        Vector3 pos = centroPlaya - Jugador.transform.position;
        pos += new Vector3 (distancia,3*distancia/4,distancia);

        CamaraO.transform.Translate(pos, Space.World);
        CamaraO.transform.LookAt(centroPlaya);

        avRotate = true;

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

 


    void desactivarTexto(){ 
        questTracker.SetActive(false);
    }






    

}
