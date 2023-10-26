using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarDialogoAAAAA : Interactable
{

    public GameObject dialogoObjetivo;
    private Vector3 movementDirection;
    private Vector3 playerPosition;
    private Vector3 npcPosition;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = transform.forward;
    }
    public override void Interact(){
        
        base.Interact();

        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        NpcNav.isInDialogue = true;
        Cursor.lockState = CursorLockMode.Confined;

        dialogoObjetivo.SetActive(true);

    }
}
