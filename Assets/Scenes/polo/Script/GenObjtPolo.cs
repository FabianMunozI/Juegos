using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenObjtPolo : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] objetos;
    public GameObject[] hielos;
    public GameObject[] glaciares;
    public GameObject[] nubes;
    public int[] objetosCantidad;
    public int cantidadObjetosPorTipo;

    float xmin;
    float xmax;
    float zmin;
    float zmax;

    float xHielomin, zHielomin, xHielomax, zHielomax;
    int cantidadHieloRand, cantidadGlaciarRand;
    int auxHielo, auxGlaciar; //eleccion random de prefabs

    private int cantidadNubes, nubesActuales;
    private List<GameObject> listaNubes = new List<GameObject>();

    void Start()
    {
        zmin = -1100;
        xmin = -1100;
        zmax = 1100;
        xmax = 1100;

        float randomValueX, randomValueY , randomValueZ;

        //Debug.Log("zmin" + zmin);
        //Debug.Log("xmin" + xmin);
        //Debug.Log("zmax" + zmax);
        //Debug.Log("xmax" + xmax);
        for (var pajaros = 0 ; pajaros < objetosCantidad[0] ; pajaros++)
        {
            randomValueX = Random.Range(xmin, xmax);
            randomValueY = Random.Range(86, 111);
            randomValueZ = Random.Range(zmin, zmax);

            Instantiate(objetos[0], new Vector3(randomValueX, randomValueY, randomValueZ), Quaternion.identity);
        }

        for (var i = 1; i < objetos.Length; i++)
        {
            for (var a = 0; a < objetosCantidad[i]; a++)
            {
                randomValueX = Random.Range(xmin, xmax);
                randomValueZ = Random.Range(zmin, zmax);

                Instantiate(objetos[i], new Vector3(randomValueX, 50, randomValueZ), Quaternion.identity);
            }
            
        }

        //Primer Lado
        zHielomin = -800f;
        zHielomax = 800f;
        xHielomin = 1300f;
        xHielomax = 1640f;

        cantidadHieloRand = Random.Range(9, 15);
        cantidadGlaciarRand = Random.Range(3, 6);

        for (var i = 0; i < cantidadHieloRand; i++)
        {

            //prefab de hielo random a poner
            auxHielo = Random.Range(0, 6);

            //posicion random dentro de los limites del cuadrante
            randomValueX = Random.Range(xHielomin, xHielomax);
            randomValueZ = Random.Range(zHielomin, zHielomax);

            Instantiate(hielos[auxHielo], new Vector3(randomValueX, 16, randomValueZ), Quaternion.identity);
        }
        for (var i = 0; i < cantidadGlaciarRand; i++)
        {
            auxGlaciar = Random.Range(0, 3);

            randomValueX = Random.Range(xHielomin, xHielomax);
            randomValueZ = Random.Range(zHielomin - 200, zHielomax + 200);

            if (auxGlaciar == 0)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.identity);
            }
            else if (auxGlaciar == 1)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.Euler(180, 0, 0));
            }
            else if (auxGlaciar == 2)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.Euler(220, 0, 0));
            }
        }

        //Segundo Lado
        cantidadHieloRand = Random.Range(9, 15);
        cantidadGlaciarRand = Random.Range(3, 6);

        for (var i = 0; i < cantidadHieloRand; i++)
        {
            //prefab de hielo random a poner
            auxHielo = Random.Range(0, 6);

            //posicion random dentro de los limites del cuadrante
            randomValueX = Random.Range(-xHielomax, -xHielomin);
            randomValueZ = Random.Range(zHielomin, zHielomax);

            Instantiate(hielos[auxHielo], new Vector3(randomValueX, 16, randomValueZ), Quaternion.identity);
        }
        for (var i = 0; i < cantidadGlaciarRand; i++)
        {
            auxGlaciar = Random.Range(0, 3);

            randomValueX = Random.Range(-xHielomax, -xHielomin);
            randomValueZ = Random.Range(zHielomin - 200, zHielomax + 200);

            if (auxGlaciar == 0)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.identity);
            }
            else if (auxGlaciar == 1)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.Euler(180, 0, 0));
            }
            else if (auxGlaciar == 2)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.Euler(220, 0, 0));
            }
        }

        //Tercer Lado
        zHielomin = 1300f;
        zHielomax = 1640f;
        xHielomin = -800f;
        xHielomax = 580f;

        cantidadHieloRand = Random.Range(9, 15);
        cantidadGlaciarRand = Random.Range(3, 6);

        for (var i = 0; i < cantidadHieloRand; i++)
        {

            //prefab de hielo random a poner
            auxHielo = Random.Range(0, 6);

            //posicion random dentro de los limites del cuadrante
            randomValueX = Random.Range(xHielomin, xHielomax); 
             randomValueZ = Random.Range(zHielomin, zHielomax);

            Instantiate(hielos[auxHielo], new Vector3(randomValueX, 16, randomValueZ), Quaternion.identity);
        }
        for (var i = 0; i < cantidadGlaciarRand; i++)
        {
            auxGlaciar = Random.Range(0, 3);

            randomValueX = Random.Range(xHielomin - 200, xHielomax + 200);
            randomValueZ = Random.Range(zHielomin, zHielomax);

            if (auxGlaciar == 0)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.identity);
            }
            else if (auxGlaciar == 1)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.Euler(180, 0, 0));
            }
            else if (auxGlaciar == 2)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.Euler(220, 0, 0));
            }
        }

        //Cuarto Lado
        cantidadHieloRand = Random.Range(9, 15);
        cantidadGlaciarRand = Random.Range(3, 6);

        for (var i = 0; i < cantidadHieloRand; i++)
        {
            //prefab de hielo random a poner
            auxHielo = Random.Range(0, 6);

            //posicion random dentro de los limites del cuadrante
            randomValueX = Random.Range(xHielomin, xHielomax);
            randomValueZ = Random.Range(-zHielomax, -zHielomin);

            Instantiate(hielos[auxHielo], new Vector3(randomValueX, 16, randomValueZ), Quaternion.identity);
        }
        for (var i = 0; i < cantidadGlaciarRand; i++)
        {
            auxGlaciar = Random.Range(0, 3);

            randomValueX = Random.Range(xHielomin - 200, xHielomax + 200);
            randomValueZ = Random.Range(-zHielomax, -zHielomin);

            if (auxGlaciar == 0)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.identity);
            }
            else if (auxGlaciar == 1)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.Euler(180, 0, 0));
            }
            else if (auxGlaciar == 2)
            {
                Instantiate(glaciares[0], new Vector3(randomValueX, 16, randomValueZ), Quaternion.Euler(220, 0, 0));
            }
        }

        //Nubes
        cantidadNubes = Random.Range(16, 20);
        nubesActuales = 0;

        for (int i = 0; i < cantidadNubes; i++)
        {
            listaNubes.Add((GameObject)Instantiate(nubes[Random.Range(0, nubes.Length)], new Vector3(Random.Range(-1400, 1400), Random.Range(215, 260), Random.Range(-1500, 1500)), Quaternion.identity));
            nubesActuales++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nubesActuales < cantidadNubes)
        {
            listaNubes.Add((GameObject)Instantiate(nubes[Random.Range(0, nubes.Length)], new Vector3(Random.Range(-1500, -1250), Random.Range(215, 260), Random.Range(-1500, 1500)), Quaternion.identity));
            nubesActuales++;
        }
        else
        {

            for (int i = listaNubes.Count - 1; i >= 0; i--)
            {
                if (listaNubes[i].transform.position.x > 1500)
                {
                    DestroyImmediate(listaNubes[i]);
                    listaNubes.RemoveAt(i);
                    nubesActuales--;
                }
            }

        }
    }
}
