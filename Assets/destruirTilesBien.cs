using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destruirTilesBien : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("tile")){
            Destroy(other.gameObject);
            //Debug.Log("algoScript");
        }
    } 
    /* private void OnTriggerEnter2d(Collider2D other) {
        if(other.CompareTag("tile")){
            Destroy(other.gameObject);
            Debug.Log("algo2");
        }
    } */

    
}
