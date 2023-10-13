using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerrarScrollable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Abrir(){
        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        NpcNav.isInDialogue = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void Cerrar(){
        CharacterMovement. movementDialogue = false;
        FpsCamera.cameraDialogue = false;
        CameraInteraction.interactionDialogue = false;
        NpcNav.isInDialogue = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
