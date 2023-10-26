using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scriptsPlayerEnable : MonoBehaviour
{
    public GameObject player;
    GameObject respaldo;

    public MisionPersePlantaTrata scriptMision;
    public GameObject textos;

    public GameObject parte1;
    public GameObject parte2;

    public GameObject dialogo;

    void Start(){
        
        Invoke("buscarNpcMision",3f);
        //Equals(SceneManager.GetActiveScene().name, "PlantaTratamiento");

    }



    public void scriptsPlayer(bool nuevoV){
        player.GetComponent<Mascota>().enabled=nuevoV;
        player.GetComponent<CameraInteraction>().enabled=nuevoV;
        player.GetComponent<FpsCamera>().enabled=nuevoV;
        player.GetComponent<CharacterMovement>().enabled=nuevoV;
        

        if(nuevoV){
            
            Cursor.lockState= CursorLockMode.Locked;
        }

        if(!nuevoV){
            Cursor.lockState= CursorLockMode.None;
        }
        player.GetComponent<AbrirMapa>().enabled=nuevoV;
        player.GetComponent<Rigidbody>().isKinematic= !nuevoV;


    }

    void buscarNpcMision(){
        
        player=GameObject.Find("Player");
        scriptMision = GameObject.Find("Elmejorpjdelavidamun").GetComponent<MisionPersePlantaTrata>();
        dialogo = gameObject.transform.GetChild(4).gameObject;
        //Debug.Log("Dialogo");
        //Debug.Log(dialogo);
        scriptMision.gameObject.transform.parent.GetChild(1).GetComponent<ActivarDialogo>().dialogoObjetivo = dialogo;

        textos = GameObject.Find("CanvasMisionesMTratar"); // hijos son los canvas perse

        parte1= textos.transform.GetChild(2).gameObject;  // palancas
        parte2= textos.transform.GetChild(3).gameObject;  // perillas

        if(Equals(SceneManager.GetActiveScene().name, "PlantaTratamientoAgua")){
            //Debug.Log("si my broda");
            if(PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
                if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=3){
                    textos.transform.GetChild(4).gameObject.SetActive(true);
                }
                else if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=2){
                    textos.transform.GetChild(3).gameObject.SetActive(true); 
                }
                else if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=1){
                    textos.transform.GetChild(2).gameObject.SetActive(true);
                }else{
                    Debug.Log("Who knows");
                }
            }
        }
    }

    public void llamarAInteract(){
        scriptMision.Interact();
    }

    public void PrimeraVez(){
        scriptMision.PrimerVez();
    }
}
