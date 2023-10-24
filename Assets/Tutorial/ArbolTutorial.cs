using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArbolTutorial : Interactable
{
    GameObject Jugador;
    public GameObject palaPrefab;

    public GameObject tumultoPrefab;
    

    void Start()
    {
        Jugador = GameObject.Find("Player");
    }

    public override void Interact()
    {
        if (Jugador.GetComponent<PickUpObjects>().PickedObject != null)
        {
            if(Jugador.GetComponent<PickUpObjects>().PickedObject.name == string.Format("{0}(Clone)", palaPrefab.name) ||
               Jugador.GetComponent<PickUpObjects>().PickedObject.name == string.Format("{0}", palaPrefab.name))
            {  
                base.Interact();
                Instantiate(tumultoPrefab, transform.position - new Vector3(0,1.5f,0), Quaternion.identity);
                gameObject.SetActive(false);
            }
        }
    }

}
