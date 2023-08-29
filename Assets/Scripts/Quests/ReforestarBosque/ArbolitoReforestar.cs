using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArbolitoReforestar : Interactable
{
    GameObject Jugador;
    public GameObject regaderaPrefab;
    public bool to_remove = false; 
    public int indice;

    void Start()
    {
        Jugador = GameObject.Find("Player");
    }

    void Update()
    {

    }

    public override void Interact()
    {
        if (Jugador.GetComponent<PickUpObjects>().PickedObject != null)
        {
            if(Jugador.GetComponent<PickUpObjects>().PickedObject.name == string.Format("{0}(Clone)", regaderaPrefab.name))
            {  
                base.Interact();
                to_remove = true;
            }
        }
    }

}
