using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UsableArrastrable : MonoBehaviour
{
    // Para usar este script se necesita una caja( o al menos su collider) hacerlo padre del objeto que se quiere empujar en caso de ser distinto de un cubo

    public GameObject player;

    Vector3 ejesX1;
    Vector3 ejesZ1;

    Vector3 ejesX2;
    Vector3 ejesZ2;

    float zDistanceOfCenter;
    float xDistanceOfCenter;

    public bool ScriptEmpubleEsPadre = false; //  OJO CON ESTA VARIABLE
    GameObject actual;


    void Start()
    {

        if(!ScriptEmpubleEsPadre){
            actual = transform.parent.gameObject;
        }else{
            actual = gameObject;
        }
        player = GameObject.Find("Player");
        zDistanceOfCenter = GetComponent<BoxCollider>().size.z/2;
        xDistanceOfCenter = GetComponent<BoxCollider>().size.x/2;
        
        //Vector3.Distance

    }
    // Update is called once per frame
    
    private void FixedUpdate() {
        ejesX1 = transform.position + new Vector3(xDistanceOfCenter,0,0);
        ejesX2 = transform.position + new Vector3(-xDistanceOfCenter,0,0);

        ejesZ1 = transform.position + new Vector3(0,0,zDistanceOfCenter);
        ejesZ2 = transform.position + new Vector3(0,0,-zDistanceOfCenter);

        Vector3 pP = player.transform.position;

        float dX1 = Vector3.Distance(pP, ejesX1);
        float dX2 = Vector3.Distance(pP, ejesX2);
        float dZ1 = Vector3.Distance(pP, ejesZ1);
        float dZ2 = Vector3.Distance(pP, ejesZ2);


        float distances = Mathf.Min(dX1, dX2, dZ1, dZ2);

        if (distances == dX1 || distances == dX2){
            actual.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        }else{
            actual.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        
    }

    

}
