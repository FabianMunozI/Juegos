using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSentada2 : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("callAnimationsIdle", Random.Range(10,30));
    }

    void callAnimationsIdle()
    {
        int animation_integer = Random.Range(0,1);
        animator.SetInteger("RandomInt", animation_integer);
        Invoke("callAnimationsIdle", Random.Range(10,30)); // Loop
    }

}

