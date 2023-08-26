using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LadridoPerroNpc : MonoBehaviour
{   

    private bool playerInZone;
    [SerializeField ]private float timer;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInZone){
            timer -= Time.deltaTime;
            if(timer <= 0){
                audioSource.Play();
                timer = 15;
            }
        }
    }

    public void OnTriggerEnter(Collider other) {

        if(!other.gameObject.CompareTag("Player")) return;
        playerInZone = true;
    }

    public void OnTriggerExit(Collider other) {
        if(!other.gameObject.CompareTag("Player")) return;
        playerInZone = false;
    }
}
