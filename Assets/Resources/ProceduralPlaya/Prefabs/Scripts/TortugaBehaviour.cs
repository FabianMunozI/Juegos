using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortugaBehaviour : MonoBehaviour
{
    Animator animator;

    private float velocidad = .001f;
    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("callAnimationsIdle", Random.Range(0,5));
    }

    void callAnimationsIdle()
    {
        int animation_integer = Random.Range(1,2);
        animator.SetInteger("Accion", animation_integer);
        Invoke("callAnimationsIdle", Random.Range(5,20)); // Loop
    }


    void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Tortuga|Caminar"))
        {
            transform.localPosition += transform.forward * velocidad;
           
        }

    }

}
