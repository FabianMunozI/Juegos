using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class ObjectNav : MonoBehaviour
{
    public NavMeshAgent agent;
    public float tiempoEntreReposicion;
    public float x_superior, x_inferior;
    public float z_superior, z_inferior;
    private float x_aleatoria;
    private float z_aleatoria;
    private Vector3 randomPoint;
    [SerializeField] private float timer;
    private Vector3 result;

    void Start(){
       timer = tiempoEntreReposicion;
       result = transform.position;
    }
    // Update is called once per frame
    void Update()
    {   
        if(agent.isStopped){
            
            timer -= Time.deltaTime;
            if(timer <= 0){
                result = GenerateRandomPoint();
                gameObject.transform.position = result;
                tiempoEntreReposicion = UnityEngine.Random.Range(5,10);
                timer = tiempoEntreReposicion;
            } 
        }
  
    }

    public void StartTimer(){
        timer -= Time.deltaTime;
        if(timer <= 0){
            result = GenerateRandomPoint();
            gameObject.transform.position = result;
            tiempoEntreReposicion = UnityEngine.Random.Range(5,10);
            timer = tiempoEntreReposicion;
        } 
    }
    public Vector3 GenerateRandomPoint(){
        bool flag = true;
        NavMeshHit hit;
        Vector3 result = new Vector3(0,0,0);
        while(flag){
            x_aleatoria = UnityEngine.Random.Range(x_inferior, x_superior);
            z_aleatoria = UnityEngine.Random.Range(z_inferior, z_superior);
            //print(z_aleatoria);
            randomPoint = new Vector3(x_aleatoria, 0, z_aleatoria);
            if(NavMesh.SamplePosition(randomPoint, out hit, 0.5f, NavMesh.AllAreas)){
                flag = false;
                result = hit.position;
            }
        }
        return result;
    }
}
