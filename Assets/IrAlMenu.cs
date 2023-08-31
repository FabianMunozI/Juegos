using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IrAlMenu : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject canvasIrMenu;
    GameObject player;

    bool menuAbierto;

    bool segundoMenu;
    void Start()
    {
        menuAbierto = false;
        player = GameObject.Find("Player");
        if(player ==null){
           Invoke("invokePlayer",3f);
           //Cursor.lockState = CursorLockMode.None;
           segundoMenu=true;
        }
        canvasIrMenu = GameObject.Find("CIrMenu").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !menuAbierto && segundoMenu){
            menuAbierto=true;
            canvasIrMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            ScriptsPlayer(false);
        }

        if(segundoMenu && menuAbierto && Input.GetMouseButtonDown(0)){
            irMenu();
            canvasIrMenu.SetActive(false);
            menuAbierto=false;
        }else if(segundoMenu && menuAbierto && Input.GetMouseButtonDown(1)){ // click derecho
            canvasIrMenu.SetActive(false);
            menuAbierto =false;
            Cursor.lockState = CursorLockMode.Locked;
            ScriptsPlayer(true);

        }
    }

    public void irMenu(){
        //GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled=false;;
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENU);
    }

    public void ScriptsPlayer(bool val){
        player.GetComponent<FpsCamera>().enabled = val;
    }

    public void libreMouse(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    void invokePlayer(){
        player = GameObject.Find("PlayerNandy(Clone)");
    }
}
