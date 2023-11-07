using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaviotaBehaviour : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;

    private float velocidadinicial = .15f;
    private float velocidadhorizontal = .1f;
    private AudioClip grito1, grito2;
    private AudioSource audioGav;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb =  GetComponent<Rigidbody>();
        Invoke("callAnimationsIdle", Random.Range(0,5));
        grito1 = Resources.Load<AudioClip>("ProceduralPlaya/Prefabs/Sonidos/Gaviota_Sonido1");
        grito2 = Resources.Load<AudioClip>("ProceduralPlaya/Prefabs/Sonidos/Gaviota_Sonida2");
        audioGav = GetComponent<AudioSource>();
    }

    void callAnimationsIdle()
    {
        int animation_integer = Random.Range(0,6);
        if(animation_integer == 0) Invoke("reproducirgrito1",0.2f);
        else if(animation_integer == 1) Invoke("reproducirgrito2",0.2f);
        animator.SetInteger("TransicionIdle", animation_integer);
        Invoke("callAnimationsIdle", Random.Range(0,5)); // Loop
    }

    void reproducirgrito1()
    {
        audioGav.PlayOneShot(grito1, .3f);
    }

    void reproducirgrito2()
    {
        audioGav.PlayOneShot(grito2, .3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == string.Format("Player"))
        {
            Debug.Log("Jugador entró.");
            animator.SetBool("Volar", true);
            rb.useGravity = false;
        }

        
    }

    void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("eleto|PartirVolandoando"))
        {
            if(transform.position.y < 3){
                    transform.position += new Vector3(0,velocidadinicial,0);
                    velocidadinicial += 0.001f;
            }
            else if(transform.position.y < 7){
                    transform.position += new Vector3(0,velocidadinicial,0);
                    velocidadinicial += 0.002f;
            }
            else if(transform.position.y < 15){
                    transform.position += new Vector3(0,velocidadinicial,0);
                    velocidadinicial += 0.003f;
            }
            if (velocidadhorizontal < .2f) velocidadhorizontal += 0.005f;
    
            transform.localPosition += transform.forward * velocidadhorizontal;
           
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Esqueleto|Volar"))
        {
            if(transform.position.y < 3){
                    transform.position += new Vector3(0,velocidadinicial,0);
                    velocidadinicial += 0.001f;
            }
            else if(transform.position.y < 7){
                    transform.position += new Vector3(0,velocidadinicial,0);
                    velocidadinicial += 0.002f;
            }
            else if(transform.position.y < 15){
                    transform.position += new Vector3(0,velocidadinicial,0);
                    velocidadinicial += 0.003f;
            }
            transform.localPosition += transform.forward * velocidadhorizontal;
        }

        

    }

}
