using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarNav : MonoBehaviour
{

    //[SerializeField] private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;
    private Vector3 actualDestination;
    static public bool isInDialogue = false;
    private float movementSpeed;

    public Vector3 direccionActual;
    public List<Vector3> direcciones;
    private int i;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = direcciones[0];
        movementSpeed = navMeshAgent.speed;
        i = 1;
        /* for(int j = 0; j < direcciones.Count; j++){
            print(direcciones[j]);
        } */
    }

    // Update is called once per frame
    void Update()
    {   
        direccionActual = navMeshAgent.destination;
        //print(navMeshAgent.remainingDistance);
        if(navMeshAgent.remainingDistance <= 5){
            print(direcciones[i]);
            navMeshAgent.destination = direcciones[i];
            i++;
            if(i >= direcciones.Count){
                i = 0;
            }
        }
    }
}
