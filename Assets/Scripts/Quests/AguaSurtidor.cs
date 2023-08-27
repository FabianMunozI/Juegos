using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AguaSurtidor : MonoBehaviour
{   
    private bool misionActiva;
    private bool surtidorAbierto;
    public static bool interactuado;
    public float tiempoTimer;
    
    [SerializeField] private float timer;
    public float speed;

    public Vector3 positionAguaArriba, positionAguaAbajo;
    // Start is called before the first frame update
    void Start()
    {
        timer = tiempoTimer;
        misionActiva = false;
        surtidorAbierto = false;
        interactuado = false;
    }

    // Update is called once per frame
    void Update()
    {
        /* if(misionActiva){
            timer -= Time.deltaTime;
        }
        if(!surtidorAbierto && timer <= 0){
            surtidorAbierto = true;
            SubirAgua();
        } */
        timer -= Time.deltaTime;
        if(timer <= 0 && transform.position != positionAguaArriba){
            SubirAgua();
        }

        if(interactuado){
            BajarAgua();
            if(transform.position == positionAguaAbajo){
                interactuado = false;
                timer = tiempoTimer;
            }
        }
    }

    private void SubirAgua(){
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, positionAguaArriba, step);
    }
    private void BajarAgua(){
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, positionAguaAbajo, step);
    }
}
