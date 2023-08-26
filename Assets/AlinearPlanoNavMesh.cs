using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AlinearPlanoNavMesh : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 posPlayer;
    void Start()
    {
        posPlayer = GameObject.Find("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(posPlayer.x, 5f, posPlayer.z);
    }
}
