using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proceduralsimplista : MonoBehaviour
{

    public GameObject bloque;
    List<GameObject> terreno;

    [Header ("TAMAÑO")]
    public int ancho = 100;
    public int alto = 100;

    [Header ("SEMILLA")]
    [Range(0, 10000)]
    public int semilla = 0;
    public bool SemillaAleatoria = false;

    [Header ("TERRENO")]
    [Range(0, 1)]
    public float GrosorDelTerreno=0.4f;

    [Header("ZOOM")]
    [Range(0.01f,0.9f)]
    public float Zoom = 0.1f;


    private void Start()
    {
        terreno = new List<GameObject>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) generarTerreno();
    }

    void generarTerreno()
    {
        foreach (GameObject bloque in terreno)
        {
            Destroy(bloque);
        }
        terreno = new List<GameObject>();

        if(SemillaAleatoria)
        semilla = Random.Range(1, 10000);

        for (float i = -ancho*Zoom/2; i < ancho*Zoom/2; i+=Zoom)
        {
            for (float j = -alto*Zoom / 2; j < alto*Zoom / 2; j+=Zoom )
            {
                Vector2 vec = new Vector2(i, j);
                if (Mathf.PerlinNoise(i + semilla, j + semilla) < GrosorDelTerreno)
                {
                    GameObject blo = Instantiate(bloque, vec / Zoom, Quaternion.identity);
                    terreno.Add(blo);
                }
            }
        }
    }
}
