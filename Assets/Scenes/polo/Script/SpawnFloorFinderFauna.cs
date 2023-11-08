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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Angelo();
        }
    }

    private void Angelo()
    {
        Vector3 GetNewPos()
        {
            Vector3 offset = Vector3.zero;
            do
            {
                offset = new Vector3(Random.Range(-radio, radio), 0f, Random.Range(-radio, radio));
            } while (offset.sqrMagnitude >= radio * radio);

            return centroZona + offset;
        }
        

        RaycastHit hitinfo;

        bool posicionEncontrada = false;

        Vector3 newPos = GetNewPos();

        while (!posicionEncontrada)
        {
            if (Physics.Raycast(newPos+Vector3.up*100, Vector3.down, out hitinfo, 2000f, floorMask))
            {
                if (!(evitarAgua == (evitarAgua | (1 << hitinfo.collider.gameObject.layer)))) // evaluar si el layer que choco el raycast es agua
                {
                    newPos = hitinfo.point + offSet;
                    posicionEncontrada = true;
                }
                else
                {
                    newPos = GetNewPos();
                }
            }
        }
        transform.position = newPos;

    }

}
