using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcebergCaer : MonoBehaviour
{
    float multiplicador;
    Vector3 pos_inicial;
    // Start is called before the first frame update
    void Start()
    {
        pos_inicial = transform.position;
        multiplicador=4.75f;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= 0){
            transform.position = pos_inicial;
        }
        transform.position += Vector3.down * Time.deltaTime * 1 * multiplicador ;
    }
}