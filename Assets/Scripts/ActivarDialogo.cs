using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarDialogo : Interactable
{

    public GameObject dialogoObjetivo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Interact(){
        
        base.Interact();
        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        dialogoObjetivo.SetActive(true);

    }
}