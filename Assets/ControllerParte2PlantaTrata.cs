using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerParte2PlantaTrata : MonoBehaviour
{
    public ControllerPalancas puzzle1;

    public bool activarBotones;

    // Start is called before the first frame update


    public Transform posInicial;
    public GameObject referenciaLookAt;

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

    public GameObject[] objetosActivarAlFinal;
    void Start()
    {
        sePuedeJugar=false;
        perillasListas=false;
        botonesListos=false;

        planoPuesto=false;
        player=GameObject.Find("Player");
        activarBotones=false;

        /* PlayerPrefs.SetInt("PalancaPlantaTratamientoAgua", 3);
        PlayerPrefs.Save(); */

        if (PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            //Debug.Log(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua"));
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=3){
                //Debug.Log("entre");
                /* tuberia.SetTrigger("activar");
                tuberia2DentroMuros.SetTrigger("activar");
                botonRojo.GetComponent<BoxCollider>().enabled=false;
                botonVerde.GetComponent<BoxCollider>().enabled=false; */
                for(int i =0;i <6 ;i++){
                    botones[i].GetComponent<BoxCollider>().enabled=false;
                    perillas[i].GetComponent<BoxCollider>().enabled=false;
                }
                for(int i =0; i< 4;i++){
                    objetosActivarAlFinal[i].SetActive(true);
                }
                GameObject a= GameObject.Find("plano1MisionParte1");
                a.transform.GetChild(0).gameObject.SetActive(true);
                a.GetComponent<BoxCollider>().enabled=false;

                objetosActivarAlFinal[0].GetComponent<Animator>().SetTrigger("activar");
                objetosActivarAlFinal[0].GetComponent<AudioSource>().Play();
                objetosActivarAlFinal[3].GetComponent<Animator>().SetTrigger("activar");

                GameObject planee=transform.GetChild(3).gameObject;
                planee.GetComponent<BoxCollider>().enabled=false;
                planee.GetComponent<MeshRenderer>().enabled=false;


                GameObject plano = transform.GetChild(3).GetChild(0).gameObject;
                plano.SetActive(true);
                for(int i=0; i<6;i++){
                    plano.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);

                    if(i==0){

                        plano.transform.GetChild(i).transform.Rotate(Vector3.up, 270.0f);
                    }else if(i==1){
                        plano.transform.GetChild(i).GetChild(0).transform.Rotate(Vector3.up, 90f);
                    }
                    else if(i==2){
                        plano.transform.GetChild(i).GetChild(0).transform.Rotate(Vector3.up, 0.0f);
                    }
                    else if(i==3){
                        plano.transform.GetChild(i).GetChild(0).transform.Rotate(Vector3.up, 90.0f);
                    }
                    else if(i==4){
                        plano.transform.GetChild(i).GetChild(0).transform.Rotate(Vector3.up, 0.0f);
                    }
                    else if(i==5){
                        plano.transform.GetChild(i).GetChild(0).transform.Rotate(Vector3.up, 270.0f);
                    } 
                }
            }
        }
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

    void moverFinal(){
        player.transform.position = posFinal.position;
        player.transform.LookAt(referenciaLookAtFinal.transform);
        scriptsPlayer(true);
        PlayerPrefs.SetInt("PalancaPlantaTratamientoAgua", 3);
        PlayerPrefs.Save();
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
            // primero la animacion y luego las particulas  // sonidos?


            player.transform.position = posFinal.position;
            player.transform.LookAt(referenciaLookAtFinal.transform);
            
            Invoke("primeraParte",0.1f);
            Invoke("moverFinal",3.5f);

            objetosActivarAlFinal[0].GetComponent<Animator>().SetTrigger("activar");
            objetosActivarAlFinal[0].GetComponent<AudioSource>().Play();
            objetosActivarAlFinal[1].SetActive(true);
            objetosActivarAlFinal[2].SetActive(true);
            objetosActivarAlFinal[3].GetComponent<Animator>().SetTrigger("activar");


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
