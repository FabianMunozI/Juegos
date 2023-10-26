using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalirMisionEolica : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Salir(){
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_DESERT);
    }
}
