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

    public int seed2;
    System.Random prng;
    void Start()
    {
        Invoke("generar",0.1f);
    }

    void generar(){
        zmin = transform.GetChild(0).position.z;
        xmin = transform.GetChild(0).position.x;
        zmax = transform.GetChild(1).position.z;
        xmax = transform.GetChild(1).position.x;

        if (!PlayerPrefs.HasKey("seedDesierto"))
        {
            int seed = Random.Range(0, 10000);
            PlayerPrefs.SetInt("seedDesierto", seed);
            PlayerPrefs.Save();
        }
        seed2 = PlayerPrefs.GetInt("seedDesierto");//Random.Range(0,10000);
        prng = new System.Random(seed2);
        /* Debug.Log("zmin" + zmin);
        Debug.Log("xmin" + xmin);
        Debug.Log("zmax" + zmax);
        Debug.Log("xmax" + xmax); */

        for (var i = 0; i < objetos.Length; i++)
        {
            for (var a = 0; a < objetosCantidad[i]; a++)
            {
                float randomValueX = prng.Next (Mathf.FloorToInt(xmin), Mathf.FloorToInt(xmax));
                float randomValueZ = prng.Next (Mathf.FloorToInt(zmin), Mathf.FloorToInt(zmax));
                //float randomValueX = Random.Range(xmin, xmax);
                //float randomValueZ = Random.Range(zmin, zmax);

                Instantiate(objetos[i], new Vector3(randomValueX, 100, randomValueZ), Quaternion.identity);
            }
            
        }
    }
}
