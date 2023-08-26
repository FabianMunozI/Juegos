using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AlinearPlanoNavMesh : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 posPlayer;

    public float altura;
    void Start()
    {
        posPlayer = GameObject.Find("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(posPlayer.x, altura, posPlayer.z);
    }
}
