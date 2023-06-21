using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoneTrigger : MonoBehaviour
{

    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;
    public GameObject dialogoObjetivo;
    bool playerInZone = false;
    // Start is called before the first frame update
    void Update(){
        if(playerInZone){
            if(Input.GetKey(KeyCode.E)){
                CharacterMovement.movementDialogue = true;
                CameraInteraction.interactionDialogue = true;
                FpsCamera.cameraDialogue = true;
                onTriggerExit.Invoke();
                dialogoObjetivo.SetActive(true);
            }
            if(!dialogoObjetivo.activeSelf){
                onTriggerEnter.Invoke();
            }
        }
    }
    public void OnTriggerEnter(Collider other) {
        onTriggerEnter.Invoke();
        playerInZone = true;
    }

    public void OnTriggerExit(Collider other) {
        onTriggerExit.Invoke();
        playerInZone = false;
    }
}
