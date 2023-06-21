using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasureroReciclaje : Usable
{
    public GameObject Jugador;
    
    public MisionStart mision;
    
    public string TipoBasurero;

    public GameObject objetoPickeadoReferencia;

    public void Start(){
        Jugador = GameObject.Find("Player");
        mision = GameObject.Find("Plataforma").GetComponent<MisionStart>();
        
        
        //Invoke("Usar",2f);
    }
    public override void usable()
    {
        base.usable();
        // tipos: vidrio=verde , plastico=amarillo , pila=roja , carton=azul

        string uno =objetoPickeadoReferencia.GetComponent<Pickable>().tipoBasura;
        if(string.Equals(uno, TipoBasurero )){
            objetoPickeadoReferencia.SetActive(false);
            Debug.Log("Acertaste");

            mision.ObjBienPuestos +=1;
        }else{
            objetoPickeadoReferencia.SetActive(false);
            Debug.Log("No acertaste");

            mision.Vidas-=1;
        }

        
    }
}
