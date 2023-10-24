using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petroleo_pos : MonoBehaviour
{
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer> ();
        rend.material.SetFloat("_PerX", 0f);
        rend.material.SetFloat("_PerY", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
