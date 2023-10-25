using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarMisionEolica : Interactable
{   

    public GameObject canvasMision;
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

        canvasMision.transform.GetChild(0).gameObject.SetActive(false);
        SetActivePlayerScripts(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void NoIniciarMision(){
        canvasMision.transform.GetChild(0).gameObject.SetActive(false);
        SetActivePlayerScripts(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Interact(){ // preguntar mision
        base.Interact();
        canvasMision.transform.GetChild(0).gameObject.SetActive(true); // aceptar o rechazar mision
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
