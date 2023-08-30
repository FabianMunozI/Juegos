using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasureroReciclaje : Usable
{
    public AudioSource sonido;
    public GameObject Jugador;
    
    public MisionStart mision;
    
    public string TipoBasurero;

    public GameObject objetoPickeadoReferencia;

    public void Start(){
        sonido = GetComponent<AudioSource>();
        Jugador = GameObject.Find("Player");
        mision = GameObject.Find("PlataformaReciclaje1").GetComponent<MisionStart>();
        
        
        //Invoke("Usar",2f);
    }
    public override void usable()
    {
        base.usable();
        // tipos: vidrio=verde , plastico=amarillo , pila=roja , carton=azul

        string uno =objetoPickeadoReferencia.GetComponent<Pickable>().tipoBasura;
        if(string.Equals(uno, TipoBasurero )){
            objetoPickeadoReferencia.SetActive(false);
            sonido.Play();
            
            //Debug.Log("Acertaste");

            mision.ObjBienPuestos +=1;
            mision.actualizarTextRecolectados();
        }else{
            objetoPickeadoReferencia.SetActive(false);
            //Debug.Log("No acertaste");
            sonido.Play();
            mision.Vidas-=1;
            mision.actualizarTextVidas();
        }

        
    }
}
