using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Script para objeto que dará la mision
public class Quest_StarterV2 : Interactable
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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPosition = player.transform.position;
        playerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
        Vector3 npcPosition = new Vector3(transform.position.x, 1, transform.position.z);

        /////////Rotacion del NPC al jugador//////////////////////
        Quaternion rotation = Quaternion.LookRotation(playerPosition - transform.position);
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;

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
