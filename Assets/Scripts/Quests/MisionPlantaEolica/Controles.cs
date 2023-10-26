using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controles : MonoBehaviour
{   
    [SerializeField] UnityEvent KeyDownE;
    [HideInInspector] public bool detenido = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && !detenido){
            KeyDownE.Invoke();
        }
    }
}
