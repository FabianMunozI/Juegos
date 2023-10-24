using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cableInteraction : Interactable
{
    public int tipo;
    public bool izquierdo;
    public bool seleccionado;
    controladorJuego controladorJuego;
    void Start() {
        controladorJuego = transform.parent.GetComponent<controladorJuego>();
        seleccionado=false;
    }
    
    public override void Interact()
    {
        base.Interact();
        if(izquierdo){
            controladorJuego.seleccionarIzq(tipo, seleccionado);
            seleccionado=!seleccionado;
            controladorJuego.ambosIgualesIzqDer();
        }else if(!izquierdo){
            controladorJuego.seleccionarDer(tipo, seleccionado);
            seleccionado=!seleccionado;
            controladorJuego.ambosIgualesIzqDer();
        }
    }

}
