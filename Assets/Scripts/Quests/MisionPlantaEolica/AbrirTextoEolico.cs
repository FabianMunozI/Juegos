using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirTextoEolico : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Inicio(){
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void Termino(){
        Cursor.lockState = CursorLockMode.Locked;
    }
}
