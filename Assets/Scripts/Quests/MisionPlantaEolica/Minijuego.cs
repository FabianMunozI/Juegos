using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minijuego : MonoBehaviour
{
    public GameObject[] partes;
    private int index = 0;
    private int fallos = 0;
    public GameObject canvasFinal;
    public AbrirTextoEolico abrirTextoScript;
    public GameObject canvasFallos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoltarParte(){
        partes[index].SetActive(true);
        index++;
        if (index >= partes.Length){
            abrirTextoScript.Inicio();
            canvasFinal.SetActive(true);
            canvasFinal.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

    }

    public void FallarSoltarParte(){
        fallos++;
        if(fallos == 1){
            canvasFallos.transform.GetChild(0).gameObject.SetActive(false);
            canvasFallos.transform.GetChild(1).gameObject.SetActive(true);
        }
        if(fallos == 2){
            canvasFallos.transform.GetChild(1).gameObject.SetActive(false);
            canvasFallos.transform.GetChild(2).gameObject.SetActive(true);
        }

        if(fallos >= 3){
            canvasFallos.transform.GetChild(2).gameObject.SetActive(false);
            abrirTextoScript.Inicio();
            canvasFinal.SetActive(true);
            canvasFinal.transform.GetChild(2).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

}
