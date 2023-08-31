using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class funcionBotonAceptar : MonoBehaviour
{
    // Start is called before the first frame update
    public MisionStart referenciaMisionStart;


    public void funcionAceptar(){
        referenciaMisionStart.misionStarter();
        Cursor.lockState = CursorLockMode.Locked;
        referenciaMisionStart.Jugador.GetComponent<FpsCamera>().enabled = true;

    }

    public void funcionRechazar(){
        Cursor.lockState = CursorLockMode.Locked;
        referenciaMisionStart.Jugador.GetComponent<FpsCamera>().enabled = true;
    }

}
