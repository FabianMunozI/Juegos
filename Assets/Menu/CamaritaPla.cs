using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CamaritaPla : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_PROCEDURALPLAYA);
        SceneManager.LoadScene("PlayaProcedural", LoadSceneMode.Additive);
    }

}