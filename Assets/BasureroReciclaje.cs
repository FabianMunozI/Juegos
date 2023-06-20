using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasureroReciclaje : Usable
{
    public GameObject Jugador;
    public MisionStart mision;
    
    public string TipoBasurero;

    public void Start(){
        Jugador = GameObject.Find("Player");
        mision = GameObject.Find("Plataforma").GetComponent<MisionStart>();
        
        
        //Invoke("Usar",2f);
    }
    public override void usable()
    {
        base.usable();

        if(Jugador.GetComponent<PickUpObjects>().PickedObject.GetComponent<Pickable>().tipoBasura == TipoBasurero){
            
            mision.ObjBienPuestos +=1;
        }else if(Jugador.GetComponent<PickUpObjects>().PickedObject.GetComponent<Pickable>().tipoBasura != TipoBasurero){
            mision.Vidas-=1;
        }

        
    }
}
