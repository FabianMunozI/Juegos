using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TP_MisionPlanta : Interactable
{
    public int tipo;
    bool habilitarPuerta;
    GameObject msg;
    // Start is called before the first frame update
    void Start()
    {
        habilitarPuerta=false;
        msg=GameObject.Find("ContenedorMsgPuertaTP").transform.GetChild(0).gameObject;
    }

    public override void Interact()
    {
        base.Interact();
        if (PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=1){
                habilitarPuerta=true;
            }else{
                msg.SetActive(true);
                Invoke("quitarMsg1seg",1f);
            }
        }else{
            msg.SetActive(true);
            Invoke("quitarMsg1seg",1f);
        }



        if(tipo==0 && habilitarPuerta){
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MISIONPLANTA);
        }else if(tipo == 1 && habilitarPuerta){
            //Debug.Log("entre aqui3");
            PlayerPrefs.SetInt("playaProceduralPos", 1);
            PlayerPrefs.Save();
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALPLAYA);
        }
        
    }

    void quitarMsg1seg(){
        msg.SetActive(false);
    }
}
