using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class botonTipoPlantaTrataParte1 : Interactable
{
    public Animator controller;

    GameObject msg; // faltan los planos
    public bool botonActivar;
    public ControllerPalancas controllerPalancas;
    void Start() {
        //controllerPalancas = this.transform.parent.parent.GetComponent<ControllerPalancas>();
        msg = GameObject.Find("CDebo").transform.GetChild(0).gameObject; //ContenedorMsgDeboHacerAlgoAntes
        /* if(PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=2){
                bola.GetComponent<MeshRenderer>().material.color=Color.green;
                controller.SetTrigger("completa");
                GetComponent<BoxCollider>().enabled=false;
            }
        } */
    }
    
    /* private void OnEnable() {
        controllerPalancas.primeroActivado=null;
        controllerPalancas.segundoActivado = null;
        controllerPalancas.terceroActivado = null;
    } */

    public override void Interact()
    {
        base.Interact();
        if (PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            //Debug.Log(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua"));
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=1 && controllerPalancas.activarBotones){
                controller.SetTrigger("activar");
                controllerPalancas.presionado(gameObject);
                //emitir sonido?
                
            }else{
                msg.SetActive(true);
                Invoke("quitarMsg1seg",1f);

            }
        }else{
            Debug.Log("falta la parte 1 de la mision fuera de la planta");
            msg.SetActive(true);
            Invoke("quitarMsg1seg",1f);
        }
    }


    void quitarMsg1seg(){
        msg.SetActive(false);
    }
}
