using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activarMisionRitmo : Interactable
{
    // Start is called before the first frame update
    public GameObject MisionRitmo;
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    public override void Interact(){ // preguntar mision
        base.Interact();
        MisionRitmo.transform.GetChild(1).gameObject.SetActive(true); // aceptar o rechazar mision
        Cursor.lockState = CursorLockMode.Confined;
        SetActivePlayerScripts(false);

    }

    public void iniciarMision(){
        MisionRitmo.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void noIniciar(){
        MisionRitmo.transform.GetChild(1).gameObject.SetActive(false);
        SetActivePlayerScripts(true);
    }

    void SetActivePlayerScripts(bool valor){
        player.GetComponent<AbrirMapa>().enabled = valor;
        player.GetComponent<Mascota>().enabled = valor;
        player.GetComponent<CharacterMovement>().enabled = valor;
        player.GetComponent<FpsCamera>().enabled = valor;
        player.GetComponent<CameraInteraction>().enabled = valor;
    }

}
