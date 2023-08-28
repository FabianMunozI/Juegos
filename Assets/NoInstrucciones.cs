using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoInstrucciones : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject instrucciones;
    void Start()
    {
        instrucciones = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void apagarInstrucciones(){
        instrucciones.transform.parent.GetChild(4).gameObject.SetActive(true);
        instrucciones.transform.parent.GetChild(0).gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        instrucciones.SetActive(false);

    }
}
