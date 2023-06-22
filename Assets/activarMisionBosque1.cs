using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activarMisionBosque1 : Interactable
{
    public GameObject Jugador;

    Incendio scriptIncendio;

    public bool misionActivada=false;


    void Start()
    {
        scriptIncendio = GetComponent<Incendio>();
        Jugador = GameObject.Find("Player");

        //Balde_Agua = GameObject.Find("Wooden_Bucket");
    }

    public override void Interact() // este script lo tiene la fogata
    {
        base.Interact();

        if(misionActivada==false){
            misionActivada=true;
            gameObject.layer = 0;
            
        }
        

    }
}
