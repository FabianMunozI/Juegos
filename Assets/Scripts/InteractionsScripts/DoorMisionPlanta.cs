using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorMisionPlanta : Interactable
{
    public Animator controller;
    public bool Open;
    BoxCollider collider;
    GameObject player;
    bool flag;

    GameObject msg;

    public GameObject candado;
    public GameObject llave;
    void Start() {
        msg = GameObject.Find("ContenedorMsgPuertaTratamientoAgua").transform.GetChild(0).gameObject;
        flag=false;
        player = GameObject.Find("Player");
        //controller= gameObject.transform.parent.GetComponent<Animator>();
        Open=false;
        //controller.SetBool("Open",false);
        collider=this.GetComponent<BoxCollider>();
        collider.isTrigger= false;

        if (PlayerPrefs.HasKey("terminoJuegoElectricidad1PlantaTratamientoAgua")){
            if(PlayerPrefs.GetInt("terminoJuegoElectricidad1PlantaTratamientoAgua")==1){ // ya hizo la mision 1 vez
                flag=true;
                Destroy(candado);
                Destroy(llave);
            }
        }
    }
    
    public override void Interact()
    {
        base.Interact();
        if(player.GetComponent<PickUpObjects>().PickedObject!=null && !flag){
            //Debug.Log("1");
            if(player.GetComponent<PickUpObjects>().PickedObject.GetComponent<PickableMascota>().tipo==1){
                //Debug.Log("2");
                Destroy(player.GetComponent<PickUpObjects>().PickedObject);
                player.GetComponent<PickUpObjects>().PickedObject=null;
                //eliminar objeto tomado // desbloquear puerta
                Destroy(candado);
                flag=true;
            }else{
                //Debug.Log("3");
                //Debug.Log("Necesitas la llave");
                msg.SetActive(true);
                Invoke("quitarMsg1seg",0.75f);
                //mostrar por pantalla que necesitas la llave
            }
        }else if(player.GetComponent<PickUpObjects>().PickedObject==null && !flag){
            
            //Debug.Log("Necesitas la llave");
            msg.SetActive(true);
            Invoke("quitarMsg1seg",0.75f);
        }
        // si el player tiene un objeto en la mano que sea la llave, se rompe el candado y puedes abrir la puerta, sino desplegar panel/texto
        //que diga que no puedes abrir la wea 
        //Debug.Log(gameObject.transform.rotation.eulerAngles);
        if(flag){
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
        }
        
        //Debug.Log(gameObject.transform.rotation.eulerAngles);
        
        //
        
        //transform.Rotate(Vector3.up * 90);
    }

    void istriggereadoFalso(){
        collider.isTrigger= false;
    }

    void quitarMsg1seg(){
        msg.SetActive(false);
    }
}