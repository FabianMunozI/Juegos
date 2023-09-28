using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFloorFinder : MonoBehaviour
{
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Vector3 offSet;


    void Start()
    {
        Invoke("Angelo", 0.5f);
    }

    private void Angelo()
    {
        RaycastHit hitinfo;

        if (Physics.Raycast(transform.position, Vector3.down, out hitinfo, 1000f, floorMask))
            transform.position = hitinfo.point + offSet;
    }

}
