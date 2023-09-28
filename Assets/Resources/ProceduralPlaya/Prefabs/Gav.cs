using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gav : MonoBehaviour
{
    private AudioClip grito1, grito2;
    private AudioSource audio;
    private float velocidadhorizontal = .3f;
    // Start is called before the first frame update
    void Start()
    {
        grito1 = Resources.Load<AudioClip>("ProceduralPlaya/Prefabs/Sonidos/Gaviota_Sonido1");
        grito2 = Resources.Load<AudioClip>("ProceduralPlaya/Prefabs/Sonidos/Gaviota_Sonida2");
        audio = GetComponent<AudioSource>();
        Invoke("gritar", Random.Range(10,30));
    }

    void gritar()
    {
        if(Random.Range(0,1) == 0)
            audio.PlayOneShot(grito1, .3f);
        else
            audio.PlayOneShot(grito2, .3f);

        Invoke("gritar", Random.Range(10,30));
    }

    private void Update() {
        transform.localPosition += transform.forward * velocidadhorizontal;
    }

}
