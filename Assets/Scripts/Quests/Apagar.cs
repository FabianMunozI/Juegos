using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Apagar : Interactable
{
    public bool live;
    public GameObject Jugador;
    public GameObject Balde_Agua;
    
    void Start() {
        live = true;
        Jugador = GameObject.Find("Player");
        Balde_Agua = GameObject.Find("Wooden_Bucket");
    }

    public override void Interact()
    {
        if(Jugador.GetComponent<PickUpObjects>().PickedObject == Balde_Agua)
        {
            if (Balde_Agua.GetComponent<Balde_Comp>().tiene_agua)
            {   
                base.Interact();
            
                if(live){
                    this.gameObject.SetActive(false);
                    live = false;
                }
                Balde_Agua.GetComponent<Balde_Comp>().tiene_agua = false;
            }
        }
        

    }
}

