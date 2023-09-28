using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenObjtPolo : MonoBehaviour
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
        zmin = -1100;
        xmin = -1100;
        zmax = 1100;
        xmax = 1100;

        Debug.Log("zmin" + zmin);
        Debug.Log("xmin" + xmin);
        Debug.Log("zmax" + zmax);
        Debug.Log("xmax" + xmax);

        for (var i = 0; i < objetos.Length; i++)
        {
            for (var a = 0; a < objetosCantidad[i]; a++)
            {
                float randomValueX = Random.Range(xmin, xmax);
                float randomValueZ = Random.Range(zmin, zmax);

                Instantiate(objetos[i], new Vector3(randomValueX, 50, randomValueZ), Quaternion.identity);
            }
            
        }

        float xHielomin, zHielomin, xHielomax, zHielomax;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
