using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFloorFinderNPC : MonoBehaviour
{
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private LayerMask evitarAgua;
    [SerializeField] private Vector3 offSet;


    void Start()
    {
        Invoke("Angelo", 0.5f);
    }

    private void Angelo()
    {
        RaycastHit hitinfo;

        bool posicionEncontrada = false;

        while (!posicionEncontrada)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hitinfo, 1000f, floorMask))
            {
                if (!(evitarAgua == (evitarAgua | (1 << hitinfo.collider.gameObject.layer)))) // evaluar si el layer que choco el raycast es agua
                {
                    transform.position = hitinfo.point + offSet;
                    posicionEncontrada = true;
                }
                else
                {
                    Vector3 aux = new Vector3(Random.Range(-1200, 1200), 50, Random.Range(-1200, 1200));
                    transform.position = aux;
                }
            }
        }
    }

}
