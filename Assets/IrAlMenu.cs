using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrAlMenu : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject canvasIrMenu;
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        canvasIrMenu = GameObject.Find("CIrMenu").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            canvasIrMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            ScriptsPlayer(false);
        }
    }

    public void irMenu(){
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENU);
    }

    public void ScriptsPlayer(bool val){
        player.GetComponent<FpsCamera>().enabled = val;
    }

    public void libreMouse(){
        Cursor.lockState = CursorLockMode.Locked;
    }
}
