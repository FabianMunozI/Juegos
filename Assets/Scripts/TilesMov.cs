using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TilesMov : MonoBehaviour
{
    float multiplicador;
    GameObject Critmo;

    RitmoTeclas rtiles;
    public float vel;

    AudioSource audioFail;
    // Start is called before the first frame update
    void Start()
    {
        //audioFail = GetComponent<AudioSource>();
        Critmo = GameObject.Find("Critmo");
        rtiles = Critmo.transform.GetChild(0).GetChild(0).GetComponent<RitmoTeclas>();
        multiplicador=2.75f;

        Invoke("siSeDestruyePorNoClick", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Vector3.down * Time.deltaTime * 100 * multiplicad
        transform.position += new Vector3(0, -vel, 0) * Time.deltaTime;
    }

    public void siSeDestruyePorNoClick() {
        rtiles.puntaje -=1;
        //Debug.Log("descontar");
        rtiles.ActualizarPuntajes();
        //audioFail.Play();
    }
}
