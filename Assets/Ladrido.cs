using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladrido : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject audios;
    GameObject Player;
    int c =0;
    void Start()
    {
        Player = GameObject.Find("Player");
        audios = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && Player.GetComponent<Mascota>().eresLaMascota){
            audios.transform.GetChild(c).GetComponent<AudioSource>().Play();
            c+=1;
            if(c==3){
                c=0;
            }

            llamarAtencion();

        }
    }

    void llamarAtencion(){
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("npcmascota");
        Debug.Log(npcs.Length);
        for(int i = 0; i< npcs.Length ; i++){
            if(Vector3.Distance(transform.position, npcs[i].transform.position) <50 ){
                Debug.Log("zi");
                npcs[i].transform.LookAt(transform);
            }
        }

    }
}

