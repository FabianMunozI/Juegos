using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TepearMision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_TP_EOLICA);
        }
    }
}
