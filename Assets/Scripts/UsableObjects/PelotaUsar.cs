using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelotaUsar : Usable
{
    public GameObject Jugador;
    public float potenciaFuerza;

    public void Start(){
        Jugador = GameObject.Find("Player");
        //Invoke("Usar",2f);
    }
    public override void usable()
    {
        base.usable();
        //Debug.Log("tambien entre aki");
        Vector3 direccionMirarJugador= Jugador.transform.GetChild(0).GetChild(0).transform.position - Jugador.transform.GetChild(0).position;  
        this.transform.GetComponent<Rigidbody>().AddForce(direccionMirarJugador * potenciaFuerza* 300);      

        
    }
}
