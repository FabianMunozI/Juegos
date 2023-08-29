using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovAutoridad : MonoBehaviour
{
    // Start is called before the first frame update
    public bool primeraFase;

    public bool segundaFase;
    public bool terceraFase;

    bool cuartaFase;
    public Vector3 primerVector, segundoVector, tercerVector, cuartoVector;
    Animator controler;
    float vel;
    GameObject posicionesObj;
    GameObject player;
    void Start()
    {
        player=GameObject.Find("Player");

        GetComponent<Collider>().isTrigger = true;
        posicionesObj = transform.parent.parent.parent.GetChild(24).GetChild(4).gameObject;

        controler = GetComponent<Animator>();
        controler.SetTrigger("caminar");

        vel=1f;
        primeraFase=true;

        this.transform.position = new Vector3(posicionesObj.transform.position.x, posicionesObj.transform.position.y - 0.5f, transform.position.z);
        

        Vector3 targetPosition = posicionesObj.transform.GetChild(0).position;
        targetPosition.y = this.transform.position.y; 
        this.transform.LookAt(targetPosition); 

        primerVector = (posicionesObj.transform.GetChild(0).position -  transform.position).normalized; 
        segundoVector = (posicionesObj.transform.GetChild(1).position -  posicionesObj.transform.GetChild(0).position).normalized; 
        tercerVector = (posicionesObj.transform.GetChild(2).position -  posicionesObj.transform.GetChild(1).position).normalized; 
        cuartoVector = (posicionesObj.transform.GetChild(3).position -  posicionesObj.transform.GetChild(2).position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(primeraFase){
            transform.position += primerVector * Time.deltaTime * vel;

            if(Vector3.Distance(transform.position, posicionesObj.transform.GetChild(0).position) <0.5f){
                transform.position = posicionesObj.transform.GetChild(0).position;

                Vector3 targetPosition2 = posicionesObj.transform.GetChild(1).position;
                targetPosition2.y = transform.position.y; 
                this.transform.LookAt(targetPosition2); 

                primeraFase=false;
                segundaFase=true;
            }
        }else if(segundaFase){
            transform.position += segundoVector * Time.deltaTime * vel;

            if(Vector3.Distance(transform.position, posicionesObj.transform.GetChild(1).position) <0.5f){
                //GetComponent<Collider>().isTrigger = false;
                Vector3 targetPosition2 = posicionesObj.transform.GetChild(2).position;
                targetPosition2.y = transform.position.y; 
                this.transform.LookAt(targetPosition2); 
                //controler.SetTrigger("quieto");
                segundaFase=false;
                terceraFase=true;
            }

        }else if(terceraFase){
            transform.position += tercerVector * Time.deltaTime * vel;

            if(Vector3.Distance(transform.position, posicionesObj.transform.GetChild(2).position) <0.5f){
                //GetComponent<Collider>().isTrigger = false;
                Vector3 targetPosition2 = posicionesObj.transform.GetChild(3).position;
                targetPosition2.y = transform.position.y; 
                this.transform.LookAt(targetPosition2); 
                terceraFase = false;
                cuartaFase =true;
            }

        }else if(cuartaFase){
            transform.position += cuartoVector * Time.deltaTime * vel;

            if(Vector3.Distance(transform.position, posicionesObj.transform.GetChild(3).position) <0.5f){
                GetComponent<Collider>().isTrigger = false;
                controler.SetTrigger("quieto");

                
                Vector3 targetPosition2 = player.transform.position;
                targetPosition2.y = transform.position.y; 
                this.transform.LookAt(targetPosition2); 
                
                cuartaFase =false;
            }

        }

    }
}
