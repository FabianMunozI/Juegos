using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : Interactable
{
    public Animator controller;
    public bool Open;
    BoxCollider collider;

    void Start() {
        //controller= gameObject.transform.parent.GetComponent<Animator>();
        Open=false;
        //controller.SetBool("Open",false);
        collider=this.GetComponent<BoxCollider>();
        collider.isTrigger= false;
    }
    
    public override void Interact()
    {
        base.Interact();
        //Debug.Log(gameObject.transform.rotation.eulerAngles);
        if(Open){
            collider.isTrigger= true;
            controller.SetTrigger("Cerrar");
            Open=false; 
            Invoke("istriggereadoFalso",0.5f);
        }else{
            collider.isTrigger= true;
            controller.SetTrigger("Abrir");
            Open=true;
            Invoke("istriggereadoFalso",0.5f);
        }
        //Debug.Log(gameObject.transform.rotation.eulerAngles);
        
        //
        
        //transform.Rotate(Vector3.up * 90);
    }

    void istriggereadoFalso(){
        collider.isTrigger= false;
    }
}