using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArbolReforestar : Interactable
{
    GameObject Jugador;
    public GameObject palaPrefab;
    public bool to_remove = false; 
    

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
            if(Jugador.GetComponent<PickUpObjects>().PickedObject.name == string.Format("{0}(Clone)", palaPrefab.name))
            {  
                base.Interact();
                to_remove = true;
            }
        }
    }

}
