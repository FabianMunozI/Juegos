using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cambioMapaPlantaTemporal : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    GameObject menuCambioDeMapa;
    void Start()
    {
        player = GameObject.Find("Player");
        menuCambioDeMapa = GameObject.Find("ContMapa").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTpPlantaProcedural(){
        if(Equals(SceneManager.GetActiveScene().name, "TP_PlantaTratamiento")){
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_TP_PLANTA);
            //Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_TP_PLANTA);
        }
        
    }

    public void Salir(){
        menuCambioDeMapa.SetActive(false);
        player.GetComponent<CharacterMovement>().enabled = true;
        player.GetComponent<FpsCamera>().enabled = true;
        if(player.GetComponent<AbrirMapa>().abrirMapaGrande == false){
            player.GetComponent<AbrirMapa>().mapaChico.SetActive(true);
        }
        player.GetComponent<AbrirMapa>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

    }
}
