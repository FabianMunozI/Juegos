using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerarObjetos : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] objetos;
    public int[] objetosCantidad;
    public int cantidadObjetosPorTipo;

    float xmin;
    float xmax;
    float zmin;
    float zmax;
    void Start()
    {
        zmin = transform.GetChild(0).position.z;
        xmin = transform.GetChild(0).position.x;
        zmax = transform.GetChild(1).position.z;
        xmax = transform.GetChild(1).position.x;

        for (var i = 0; i < objetos.Length; i++)
        {
            for (var a = 0; a < objetosCantidad[i]; a++)
            {
                float randomValueX = Random.Range(xmin, xmax);
                float randomValueZ = Random.Range(zmin, zmax);

                Instantiate(objetos[i], new Vector3(randomValueX, 50, randomValueZ), Quaternion.identity);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
