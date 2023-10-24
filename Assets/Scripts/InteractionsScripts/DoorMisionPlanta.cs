using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorMisionPlanta : Interactable
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

        // si el player tiene un objeto en la mano que sea la llave, se rompe el candado y puedes abrir la puerta, sino desplegar panel/texto
        //que diga que no puedes abrir la wea 
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