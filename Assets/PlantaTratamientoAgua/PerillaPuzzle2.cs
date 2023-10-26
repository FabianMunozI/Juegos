using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PerillaPuzzle2 : Interactable
{
    public Animator controller;

    GameObject msg; // faltan los planos
    public bool botonActivar;
    public ControllerParte2PlantaTrata controllerPuzzle2;

    public int valorPerilla;
    public GameObject referenciaPlano;
    void Start() {
        valorPerilla=0;
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
                referenciaPlano.transform.Rotate(Vector3.up, 90.0f);
                valorPerilla+=1;
                if(valorPerilla==4){
                    valorPerilla=0;
                }
                controllerPuzzle2.chequearPerillas();

                //sonido?

                /* controllerPalancas.presionado(gameObject); */
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
