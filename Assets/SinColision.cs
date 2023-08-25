using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinColision : MonoBehaviour
{
    // Start is called before the first frame update

    public int Disponible;
    void Start()
    {
        Disponible=0;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        if(other != null){
            Disponible+=1;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other !=null){
            Disponible -=1;
        }
    }
}
