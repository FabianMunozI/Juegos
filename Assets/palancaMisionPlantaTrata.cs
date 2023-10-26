using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class palancaMisionPlantaTrata : Interactable
{
    public Animator controller;
    public GameObject bola;

    GameObject msg;
    public controladorJuego controladorJuego;
    void Start() {
        msg = GameObject.Find("ContenedorMsgPuertaTratamientoAgua").transform.GetChild(0).gameObject;
        if (PlayerPrefs.HasKey("terminoJuegoElectricidad1PlantaTratamientoAgua")){
            if(PlayerPrefs.GetInt("terminoJuegoElectricidad1PlantaTratamientoAgua")>=1){
                bola.GetComponent<MeshRenderer>().material.color=Color.yellow;
                //controller.SetTrigger("completa");
            }
        }

        if(PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=1){
                bola.GetComponent<MeshRenderer>().material.color=Color.green;
                controller.SetTrigger("completa");
                GetComponent<BoxCollider>().enabled=false;
            }
        }
    }
    
    public override void Interact()
    {
        base.Interact();
        if (PlayerPrefs.HasKey("terminoJuegoElectricidad1PlantaTratamientoAgua")){
            if(PlayerPrefs.GetInt("terminoJuegoElectricidad1PlantaTratamientoAgua")>=1){
                controller.SetTrigger("completa");
                bola.GetComponent<MeshRenderer>().material.color=Color.green;
                GetComponent<BoxCollider>().enabled=false;
                controladorJuego.audios[2].Play();

                PlayerPrefs.SetInt("PalancaPlantaTratamientoAgua", 1);
                PlayerPrefs.Save();

                GameObject a = GameObject.Find("Elmejorpjdelavidamun");
                a.GetComponent<MisionPersePlantaTrata>().textos.transform.GetChild(1).gameObject.SetActive(false);
                a.GetComponent<MisionPersePlantaTrata>().textos.transform.GetChild(2).gameObject.SetActive(true);
            }
        }else{
            //animacion se devuelve incompleta
            controller.SetTrigger("incompleta");
        }

       


        
    }

   

    void quitarMsg1seg(){
        msg.SetActive(false);
    }
}
