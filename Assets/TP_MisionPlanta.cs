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
            GameObject pl = GameObject.Find("Player");
            PlayerPrefs.SetFloat("playaProceduralPosx", pl.transform.position.x);
            PlayerPrefs.SetFloat("playaProceduralPosy", pl.transform.position.y);
            PlayerPrefs.SetFloat("playaProceduralPosz", pl.transform.position.z);
            PlayerPrefs.Save();
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MISIONPLANTA);

        }else if(tipo ==1){
            Debug.Log("entre aqui3");
            PlayerPrefs.SetInt("playaProceduralVolver", 1); // Para posicionar al jugador solo cuando sale del 
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALPLAYA);
        }
        
    }
}
