using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LadridoPerroNpc : MonoBehaviour
{
    private bool playerInZone;
    private float timer = 7;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInZone){
            timer -= Time.deltaTime;
            if(timer <= 0){
                audioSource.Play();
                timer = 7;
            }
        }
    }

    public void OnTriggerEnter(Collider other) {
        if(!String.IsNullOrEmpty("Player") && !other.gameObject.CompareTag("Player")) return;
        playerInZone = true;
    }

    public void OnTriggerExit(Collider other) {
        if(!String.IsNullOrEmpty("Player") && !other.gameObject.CompareTag("Player")) return;
        playerInZone = false;
    }
}
