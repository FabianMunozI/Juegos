using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarDialogo : Interactable
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
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
        playerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
        npcPosition = new Vector3(transform.position.x, 1, transform.position.z);

        /////////Rotacion del NPC al jugador//////////////////////
        Quaternion rotation = Quaternion.LookRotation(playerPosition - transform.position);
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;
        //////////////////////////////////////////////////////////

        ///////////Rotacion del jugador al NPC////////////////////
        rotation = Quaternion.LookRotation(npcPosition - player.transform.position);
        rotation.x = 0f;
        rotation.z = 0f;
        player.transform.rotation = rotation;
        //////////////////////////////////////////////////////////

        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        NpcNav.isInDialogue = true;

        dialogoObjetivo.SetActive(true);

    }
}
