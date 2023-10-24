using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladrido : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject audios;
    GameObject Player;
    int c =0;

    float estaDisponible;
    void Start()
    {
        estaDisponible = 0f;
        Player = GameObject.Find("Player");
        audios = transform.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        estaDisponible -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.E) && Player.GetComponent<Mascota>().eresLaMascota){
            audios.transform.GetChild(c).GetComponent<AudioSource>().Play();

            if(estaDisponible<=0){
                estaDisponible=3f; // coolDownLadrido
            
                c+=1;
                if(c==3){
                    c=0;
                }

                llamarAtencion();
            }
            

        }
    }

    void llamarAtencion(){
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("npcTala");
        for(int i = 0; i< npcs.Length ; i++){
            if(Vector3.Distance(transform.position, npcs[i].transform.position) <20f &&  Vector3.Distance(transform.position, npcs[i].transform.position)>5f){
                //Debug.Log("zi");

                npcs[i].GetComponent<ControlladorNavMesh>().ActualizarPuntoDestinoNavMeshAgent(transform.position);

                /* Vector3 targetPosition = transform.position;
                targetPosition.y = npcs[i].transform.position.y;
                npcs[i].transform.LookAt(targetPosition); */
                //npcs[i].transform.LookAt(transform);
            }
        }

    }
}

