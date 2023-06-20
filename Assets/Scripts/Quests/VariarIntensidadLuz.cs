using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariarIntensidadLuz : MonoBehaviour
{
    Light fire_light;

    void Start()
    {
        fire_light = GetComponent<Light>();
    }

    void Update()
    {
        fire_light.intensity = Mathf.PingPong(Time.time * 5, 3) + 3;
    }
}


