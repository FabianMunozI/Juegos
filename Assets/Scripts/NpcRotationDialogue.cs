using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRotationDialogue : MonoBehaviour
{

    private Vector3 movementDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = transform.forward;
        
    }
}
