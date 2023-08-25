using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarDeMapa : Interactable
{
    public GameObject menuCambioDeMapa;
    public GameObject player;


    //public string nombreScenaCambiar;
    void Start() {
        player = GameObject.Find("Player");
        menuCambioDeMapa = GameObject.Find("ContMapa").transform.GetChild(0).gameObject; //asegurarse de no cambiar el name de este prefab y dejarlo en la pos 0 del canvas
        //menuCambioDeMapa.SetActive(false);
        
    }
    
    public override void Interact()
    {
        base.Interact();
        menuCambioDeMapa.SetActive(true);
        player.GetComponent<CharacterMovement>().enabled = false;
        player.GetComponent<FpsCamera>().enabled = false;
        if(player.GetComponent<AbrirMapa>().abrirMapaGrande == false){
            player.GetComponent<AbrirMapa>().mapaChico.SetActive(false);
        }
        player.GetComponent<AbrirMapa>().enabled=false;
        Cursor.lockState = CursorLockMode.Confined;
        
        //SceneManager.LoadScene(nombreScenaCambiar);
    }
}
