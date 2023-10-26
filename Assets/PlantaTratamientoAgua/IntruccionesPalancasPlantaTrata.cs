using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntruccionesPalancasPlantaTrata : Interactable
{
    public Animator controller;
    public GameObject bola;

    bool arriba;
    public int valor;
    GameObject msg; // faltan los planos

    ControllerPalancas controllerPalancas;
    public PlanoPlantaTrataInteractuable plano1;
    void Start() {
        controllerPalancas = this.transform.parent.parent.GetComponent<ControllerPalancas>();
        arriba=true;
        msg = GameObject.Find("CDebo").transform.GetChild(0).gameObject; //ContenedorMsgDeboHacerAlgoAntes

        /* if(PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=2){
                bola.GetComponent<MeshRenderer>().material.color=Color.green;
                controller.SetTrigger("completa");
                GetComponent<BoxCollider>().enabled=false;
            }
        } */
    }
    
    public override void Interact()
    {
        base.Interact();
        if (PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=1 && plano1.flag ){

                if(arriba){
                    controller.SetTrigger("abajo");
                }else{
                    controller.SetTrigger("arriba");
                }
                arriba=!arriba;
                dValor();
                controllerPalancas.ActualizarValoresPalancas();

                
            }else{
                msg.SetActive(true);
                Invoke("quitarMsg1seg",1f);
            }
                /* else{
                if(arriba){
                    controller.SetTrigger("abajo");
                }else{
                    controller.SetTrigger("arriba");
                }
                arriba=!arriba;
                dValor();
                controllerPalancas.ActualizarValoresPalancas();
            } */
        }else{
            Debug.Log("falta parte 1 mision fuera de la planta");
            /* if(arriba){
                controller.SetTrigger("abajo");
            }else{
                controller.SetTrigger("arriba");
            }
            arriba=!arriba;
            dValor();
            controllerPalancas.ActualizarValoresPalancas(); */
        }
    }


    void quitarMsg1seg(){
        msg.SetActive(false);
    }

    void dValor(){
        if(arriba){
            valor=0;
        }else{
            valor=1;
        }
    }
}
