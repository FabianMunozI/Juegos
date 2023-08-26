using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;

public class Mascota : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject mascota;

    GameObject camaraPlayer;
    GameObject camaraMascota;

    GameObject zonasGeneracion;

    Transform generarAqui;

    bool mascotaUp;

    public bool eresLaMascota;


    // de aqui para abajo scripts player
    CharacterMovement pM;
    FpsCamera pCamera;
    CameraInteraction pInteraction;
    //AbrirMapa ??? idk


    //de aqui para abajo scripts mascota  // recordar añadir todos los que puedan dar problemas
    CharacterMovement mascotaMov;
    FpsCamera mascotaCameraScript;


    void Start()
    {
        eresLaMascota=false;
        generarAqui=null;

        mascota = GameObject.Find("CMascota").transform.GetChild(0).gameObject;

        zonasGeneracion = GameObject.Find("ZonasGeneracionMascota");
        camaraPlayer = GameObject.Find("Player").transform.GetChild(0).gameObject;


        camaraMascota = mascota.transform.GetChild(1).gameObject;
        mascotaUp = false;

        /////////////////////////////////////////////////////////////

        pM = GetComponent<CharacterMovement>();
        pCamera = GetComponent<FpsCamera>();
        pInteraction = GetComponent<CameraInteraction>();

        mascotaMov = mascota.GetComponent<CharacterMovement>();
        mascotaCameraScript = mascota.GetComponent<FpsCamera>();

        //////
    }

    // Update is called once per frame
    void Update()
    {
        if(!mascotaUp && Input.GetKeyDown(KeyCode.G)){ //generar mascota
            for(int i=0; i<4; i++){
                if(zonasGeneracion.transform.GetChild(i).GetComponent<SinColision>().Disponible==0){
                    generarAqui = zonasGeneracion.transform.GetChild(i);
                    mascota.transform.position = generarAqui.position;
                    mascota.SetActive(true);
                    mascotaUp = !mascotaUp;
                    break;
                }
            }
            // aqui codigo por si no habian espacion disponibles para generar a la mascota sin bugearla?
            if(generarAqui==null){
                Debug.Log("Mostrar por pantalla que no hay espacio para generar la mascota");

                mascotaUp=false;
            }


        }else if(mascotaUp && Input.GetKeyDown(KeyCode.G)){ // ocultar mascota
            if(!eresLaMascota){
                generarAqui=null;
                mascota.SetActive(false);

                mascotaUp=!mascotaUp;
            }else{
                Debug.Log("debes dejar de ser la mascota 1ro");
                //esto hay que hacerlo por pantalla
            }
            
        }

        if(mascotaUp && Input.GetKeyDown(KeyCode.Tab)){
            //Debug.Log("tabulador");

            if(!eresLaMascota){
                //desactivar scripts player, cambiar de camara habilitanto una y deshabilitando la otra
                scriptsPlayer(false);
                scriptsMascota(true);

                eresLaMascota= !eresLaMascota;

            }else if(eresLaMascota){
                //habilitar scripts player, deshabilitar scripts mascota, y cambiar de camara
                scriptsPlayer(true);
                scriptsMascota(false);

                eresLaMascota= !eresLaMascota;
            }


        }


        if(mascotaUp && Vector3.Distance(mascota.transform.position, transform.position)>100){ // si estas muy lejos de la mascota

            if(eresLaMascota){ // si eres la mascota te cambia automaticamente al player
                scriptsPlayer(true);
                scriptsMascota(false);

                eresLaMascota= !eresLaMascota;
            }else{  // desactiva la mascota 
                generarAqui=null;
                mascota.SetActive(false);

                mascotaUp=!mascotaUp;

            }

            
        }



    }


    void scriptsPlayer(bool nuevoBool){
        pM.enabled = nuevoBool;
        pCamera.enabled = nuevoBool;
        pInteraction.enabled = nuevoBool;
        camaraPlayer.SetActive(nuevoBool);
    }

    void scriptsMascota(bool nuevoBool){
        mascotaMov.enabled = nuevoBool;
        mascotaCameraScript.enabled = nuevoBool;
        camaraMascota.SetActive(nuevoBool);
    }

}
