using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class botonTipo2 : Interactable
{
    public Animator controller;

    GameObject msg; // faltan los planos
    public bool botonActivar;
    public ControllerParte2PlantaTrata controllerPuzzle2;

    public GameObject tapadura;
    public int inicialTapadura; // 0 para apagado 1 para encendido
    void Start() {
        tapadura= tapadura.transform.GetChild(0).gameObject;
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
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=2 && controllerPuzzle2.planoPuesto && controllerPuzzle2.sePuedeJugar){
                controller.SetTrigger("activar");

                //sonido??

                /* controllerPuzzle2.presionado(gameObject); */
                //emitir sonido?
                if(inicialTapadura==0){
                    inicialTapadura=1;
                    tapadura.SetActive(true);
                }else{
                    inicialTapadura=0;
                    tapadura.SetActive(false);
                }
                controllerPuzzle2.chequearBotones();

                //
                
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