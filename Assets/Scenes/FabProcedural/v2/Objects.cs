using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public int tipo;
    void Start()
    {   
        transform.position = new Vector3(transform.position.x, 100f, transform.position.z);
        Invoke("FindLand",0.5f);
        //FindLand();
    }

    public void FindLand()
    {   
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
        }
        else
        {
            ray = new Ray(transform.position, transform.up);
            if (Physics.Raycast(ray, out hitInfo))
            {
                transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
            }
        }

        if(tipo == 1){
            transform.position = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);
        }
        else if(tipo == 2){
            transform.position = new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z);
        }
    }
}
