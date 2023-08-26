using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;
using Unity.AI.Navigation;

public class generarMesh : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject plano;
    void Start()
    {
        plano= GameObject.Find("RuntimeMesh");
        NavMeshBuilder.BuildNavMesh();
        plano.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
