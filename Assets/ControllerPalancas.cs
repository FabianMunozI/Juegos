using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPalancas : MonoBehaviour
{
    public ControllerParte2PlantaTrata puzzle2;
    public IntruccionesPalancasPlantaTrata[] palancas;
    public bool activarBotones;

    public bool segundoPuzzle;
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

    public Transform posMedioAnterior;
    public GameObject referenciaLookAtMedioAnterior;

    public Transform posFinal;
    public GameObject referenciaLookAtFinal;

    public Animator tuberia;
    public Animator tuberia2DentroMuros;

    GameObject respaldo;

    GameObject player;
    void Start()
    {
        player=GameObject.Find("Player");
        activarBotones=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActualizarValoresPalancas(){
        bool temp = false;

        if(palancas[0].valor==0){
            if(palancas[1].valor==1){
                if(palancas[2].valor==0){
                    if(palancas[3].valor==0){
                        //Debug.Log("ActivarBotones");
                        activarBotones=true;
                        temp=true;
                    }
                }
            }
        }
        if(!temp){
            activarBotones=false;
        }
        
    }

    public void FactivarBotones(bool botonesActivar){
        
        botonRojo.SetActive(botonesActivar);
        botonVerde.SetActive(botonesActivar);
        
    }

    public void verificarBotones(){
        if(primeroActivado!= null && segundoActivado!= null && terceroActivado != null){
            if(primeroActivado == botonRojo && segundoActivado == botonVerde && terceroActivado == botonRojo){
                //Debug.Log("Botones cumplidos");
                player.transform.position = posFinal.position;
                player.transform.LookAt(referenciaLookAtFinal.transform);
                
                Invoke("primeraParte",0.1f);
                for(int i=0; i<4;i++){
                    palancas[i].gameObject.GetComponent<BoxCollider>().enabled=false;
                }
                botonRojo.GetComponent<BoxCollider>().enabled=false;
                botonVerde.GetComponent<BoxCollider>().enabled=false;
                tuberia.SetTrigger("activar");
                tuberia.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                // iniciar animacion giro cañeria y elevacion del agua
                Invoke("moverMedio",3.5f);
                Invoke("moverMedioAnterior",7f);
                Invoke("moverFinal",8f);
                //Debug.Log("Se acabo");
                // cambiar flujo de agua hacia adentro, subir plano de agua del pretratamiento, animacion, desactivar palancas y botones
            }
        }
        
    }

    public void presionado(GameObject boton){
        if(primeroActivado == null){
            primeroActivado = boton;
        }else if( segundoActivado == null){
            segundoActivado = boton;
        }else if( terceroActivado == null){
            terceroActivado = boton;
        }else{
            primeroActivado = segundoActivado;
            segundoActivado = terceroActivado;
            terceroActivado = boton;
        }
        verificarBotones();
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

    void moverMedioAnterior(){
        //tuberia2DentroMuros.SetTrigger("activar");
        //tuberia2DentroMuros.transform.parent.GetChild(1).GetChild(2).gameObject.SetActive(true);
        player.transform.position = posMedioAnterior.position;
        player.transform.LookAt(referenciaLookAtMedioAnterior.transform);
    }

    void moverFinal(){
        player.transform.position = posFinal.position;
        player.transform.LookAt(referenciaLookAtFinal.transform);
        scriptsPlayer(true);

        PlayerPrefs.SetInt("PalancaPlantaTratamientoAgua", 2);
        PlayerPrefs.Save();
        /* Debug.Log("Variable PalancaPlantaTratamientoAgua = ");
        Debug.Log(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")); */
        puzzle2.HabilitarPuzzle();
    }

    void primeraParte(){
        player.transform.position = posInicial.position;
        scriptsPlayer(false);
        player.transform.LookAt(referenciaLookAt.transform);
        segundoPuzzle=true;
    }
}
