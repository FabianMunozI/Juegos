using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nubes : MonoBehaviour
{
    // Start is called before the first frame update
    private float velocidad;
    private Vector3 vectorVelocidad;
    void Start()
    {
        velocidad = Random.Range(0.001f,.5f);
        vectorVelocidad = new Vector3(velocidad, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += vectorVelocidad;
    }
}
