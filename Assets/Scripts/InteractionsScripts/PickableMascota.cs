using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableMascota : Interactable//, IPunObservable
{
    GameObject Jugador;
    GameObject mascota;

    public void Start(){
        Jugador = GameObject.Find("Player");
        mascota = GameObject.Find("CMascota").transform.GetChild(0).gameObject;
        //Invoke("Usar",2f);
    }
    public override void Interact()
    {
        base.Interact();
        if(mascota.GetComponent<mascotaInteraction>().mascotita){
            if( Jugador.GetComponent<PickUpObjects>().PickedObjectMascota == null){
                Jugador.GetComponent<PickUpObjects>().PickedObjectMascota = this.gameObject;
                this.transform.SetParent(mascota.transform.GetChild(1));
                this.transform.position = mascota.transform.GetChild(1).position;

                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponent<Collider>().isTrigger = true;

            }else if (Jugador.GetComponent<PickUpObjects>().PickedObjectMascota != null){
                this.transform.SetParent(null);
                this.GetComponent<Rigidbody>().useGravity = true;

                this.GetComponent<Rigidbody>().isKinematic = false;
                Jugador.GetComponent<PickUpObjects>().PickedObjectMascota = null;
                this.GetComponent<Collider>().isTrigger = false;

            }
        }else{
            Debug.Log("mascotita false");
            if( Jugador.GetComponent<PickUpObjects>().PickedObject == null){
                Jugador.GetComponent<PickUpObjects>().PickedObject = this.gameObject;
                this.transform.SetParent(Jugador.transform.GetChild(1));
                this.transform.position = Jugador.transform.GetChild(1).position;

                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponent<Collider>().isTrigger = true;

            }else if (Jugador.GetComponent<PickUpObjects>().PickedObject != null){
                this.transform.SetParent(null);
                this.GetComponent<Rigidbody>().useGravity = true;

                this.GetComponent<Rigidbody>().isKinematic = false;
                Jugador.GetComponent<PickUpObjects>().PickedObject = null;
                this.GetComponent<Collider>().isTrigger = false;

            }
        }
        
    }
}
