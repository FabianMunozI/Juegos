using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsCharacter : MonoBehaviour
{
    public int tipo;
    void Start()
    {   
        transform.position = new Vector3(transform.position.x, 200f, transform.position.z);
        Invoke("FindLand",0.5f);
        //FindLand();
    }

    public void FindLand()
    {   
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y+5, hitInfo.point.z);
        }
        else
        {
            ray = new Ray(transform.position, transform.up);
            if (Physics.Raycast(ray, out hitInfo))
            {
                transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y+5, hitInfo.point.z);
            }
        }
    }
}
