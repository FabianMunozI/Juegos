using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avemovimiento : MonoBehaviour
{
    float multiplicador;
    Vector3 pos_inicial;
    // Start is called before the first frame update
    void Start()
    {
        pos_inicial = transform.position;
        multiplicador=2.75f;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z >= 50){
            transform.position = pos_inicial;
        }
        transform.position += Vector3.forward * Time.deltaTime * 1 * multiplicador ;
    }
}