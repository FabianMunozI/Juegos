using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Permiso : Interactable
{
    
    public int tipo;
    public MisionTala misionTala;
    void Start() {
        
    }

    void Awake(){
        misionTala = transform.parent.parent.parent.parent.GetComponent<MisionTala>();
        if(tipo==0){
            misionTala.permiso1 = this.gameObject;
        }
        else if(tipo==1){
            misionTala.permiso2 = this.gameObject;
        }
        else if(tipo==2){
            misionTala.permiso3 = this.gameObject;
        }
    }
    
    public override void Interact()
    {
        base.Interact();
        
        if(tipo==0){
            misionTala.permisoUno = true;
            gameObject.SetActive(false);
            this.transform.parent.parent.parent.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
        else if(tipo==1){
            misionTala.permisoDos = true;
            gameObject.SetActive(false);
            this.transform.parent.parent.parent.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
        else if(tipo==2){
            misionTala.permisoTres = true;
            gameObject.SetActive(false);
        }

    }
}
