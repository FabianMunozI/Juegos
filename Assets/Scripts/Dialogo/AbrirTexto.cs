using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirTexto : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   

        Invoke("Inicio", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Inicio(){
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        NpcNav.isInDialogue = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

}
