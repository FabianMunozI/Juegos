using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AguaSurtidorPadre : AguaSurtidor
{



    // Start is called before the first frame update
    void Start()
    {
        interactuado = false;
        positionAguaAbajo = transform.position;
        positionAguaArriba = new Vector3(positionAguaAbajo.x, positionAguaAbajo.y + 0.045f, positionAguaAbajo.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0){
            timer -= Time.deltaTime;
        }

        if(timer <= 0 && transform.position != positionAguaArriba && !interactuado){
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
