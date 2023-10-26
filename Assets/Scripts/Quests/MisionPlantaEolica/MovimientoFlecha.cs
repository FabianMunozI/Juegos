using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoFlecha : MonoBehaviour
{
    public float vel;
    private int direccion;
    private Vector3 limitador;
    private int state;
    public float timerInput;
    private float timer;
    public GameObject mensajeBien;
    public GameObject mensajeMal;

    public Controles scriptControles;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerInput;
        limitador = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {   
        if (direccion == 0){
            transform.position += new Vector3(-vel, 0, 0) * Time.deltaTime;
            /* limitador += new Vector3(-vel, 0, 0) * Time.deltaTime;
            Debug.Log(limitador.x);
            if (limitador.x <= -23f){ //49
                direccion = 1;
            } */
        }
        else if (direccion == 1){
            transform.position += new Vector3(vel, 0, 0) * Time.deltaTime;
            /* limitador += new Vector3(vel, 0, 0) * Time.deltaTime;
            if (limitador.x >= 23f){ //49
                direccion = 0;
            } */
        }

        if (direccion != 1 && direccion != 0){
            if(timer > 0){
                timer -= Time.deltaTime;
            }
            if(timer <= 0){
                timer = timerInput;
                direccion += 2;
                scriptControles.detenido = false;
                mensajeBien.SetActive(false);
                mensajeMal.SetActive(false);
            }    
        }
        
    }

    public void Reverse(){
        if(direccion == 0){
            direccion = 1;
        }
        else{
            direccion = 0;
        }
    }

    public void ReverseState(){
        if(state == 0){
            state = 1;
        }
        else{
            state = 0;
        }
    }

    public void TeclaPresionada(){
        scriptControles.detenido = true;
        direccion -= 2;
        if(state == 1){
            Debug.Log("Buen trabajo!");
            MostrarMensajeBien();
        }
        else{
            Debug.Log("Fallaste!");
            MostrarMensajeMal();
        }
        
    }

    public void MostrarMensajeBien(){
        mensajeBien.SetActive(true);
    }

    public void MostrarMensajeMal(){
        mensajeMal.SetActive(true);
    }
}
