using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pozo : Interactable
{
    public GameObject Jugador;
    public GameObject Balde_Agua;


    void Start()
    {
        Jugador = GameObject.Find("Player");
        Balde_Agua = GameObject.Find("Wooden_Bucket");
    }

    public override void Interact()
    {
        base.Interact();
        if(Jugador.GetComponent<PickUpObjects>().PickedObject == Balde_Agua)
        {
            if (! Balde_Agua.GetComponent<Balde_Comp>().tiene_agua)
            {   
                Balde_Agua.GetComponent<Outline>().enabled = false;
                this.GetComponent<Outline>().enabled = false;
                //base.Interact();
                Balde_Agua.GetComponent<Balde_Comp>().tiene_agua = true;
            }
        }
    }
}
