using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcNav : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;
    private Vector3 actualDestination;
    static public bool isInDialogue = false;
    private float movementSpeed;
    private Animator animator;
    private float animSpeedMax = 0.6f;
    private float animSpeed;
    private float actualSpeed;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = movePositionTransform.position;
        movementSpeed = navMeshAgent.speed;
        animator = GetComponent<Animator>();
    }
    void Update()
    {   

        navMeshAgent.destination = movePositionTransform.position;
        
        if(isInDialogue || (navMeshAgent.remainingDistance <= 0.15)){
            navMeshAgent.isStopped = true;
            animator.SetFloat("movimiento", 0);
        }
        else{
            navMeshAgent.isStopped = false;
            animator.SetFloat("movimiento", animSpeedMax);
        }
        
    }
}
