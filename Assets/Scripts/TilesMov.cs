using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TilesMov : MonoBehaviour
{
    float multiplicador;

    public float vel;
    // Start is called before the first frame update
    void Start()
    {
        multiplicador=2.75f;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Vector3.down * Time.deltaTime * 100 * multiplicador ;
        transform.position += new Vector3(0, -vel, 0) * Time.deltaTime;
    }
}
