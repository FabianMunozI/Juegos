using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

public class NPCmov : MonoBehaviour
{
    // Start is called before the first frame update
    public int posGenerado;

    public GameObject posGenObj;

    public bool primeraFase;

    public bool segundaFase;

    public Animator controler;

    float vel;
    public Vector3 primerVector, segundoVector;

    public bool quedarQuieto;
    void Start()
    {
        quedarQuieto=false;
        controler = GetComponent<Animator>();
        controler.SetTrigger("caminar");

        vel=1f;
        primeraFase=true;

        this.transform.position = posGenObj.transform.position;

        Vector3 targetPosition = posGenObj.transform.GetChild(0).position;
        targetPosition.y = this.transform.position.y; 
        this.transform.LookAt(targetPosition); 

        primerVector = -(posGenObj.transform.position -  posGenObj.transform.GetChild(0).position).normalized; 
        segundoVector = -(posGenObj.transform.GetChild(0).position -  posGenObj.transform.GetChild(1).position).normalized; 

        
    }

    // Update is called once per frame
    void Update()
    {

        if(primeraFase){
            transform.position += primerVector * Time.deltaTime * vel;

            if(Vector3.Distance(transform.position, posGenObj.transform.GetChild(0).position) <0.5f){
                transform.position = posGenObj.transform.GetChild(0).position;

                Vector3 targetPosition2 = posGenObj.transform.GetChild(1).position;
                targetPosition2.y = posGenObj.transform.GetChild(2).position.y; 
                this.transform.LookAt(targetPosition2); 

                primeraFase=false;
                segundaFase=true;
            }
        }else if(segundaFase){
            transform.position += segundoVector * Time.deltaTime * vel;
            if(Vector3.Distance(transform.position, posGenObj.transform.GetChild(1).position) <0.5f){
                GetComponent<Collider>().isTrigger = false;
                controler.SetTrigger("quieto");
                segundaFase=false;
                quedarQuieto = true;
            }

        }

        if(!segundaFase && !primeraFase){
            controler.SetTrigger("quieto");
        }

        
    }

    private void OnTriggerEnter(Collider other) {  // esto era enter
        if(other.CompareTag("publicoritmo") && !quedarQuieto){

            controler.SetTrigger("quieto");
            segundaFase=false;
            transform.position += -segundoVector*0.2f;
            
            GetComponent<Collider>().isTrigger = false;
            quedarQuieto=true;
            
        }
    }

}
