using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Inventory_Handler : MonoBehaviour
{

    public GameObject Inventory;

    void Start()
    {
        Inventory.GetComponent<CanvasGroup>().alpha = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            if(Inventory.GetComponent<CanvasGroup>().alpha == 1)
            {
                //Inventory.SetActive(false);
                Inventory.GetComponent<CanvasGroup>().alpha = 0;
                CharacterMovement.movementDialogue = false;
                CameraInteraction.interactionDialogue = false;
                FpsCamera.cameraDialogue = false;
                Cursor.lockState = CursorLockMode.Locked;
                    
            }
                
            else
            {
                //Inventory.SetActive(true);
                Inventory.GetComponent<CanvasGroup>().alpha = 1;
                Cursor.lockState = CursorLockMode.None;
                CharacterMovement.movementDialogue = true;
                CameraInteraction.interactionDialogue = true;
                FpsCamera.cameraDialogue = true;
                
            }
        }
    }
}
