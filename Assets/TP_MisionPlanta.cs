using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TP_MisionPlanta : Interactable
{
    public int tipo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Interact()
    {
        base.Interact();
        if(tipo==0){
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MISIONPLANTA);
        }else if(tipo ==1){
            Debug.Log("entre aqui3");
            PlayerPrefs.SetInt("playaProceduralPos", 1);
            PlayerPrefs.Save();
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_TP_PLANTA);
        }
        
    }
}
