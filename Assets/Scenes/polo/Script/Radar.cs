using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    //Pengu
    //Orca
    //Foca
    public static List<Transform> targets= new List<Transform>();

    [SerializeField] private AudioSource biper;
    [SerializeField] float minPeriod = 0.2f; //rapido
    [SerializeField] float distanceFactor = 0.01f; //lento
    [SerializeField] float lastBip = 0f;
    [SerializeField] float decayPower = 1f;

    float NextBopit
    {
        get
        {
            return Mathf.Pow(GetClosest(),decayPower) * distanceFactor + minPeriod + lastBip;
        }
    }
        
    void Update()
    {

        if (Time.time >= NextBopit)
            Bip();

    }
   
    private float GetClosest()
    {
        float dist = float.PositiveInfinity;

        foreach (Transform target in targets)
        {
            /*if (target == null)
            {
                continue;
            }*/
            Vector3 aux = target.position - transform.position;
            float newDist = aux.magnitude;

            if (newDist < dist)
            {
                dist = newDist;
            }
        }

        return dist;
    }

    private void Bip()
    {
        biper.Play();
        lastBip = Time.time;
    }

}
