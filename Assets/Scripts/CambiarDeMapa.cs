using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarDeMapa : Interactable
{

    public string nombreScenaCambiar;
    void Start() {
        
        
    }
    
    public override void Interact()
    {
        base.Interact();
        SceneManager.LoadScene(nombreScenaCambiar);
    }
}
