using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Script para objeto que dará la mision
public class Quest_Starter : Interactable
{

    public Button acceptButton, declineButton;
    public GameObject dialogoObjetivo;
    public bool misionAceptada = false;


    void Awake()
    {
        acceptButton.onClick.AddListener(acceptButton_Action);
        declineButton.onClick.AddListener(declineButton_Action);
    }

    public override void Interact(){

        Cursor.lockState = CursorLockMode.None;
        
        base.Interact();

        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        dialogoObjetivo.SetActive(true);

    }

    public void acceptButton_Action()
    {
        misionAceptada = true;
        retake_control();
    }

    public void declineButton_Action()
    {
        retake_control();
    }

    void retake_control()
    {
        CharacterMovement.movementDialogue = false;
        CameraInteraction.interactionDialogue = false;
        FpsCamera.cameraDialogue = false;
        dialogoObjetivo.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }


    
}
