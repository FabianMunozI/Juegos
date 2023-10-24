using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TumultoTutorialScript : Interactable
{
    GameObject Jugador;
    public GameObject semillaPrefab;
    public GameObject plantaChicaPrefab;

    void Start()
    {
        Jugador = GameObject.Find("Player");
    }

    public override void Interact()
    {
        if (Jugador.GetComponent<PickUpObjects>().PickedObject != null)
        {
            if(Jugador.GetComponent<PickUpObjects>().PickedObject.name == string.Format("{0}(Clone)", semillaPrefab.name) ||
                Jugador.GetComponent<PickUpObjects>().PickedObject.name == string.Format("{0}", semillaPrefab.name))
            {  
                base.Interact();
                Instantiate(plantaChicaPrefab, transform.position + new Vector3(0,0.6f,0), Quaternion.identity);
                gameObject.SetActive(false);
            }
        }
    }

}
