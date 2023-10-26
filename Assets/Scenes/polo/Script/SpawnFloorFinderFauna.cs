using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFloorFinderFauna : MonoBehaviour
{
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private LayerMask evitarAgua;
    [SerializeField] private Vector3 offSet;


    public float radio = 100f;
    public Vector3 centroZona;

    void Start()
    {
        Invoke("Angelo", 0.65f);
    }

    private void Angelo()
    {
        RaycastHit hitinfo;

        bool posicionEncontrada = false;


        while (!posicionEncontrada)
        {
            if (Physics.Raycast(transform.position+Vector3.up*100, Vector3.down, out hitinfo, 2000f, floorMask))
            {
                if (!(evitarAgua == (evitarAgua | (1 << hitinfo.collider.gameObject.layer)))) // evaluar si el layer que choco el raycast es agua
                {
                    transform.position = hitinfo.point + offSet;
                    posicionEncontrada = true;
                }
                else
                {
                    Vector3 offset = Vector3.zero;
                    do
                    {
                        offset = new Vector3(Random.Range(-radio, radio), 0f, Random.Range(-radio, radio));
                    } while (offset.sqrMagnitude >= radio * radio);
                    transform.position = centroZona + offset;
                }
            }
        }

    }

}
