using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controladorJuego : MonoBehaviour
{
    public GameObject[] izquierdos;
    public GameObject[] derechos;
    public GameObject[] cables;
    public AudioSource[] audios;

    int tipoIzq;
    int tipoDer;

    int contador=0;

    public GameObject luzPalanca; //activar la luz
    // Start is called before the first frame update
    void Start()
    {
        tipoIzq=-1;
        tipoDer=-1;
        if (PlayerPrefs.HasKey("terminoJuegoElectricidad1PlantaTratamientoAgua")){
            if(PlayerPrefs.GetInt("terminoJuegoElectricidad1PlantaTratamientoAgua")==1){ // ya hizo la mision 1 vez
                for(int i = 0; i<4; i++){
                    cables[i].SetActive(true);
                    derechos[i].GetComponent<Outline>().enabled=false;
                    izquierdos[i].GetComponent<Outline>().enabled=false;

                    derechos[i].GetComponent<cableInteraction>().seleccionado=false;
                    izquierdos[i].GetComponent<cableInteraction>().seleccionado=false;

                    derechos[i].GetComponent<CapsuleCollider>().enabled=false;
                    izquierdos[i].GetComponent<CapsuleCollider>().enabled=false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void seleccionarIzq(int tipo, bool seleccionado){
        for(int i =0;i<4;i++){
            if(tipo == i && !seleccionado){
                izquierdos[i].GetComponent<Outline>().enabled=true;
                tipoIzq=i;
            }else if(tipo== i && seleccionado){
                izquierdos[i].GetComponent<Outline>().enabled=false;
                tipoIzq=-1;
            }else{
                izquierdos[i].GetComponent<Outline>().enabled=false;
            }
        }
    }

    public void seleccionarDer(int tipo, bool seleccionado){
        for(int i =0;i<4;i++){
            if(tipo == i && !seleccionado){
                derechos[i].GetComponent<Outline>().enabled=true;
                tipoDer=i;
            }else if(tipo== i && seleccionado){
                derechos[i].GetComponent<Outline>().enabled=false;
                tipoDer=-1;
            }else{
                derechos[i].GetComponent<Outline>().enabled=false;
            }
        }
    }

    public void ambosIgualesIzqDer(){
        if(tipoDer !=-1 && tipoIzq != -1 ){ // ambos estan seleccionados
            if(tipoIzq==tipoDer){
                cables[tipoIzq].SetActive(true);
                derechos[tipoDer].GetComponent<Outline>().enabled=false;
                izquierdos[tipoIzq].GetComponent<Outline>().enabled=false;

                derechos[tipoDer].GetComponent<CapsuleCollider>().enabled=false;
                izquierdos[tipoIzq].GetComponent<CapsuleCollider>().enabled=false;

                tipoDer=-1;
                tipoIzq=-1;

                audios[0].Play();
                contador+=1;
                if(contador==4){ // activar luz palanca
                    luzPalanca.GetComponent<MeshRenderer>().material.color=Color.yellow;
                    PlayerPrefs.SetInt("terminoJuegoElectricidad1PlantaTratamientoAgua", 1);
                    PlayerPrefs.Save();
                }
                //sonido agradable electricidad
            }else{
                contador=0;
                tipoDer=-1;
                tipoIzq=-1;
                //desactivar todos 
                audios[1].Play();
                //sonido desagradable electricidad / cortocircuito
                for(int i = 0; i<4; i++){
                    cables[i].SetActive(false);
                    derechos[i].GetComponent<Outline>().enabled=false;
                    izquierdos[i].GetComponent<Outline>().enabled=false;

                    derechos[i].GetComponent<cableInteraction>().seleccionado=false;
                    izquierdos[i].GetComponent<cableInteraction>().seleccionado=false;

                    derechos[i].GetComponent<CapsuleCollider>().enabled=true;
                    izquierdos[i].GetComponent<CapsuleCollider>().enabled=true;
                }
                
            }
        }
    }
}
