using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonScript : Interactable
{
    Animator controller;
    public GameObject Interactuable;
    void Start() {
        controller= transform.GetChild(1).gameObject.GetComponent<Animator>();
        
    }
    // Start is called before the first frame update
    public override void Interact()
    {
        base.Interact();
        //Debug.Log(gameObject.transform.rotation.eulerAngles);
        controller.SetTrigger("Iniciar");
        Interactuable.GetComponent<Interactable>().Interact();
        
        //transform.Rotate(Vector3.up * 90);
    }
}
