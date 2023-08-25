using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activarMisionRitmo : Interactable
{
    // Start is called before the first frame update
    public GameObject MisionRitmo;
    void Start()
    {
        
    }

    public override void Interact(){ // preguntar mision
        base.Interact();

        MisionRitmo.SetActive(true);

    }

    

}
