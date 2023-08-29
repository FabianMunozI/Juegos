using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marcar : MonoBehaviour
{
    GameObject misionInvasores;
    bool no_cambiado = true;
    // Start is called before the first frame update
    void Start()
    {
        misionInvasores = GameObject.Find("Mision Invasores");
    }

    // Update is called once per frame
    void Update()
    {
        if(misionInvasores.GetComponent<Invasores>().Quest_started && no_cambiado)
        {
            ActivarOutline();
            Invoke("DesactivarOutline", 10f);
            no_cambiado = false;
        }
    }

    void ActivarOutline()
    {
        this.GetComponent<Outline>().enabled = true;
    }

    void DesactivarOutline()
    {
        this.GetComponent<Outline>().enabled = false;
    }
}
