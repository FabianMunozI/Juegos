using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaDeposito : MonoBehaviour
{  
    public static int jaulasAdentro = 0;
    public GameObject jaulaPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == string.Format("{0}(Clone)", jaulaPrefab.name))
        {
            jaulasAdentro ++;
            Debug.Log(jaulasAdentro);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == string.Format("{0}(Clone)", jaulaPrefab.name))
        {
            jaulasAdentro --;
        }
    }
}
