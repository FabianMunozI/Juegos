using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetivoMision : MonoBehaviour
{

    private float tiempoTimer = 30;
    public float timer;
    private bool activarTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        timer = tiempoTimer;
        //gameObject.GetComponent<ObjetivoMision>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(activarTimer){
            timer -= Time.deltaTime;
            if(timer <= 0){
                Destroy(gameObject);
            }
        }
    }

    public void ActivarCanvasObjetivos(){
        gameObject.GetComponent<ObjetivoMision>().enabled = true;
        gameObject.SetActive(true);
        activarTimer = true;

    }
}
