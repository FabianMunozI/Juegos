using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBasico : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] objetosPosibles;

    Vector3[] rotaciones;
    int randomRot;

    int randomNoGenerar;
    void Start()
    {
        randomNoGenerar=Random.Range(0,10); // segun ioh es entre 0 y 9
        randomRot=(Random.Range(0,4)); // numero entre 0 y 4 inclusivo pero creo que eso es mentira y es entre 0 y 3


        //Debug.Log(randomRot);
        if(randomNoGenerar!=9){
            Instantiate(objetosPosibles[Random.Range(0,objetosPosibles.Length)], transform.position, Quaternion.Euler(0, randomRot*90, 0));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
