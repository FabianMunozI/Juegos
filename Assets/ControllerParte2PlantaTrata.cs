using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerParte2PlantaTrata : MonoBehaviour
{
    public ControllerPalancas puzzle1;

    public IntruccionesPalancasPlantaTrata[] palancas;
    public bool activarBotones;

    // Start is called before the first frame update

    public GameObject botonRojo;
    public GameObject botonVerde;

    public GameObject terceroActivado;
    public GameObject segundoActivado;
    public GameObject primeroActivado;

    public Transform posInicial;
    public GameObject referenciaLookAt;

    public Transform posMedio;
    public GameObject referenciaLookAtMedio;

    public Transform posFinal;
    public GameObject referenciaLookAtFinal;

    public Animator tuberia;
    public Animator tuberia2DentroMuros;

    GameObject respaldo;

    public bool planoPuesto;

    GameObject player;

    public bool perillasListas;
    public bool botonesListos;

    public PerillaPuzzle2[] perillas;

    public botonTipo2[] botones;

    public bool sePuedeJugar;
    void Start()
    {
        sePuedeJugar=false;
        perillasListas=false;
        botonesListos=false;

        planoPuesto=false;
        player=GameObject.Find("Player");
        activarBotones=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void scriptsPlayer(bool nuevoV){
        player.GetComponent<Mascota>().enabled=nuevoV;
        player.GetComponent<CameraInteraction>().enabled=nuevoV;
        player.GetComponent<FpsCamera>().enabled=nuevoV;
        player.GetComponent<CharacterMovement>().enabled=nuevoV;
        if(!player.GetComponent<AbrirMapa>().abrirMapaGrande && !nuevoV){
            respaldo=GameObject.Find("MinimapContainer");
            respaldo.SetActive(false);

        }

        if(nuevoV){
            respaldo.SetActive(true);
        }
        player.GetComponent<AbrirMapa>().enabled=nuevoV;
        player.GetComponent<Rigidbody>().isKinematic= !nuevoV;


    }
    void moverMedio(){
        tuberia2DentroMuros.SetTrigger("activar");
        tuberia2DentroMuros.transform.parent.GetChild(1).GetChild(2).gameObject.SetActive(true);
        player.transform.position = posMedio.position;
        player.transform.LookAt(referenciaLookAtMedio.transform);
    }

    void moverFinal(){
        player.transform.position = posFinal.position;
        player.transform.LookAt(referenciaLookAtFinal.transform);
        scriptsPlayer(true);
    }

    void primeraParte(){
        player.transform.position = posInicial.position;
        scriptsPlayer(false);
        player.transform.LookAt(referenciaLookAt.transform);
        /* segundoPuzzle=true; */
    }

    public void HabilitarPuzzle(){
        sePuedeJugar=true;
    }

    public void chequearBotonListoPerillaLista(){
        if(perillasListas && botonesListos){
            Debug.Log("objetivoAlcanzado");

            //cambiar de valor variable para que al salir de la planta te muestre la tuberia con agua limpia
        }
    }

    public void chequearPerillas(){
        //Debug.Log("a");
        bool temp=false;
        if(perillas[0].valorPerilla==3){
            if(perillas[1].valorPerilla==1){
                if(perillas[2].valorPerilla==0){
                    if(perillas[3].valorPerilla==1){
                        if(perillas[4].valorPerilla==0){
                            if(perillas[5].valorPerilla==3){
                                perillasListas=true;
                                chequearBotonListoPerillaLista();
                                temp=true;
                            }
                        }
                    }
                }
            }
        } 

        if(!temp){
            perillasListas=false;
        }

    }

    public void chequearBotones(){
        bool temp=false;
        //Debug.Log("b");
        if(botones[0].inicialTapadura==0){
            if(botones[1].inicialTapadura==0){
                if(botones[2].inicialTapadura==0){
                    if(botones[3].inicialTapadura==0){
                        if(botones[4].inicialTapadura==0){
                            if(botones[5].inicialTapadura==0){
                                botonesListos=true;
                                temp=true;
                                chequearBotonListoPerillaLista();
                            }
                        }
                    }
                }
            }
        } 

        if(!temp){
            botonesListos=false;
        }
    

    }
}
