using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable_Outliner : Interactable//, IPunObservable
{
    public GameObject Jugador;

    public string tipoBasura = null;

    public void Start(){
        Jugador = GameObject.Find("Player");
        //Invoke("Usar",2f);
    }
    public override void Interact()
    {
        base.Interact();
        if( Jugador.GetComponent<PickUpObjects>().PickedObject == null){
            Jugador.GetComponent<PickUpObjects>().PickedObject = this.gameObject;
            this.transform.SetParent(Jugador.transform.GetChild(1));
            this.transform.position = Jugador.transform.GetChild(1).position;

            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Collider>().isTrigger = true;
            this.GetComponent<Outline>().enabled = false;

        }else if (Jugador.GetComponent<PickUpObjects>().PickedObject != null){
            this.transform.SetParent(null);
            this.GetComponent<Rigidbody>().useGravity = true;

            this.GetComponent<Rigidbody>().isKinematic = false;
            Jugador.GetComponent<PickUpObjects>().PickedObject = null;
            this.GetComponent<Collider>().isTrigger = false;
            
        }
        
    }
}

