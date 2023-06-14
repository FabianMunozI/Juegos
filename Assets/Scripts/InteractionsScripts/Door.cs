using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : Interactable
{
    public Animator controller;
    public bool Open;

    void Start() {
        //controller= gameObject.transform.parent.GetComponent<Animator>();
        Open=false;
        
    }
    
    public override void Interact()
    {
        base.Interact();
        //Debug.Log(gameObject.transform.rotation.eulerAngles);
        if(Open){
            controller.SetTrigger("Cerrar");
            Open=false;
            controller.SetBool("Open",Open);
        }else{
            controller.SetTrigger("Abrir");
            Open=true;
            controller.SetBool("Open",Open);
        }
        //Debug.Log(gameObject.transform.rotation.eulerAngles);
        
        //
        
        //transform.Rotate(Vector3.up * 90);
    }
}