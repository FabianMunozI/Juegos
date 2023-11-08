using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gav : MonoBehaviour
{
    private AudioClip grito1, grito2;
    private AudioSource audioGav;
    private float velocidadhorizontal = .3f;
    // Start is called before the first frame update
    void Start()
    {
        grito1 = Resources.Load<AudioClip>("ProceduralPlaya/Prefabs/Sonidos/Gaviota_Sonido1");
        grito2 = Resources.Load<AudioClip>("ProceduralPlaya/Prefabs/Sonidos/Gaviota_Sonida2");
        audioGav = GetComponent<AudioSource>();
        Invoke("gritar", Random.Range(10,30));
    }

    void gritar()
    {
        if(Random.Range(0,1) == 0)
            audioGav.PlayOneShot(grito1, .3f);
        else
            audioGav.PlayOneShot(grito2, .3f);

        Invoke("gritar", Random.Range(10,30));
    }

    private void Update() {
        transform.localPosition += transform.forward * velocidadhorizontal;
    }

}
