using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionStart : MonoBehaviour
{
    public Material color1;
    public Material color2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            this.gameObject.GetComponent<MeshRenderer>().material.color = color1.color;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            this.gameObject.GetComponent<MeshRenderer>().material.color = color2.color;
        }
    }
}
