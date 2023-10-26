using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MisionTrivia : MonoBehaviour
{
    public bool missionDone = false; // Mision ya hecha
    private bool Quest_started = false; // Mision en curso
    public GameObject questGiver;
    public GameObject infoGiver;
    public GameObject questTracker;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questText;
    public GameObject minimapaSenal;
    public GameObject infoSenal;
    public GameObject npcInfo;

    //public GameObject dialogoObjetivo;
    //public Button A_Button, B_Button, C_Button, D_Button;
    private  GameObject Jugador;
    public bool inputTrue = false;
    GameObject CamaraO;
    Quaternion rotOriginal;

    public static int cont_correctas = 0;
    public static int intento_actual = 0;
    public static string[] textos_sup = {
        "¿Qué tipo de contaminante es el que mas daña a las aves y peces en el océano?",
        "¿Qué impacto tiene la contaminación del petróleo en la vida marina?",
        "A lo largo del mundo existen areas marinas protegidas, para:",
        "¿Qué debemos evitar en la playa para proteger a las aves marinas?",
        "Sabias que como individuos podemos realizar distintas actividades sobre el cuidado de la fauna marina, por ejemplo."
    };
    public static string[,] texto_botones = new string[,]
    {
        {"Comida", "Plástico", "Rocas", "Arena"},
        {"Fomenta la salud de los ecosistemas marinos", "Mejora la calidad del agua", "Dañan y matan a la vida marina", "No tiene ningún impacto"},
        {"Facilitar la pesca comercial", "Conservar la biodiversidad marina y su hábitat", "Permitir la recreación sin restricciones", "Fomentar la caza de especies en peligro"},
        {"Alimentarlas con nuestras sobras", "Recolectar conchas y arena", "Dejar basura y plástico", "Tomar sol"},
        {"Tirar basura en la playa", "Utilizar productos de plástico de un solo uso", "Pescar sin restricciones", "Participar en limpiezas costeras"}
    };
    public static int[] respuestas = {1,2,1,2,3};
    public static int listerners_botones = 0;
    private GameObject triviaGO;
    void Start()
    {
        Jugador = GameObject.Find("Player");
        CamaraO = GameObject.Find("Camera");

        triviaGO = GameObject.Find("NpcsPlaya");

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

    // Update is called once per frame
    void Update()
    {
        if ( (cont_correctas == 3 || intento_actual - cont_correctas == 3) && !missionDone)
        {
            missionDone = true;
            Quest_started = false;
            questTitle.text = "Mision Terminada!";
            questText.text = "La playa se ha limpiado!";


            infoSenal.SetActive(true);
            infoGiver.SetActive(true);
            Invoke("desactivarTexto",3f);
            npcInfo.gameObject.layer = LayerMask.NameToLayer("dialogable");         

            for (int i = 0; i < triviaGO.transform.childCount; i++)
            {
                triviaGO.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");                
            }


        }


        // Apenas se inicia la mision
        if (!Quest_started && !(missionDone) && questGiver.GetComponent<Quest_StarterV2>().misionAceptada )
        {
            minimapaSenal.SetActive(false);
            Quest_started = true;
            questGiver.SetActive(false);
            
            questTracker.SetActive(true);

            for (int i = 0; i < triviaGO.transform.childCount; i++)
            {
                triviaGO.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("interactable");                
            }

            npcInfo.SetActive(true);
            npcInfo.transform.rotation = questGiver.transform.rotation;
            npcInfo.transform.position = questGiver.transform.GetChild(2).transform.position;

        }

        if (Quest_started)
        {
            questText.text = " - Difundido correctamente "+ cont_correctas +"/3.\n - Intentos fallidos "+ (intento_actual - cont_correctas) +"/3.";
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

    void desactivarTexto(){ 
        questTracker.SetActive(false);
    }

}
