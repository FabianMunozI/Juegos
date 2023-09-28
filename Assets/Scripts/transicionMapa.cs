using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transicionMapa : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menuCambioDeMapa;
    public GameObject player;
    GameObject Iv;
    void Start()
    {
        menuCambioDeMapa = GameObject.Find("ContMapa").transform.GetChild(0).gameObject; //asegurarse de no cambiar el name de este prefab y dejarlo en la pos 0 del canvas
        player = GameObject.Find("Player");
        Iv = GameObject.Find("Inventario");
    }

    // Update is called once per frame
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

    public void LoadCiudad(){

        if(Equals(SceneManager.GetActiveScene().name, "Ciudad")){
            Salir();
        }else{
            Iv.GetComponent<Inventory>().SaveInventory();
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_CITY);
        }
        
    }

    public void LoadBosque(){
        if(Equals(SceneManager.GetActiveScene().name, "Bosque")){
            Salir();
        }else{
            //Iv.GetComponent<Inventory>().SaveInventory();
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_FOREST);
        }
        
    }

    public void LoadArtico(){
        if(Equals(SceneManager.GetActiveScene().name, "Artico")){
            Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_ARTIC);
        }
        
    }

    public void LoadArticoProcedural(){
        if(Equals(SceneManager.GetActiveScene().name, "Polo")){
            Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALARTICO);
        }
        
    }

    public void LoadProceduralFabian(){
        if(Equals(SceneManager.GetActiveScene().name, "FabProcedural")){
            Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALDESIERTO);
        }
        
    }

    public void LoadProceduralPlaya(){
        if(Equals(SceneManager.GetActiveScene().name, "PlayaProcedural")){
            Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALPLAYA);
        }
        
    }

    public void LoadDesierto(){
        if(Equals(SceneManager.GetActiveScene().name, "Desierto")){
            Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_DESERT);
        }
        
    }

    public void LoadPlaya(){
        if(Equals(SceneManager.GetActiveScene().name, "Playa")){
            Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_BEACH);
        }
        
        
    }

    public void LoadMenu(){
        if(Equals(SceneManager.GetActiveScene().name, "Menu")){
            Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENU);
        }
        
        
    }

    public void LoadMenuCiudad(){
        if(Equals(SceneManager.GetActiveScene().name, "MenuCiudad")){
            Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENUCIUDAD);
        }
        
        
    }

    public void LoadMenuBosque(){
        if(Equals(SceneManager.GetActiveScene().name, "MenuBosque")){
            Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENUBOSQUE);
        }
        
        
    }

    public void LoadIntento(){
        if(Equals(SceneManager.GetActiveScene().name, "intento")){
            Salir();
        }else{
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_INTENTO);
        }
        
        
    }

    
}
