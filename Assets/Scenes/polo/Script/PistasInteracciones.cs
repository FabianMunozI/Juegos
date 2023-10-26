using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistasInteracciones : Interactable
{
    private GameObject dialogo;
    private Vector3 movementDirection;
    private Vector3 playerPosition;
    private Vector3 npcPosition;
    private GameObject player;
    

    void Start()
    {
        

        if (transform.name == "ninia(Clone)")
        {
            dialogo = GameObject.Find("Canvas").transform.GetChild(4).transform.GetChild(0).gameObject;
        }
        else if (transform.name == "huesoPescado(Clone)")
        {
            dialogo = GameObject.Find("Canvas").transform.GetChild(4).transform.GetChild(1).gameObject;
        }
        else
        {
            dialogo = GameObject.Find("Canvas").transform.GetChild(4).transform.GetChild(2).gameObject;
        }
    }

    
    void Update()
    {
        movementDirection = transform.forward;
    }
    public override void Interact()
    {

        base.Interact();
        player = GameObject.FindGameObjectWithTag("ninia(Clone)");
        playerPosition = player.transform.position;
        playerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
        npcPosition = new Vector3(transform.position.x, 1, transform.position.z);

        /////////Rotacion del NPC al jugador//////////////////////
        ///
        Quaternion rotation = Quaternion.LookRotation(playerPosition - transform.position);
        rotation.x = 0f;
        rotation.z = 0f;
        if (transform.name == "niña")
        {
            transform.rotation = rotation;
        }
        
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
        Cursor.lockState = CursorLockMode.Confined;

        dialogo.SetActive(true);

    }
}
