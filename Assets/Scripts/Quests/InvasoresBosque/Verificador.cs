
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Verificador : MonoBehaviour
{

    public int collisions = 0;

    void OnCollisionEnter(Collision collision)
    {
        collisions++;
    }

    void OnCollisionExit(Collision collision)
    {
        collisions--;
    }
}
