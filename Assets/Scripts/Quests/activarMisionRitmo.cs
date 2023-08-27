using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activarMisionRitmo : Interactable
{
    // Start is called before the first frame update
    public GameObject MisionRitmo;
    GameObject player;

    GameObject CScenarioRef;

    GameObject refAlcalde;

    GameObject refPosCamAlcalde;

    GameObject microfonoRef;
    bool activarUpdate;
    float distanceCamPos;

    Vector3 initialDistance;

    bool segUpdate;

    void Start()
    {
        CScenarioRef = GameObject.Find("CScenario");

        refAlcalde = CScenarioRef.transform.GetChild(0).GetChild(0).gameObject;
        refPosCamAlcalde = CScenarioRef.transform.GetChild(0).GetChild(1).gameObject;

        player = GameObject.Find("Player");
        microfonoRef = GameObject.Find("Podio").transform.GetChild(0).gameObject;
    }

    void Update(){
        if(activarUpdate){
            player.transform.position += initialDistance * 1/distanceCamPos * Time.deltaTime * 4;

            Vector3 direct = (refAlcalde.transform.position - player.transform.position).normalized;
            Quaternion rotGoal = Quaternion.LookRotation(direct);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotGoal, 20f *Time.deltaTime);

            if(Vector3.Distance(player.transform.position, refPosCamAlcalde.transform.position) <=0.5){
                activarUpdate=false;
                Invoke("activarInvoke2daAnimacion",1f);
                distanceCamPos = (Vector3.Distance(player.transform.position, microfonoRef.transform.GetChild(3).transform.position)) /* / 4f */; // distancia dividido tiempo
                initialDistance = microfonoRef.transform.GetChild(3).transform.position - player.transform.position;
            } 
        }

        if(segUpdate){
            player.transform.position += initialDistance * 1/distanceCamPos * Time.deltaTime * 4;

            Vector3 direct = (microfonoRef.transform.parent.GetChild(2).transform.position - player.transform.position).normalized;
            Quaternion rotGoal = Quaternion.LookRotation(direct);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotGoal, 2f *Time.deltaTime);

            if(Vector3.Distance(player.transform.position, microfonoRef.transform.GetChild(3).transform.position) <=0.5){
                segUpdate = false;
                player.GetComponent<Collider>().isTrigger = false;
                player.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    public override void Interact(){ // preguntar mision
        base.Interact();
        MisionRitmo.transform.GetChild(1).gameObject.SetActive(true); // aceptar o rechazar mision
        Cursor.lockState = CursorLockMode.Confined;
        SetActivePlayerScripts(false);

    }

    public void iniciarMision(){
        // ACTIVAR POLITICO, Y NPCS
        CScenarioRef.transform.GetChild(0).gameObject.SetActive(true);

        ///////// paneo de camara suave
        
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Collider>().isTrigger = true;
        //player.transform.position = refPosCamAlcalde.transform.position;
        

        distanceCamPos = (Vector3.Distance(player.transform.position, refPosCamAlcalde.transform.position)) /* / 4f */; // distancia dividido tiempo
        initialDistance = refPosCamAlcalde.transform.position - player.transform.position;
        activarUpdate = true;

        //////////// Activar tablero Ritmo
        MisionRitmo.transform.GetChild(0).gameObject.SetActive(true);

        //////////////////////////////  Mover player posicion y rotacion camara a escenario.

    
        ////////////////////////// desactiva el canvas si iniciar o no misin

        MisionRitmo.transform.GetChild(1).gameObject.SetActive(false); 

        //////// desactivar mensaje de interaccion para
        GameObject.Find("Interactuar").SetActive(false);
    }
    public void noIniciar(){
        MisionRitmo.transform.GetChild(1).gameObject.SetActive(false);
        SetActivePlayerScripts(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void activarInvoke2daAnimacion(){
        segUpdate = true;
    }

    void SetActivePlayerScripts(bool valor){
        player.GetComponent<AbrirMapa>().enabled = valor;
        player.GetComponent<Mascota>().enabled = valor;
        player.GetComponent<CharacterMovement>().enabled = valor;
        player.GetComponent<FpsCamera>().enabled = valor;
        player.GetComponent<CameraInteraction>().enabled = valor;
    }

}
