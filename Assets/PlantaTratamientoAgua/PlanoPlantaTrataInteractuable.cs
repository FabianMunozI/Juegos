using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanoPlantaTrataInteractuable : Interactable
{
    public bool Open;
    BoxCollider colaider;
    GameObject player;
    public bool flag;

    GameObject msg;
    GameObject msg2; // esto no va aqui...
    GameObject plano;

    void Start() {
        plano = transform.GetChild(0).gameObject;
        msg = GameObject.Find("CPareceQueaquiVaAlgo").transform.GetChild(0).gameObject;
        msg2 = GameObject.Find("CEstoNoEncajaAqui").transform.GetChild(0).gameObject;
        flag=false;
        player = GameObject.Find("Player");
        //controller= gameObject.transform.parent.GetComponent<Animator>();
        Open=false;
        //controller.SetBool("Open",false);
        colaider=this.GetComponent<BoxCollider>();
        colaider.isTrigger= false;

        /* if (PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=3){ // ya hizo la mision 1 vez
                flag=true;
            }
        } */
    }
    
    public override void Interact()
    {
        base.Interact();
        if(player.GetComponent<PickUpObjects>().PickedObject!=null /* && flag */){
            //Debug.Log("1");
            if(player.GetComponent<PickUpObjects>().PickedObject.GetComponent<PickableMascota>().tipo==2){
                //Debug.Log("2");
                Destroy(player.GetComponent<PickUpObjects>().PickedObject);
                player.GetComponent<PickUpObjects>().PickedObject=null;
                //eliminar objeto tomado // desbloquear puerta

                //ACTIVAR GAMEOBJECT INSTRUCCIONES PARED , desactivar este trigger
                plano.SetActive(true);
                GetComponent<BoxCollider>().enabled=false;

                flag=true;
            }else if(player.GetComponent<PickUpObjects>().PickedObject.GetComponent<PickableMascota>().tipo==3){
                msg2.SetActive(true);
                Invoke("quitarMsg2seg",1f);
            }else{
                //Debug.Log("3");
                //Debug.Log("Necesitas la llave");
                msg.SetActive(true);
                Invoke("quitarMsg1seg",1f);
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
            istriggereadoFalso();
        }
        
        //Debug.Log(gameObject.transform.rotation.eulerAngles);
        
        //
        
        //transform.Rotate(Vector3.up * 90);
    }

    void istriggereadoFalso(){
        colaider.isTrigger= false;
    }

    void quitarMsg1seg(){
        msg.SetActive(false);
    }

    void quitarMsg2seg(){
        msg2.SetActive(false);
    }
}
