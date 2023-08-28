using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarMisionAgua : Interactable
{
    public GameObject canvasMisionAgua;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IniciarMision(){
        AguaSurtidor.misionActiva = true;
        canvasMisionAgua.transform.GetChild(0).gameObject.SetActive(false);
        SetActivePlayerScripts(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void NoIniciarMision(){
        canvasMisionAgua.transform.GetChild(0).gameObject.SetActive(false);
        SetActivePlayerScripts(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Interact(){ // preguntar mision
        base.Interact();
        canvasMisionAgua.transform.GetChild(0).gameObject.SetActive(true); // aceptar o rechazar mision
        Cursor.lockState = CursorLockMode.Confined;
        SetActivePlayerScripts(false);

    }

    void SetActivePlayerScripts(bool valor){
        player.GetComponent<AbrirMapa>().enabled = valor;
        player.GetComponent<Mascota>().enabled = valor;
        player.GetComponent<CharacterMovement>().enabled = valor;
        player.GetComponent<FpsCamera>().enabled = valor;
        CameraInteraction.interactionDialogue = !valor;
        //player.GetComponent<CameraInteraction>().enabled = valor;
    }
}
