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

        if (Physics.Raycast(transform.position, Vector3.down, out hitinfo, 1000f, floorMask)) // && !(Physics.Raycast(transform.position, Vector3.down, out hitinfo, 1000f, evitarAgua))
            transform.position = hitinfo.point + offSet;
    }

}
