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
    void Start()
    {
        CScenarioRef = GameObject.Find("CScenario");

        refAlcalde = CScenarioRef.transform.GetChild(0).GetChild(0).gameObject;
        refPosCamAlcalde = CScenarioRef.transform.GetChild(0).GetChild(1).gameObject;

        player = GameObject.Find("Player");
        microfonoRef = GameObject.Find("Podio").transform.GetChild(0).gameObject;
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

        ///////// paneo de camara
        


        //////////// Activar tablero Ritmo
        MisionRitmo.transform.GetChild(0).gameObject.SetActive(true);

        //////////////////////////////  Mover player posicion y rotacion camara a escenario.

        player.transform.position = microfonoRef.transform.GetChild(3).transform.position;//new Vector3(-45, 2.13f, -94.75f);

        Vector3 targetPosition = microfonoRef.transform.position;
        targetPosition.y = player.transform.position.y;
        player.transform.LookAt(targetPosition);
    
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

    void SetActivePlayerScripts(bool valor){
        player.GetComponent<AbrirMapa>().enabled = valor;
        player.GetComponent<Mascota>().enabled = valor;
        player.GetComponent<CharacterMovement>().enabled = valor;
        player.GetComponent<FpsCamera>().enabled = valor;
        player.GetComponent<CameraInteraction>().enabled = valor;
    }

}
