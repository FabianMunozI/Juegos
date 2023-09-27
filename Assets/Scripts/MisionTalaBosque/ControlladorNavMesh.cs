using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class ControlladorNavMesh : MonoBehaviour
{
    [HideInInspector]
    public Transform perseguirObjetivo;

    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    public void ActualizarPuntoDestinoNavMeshAgent(Vector3 puntoDestino){
        navMeshAgent.destination = puntoDestino;
        navMeshAgent.Resume();
    }

    public void ActualizarPuntoDestinoNavMeshAgent(){
        ActualizarPuntoDestinoNavMeshAgent(perseguirObjetivo.position);
    }

    public void DetenerNavMeshAgent(){
        navMeshAgent.Stop();
    }

    public bool HemosLlegado(){
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending;
    }
}
