using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteraction : MonoBehaviour
{

    private Transform cameraa;
    public float rayDistance;

    public GameObject TextDetectInteractable;
    GameObject ultimoReconocido=null;

    public GameObject TextDetectDragable;
    GameObject ultimoReconocidoRecoger=null;

    GameObject Activar;
    // Start is called before the first frame update
    void Start()
    {

        cameraa = transform.Find("Camera");
        //TextDetectInteractable = transform.Find("Interactuar").gameObject;
        /*
        TextDetectInteractable.SetActive(false);
        TextDetectDragable.SetActive(false); */
    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.DrawRay(cameraa.position, cameraa.forward * rayDistance, Color.red);
        RaycastHit hit1;
        if (Physics.Raycast(cameraa.position, cameraa.forward, out hit1, rayDistance, LayerMask.GetMask("interactable"))){
            Deselect();
            SelectedObjet(hit1.transform);
            if(Input.GetButtonDown("Interactable")){ // letra e
                hit1.transform.GetComponent<Interactable>().Interact();
            }
        }else{
            Deselect();
        }
        RaycastHit hit2;
        if (Physics.Raycast(cameraa.position, cameraa.forward, out hit2, rayDistance, LayerMask.GetMask("grabable"))){
            Deselect2();
            if(this.GetComponent<PickUpObjects>().PickedObject ==null){
                SelectedObjet2(hit2.transform);
            }else{
                Deselect2();
                /* if(Input.GetKeyDown(KeyCode.F)){
                    GetComponent<PickUpObjects>().PickedObject.transform.GetComponent<Interactable>().Interact();
                } */
            }
            if(Input.GetKeyDown(KeyCode.F) && this.GetComponent<PickUpObjects>().PickedObject == null){ //agarrar objeto
                hit2.transform.GetComponent<Interactable>().Interact();
            }
            else if(Input.GetKeyDown(KeyCode.F) && GetComponent<PickUpObjects>().PickedObject != null) {  //soltar objeto
                GetComponent<PickUpObjects>().PickedObject.transform.GetComponent<Interactable>().Interact();
            }
        }else if(Input.GetKeyDown(KeyCode.F) && GetComponent<PickUpObjects>().PickedObject != null) {  //soltar objeto
                    GetComponent<PickUpObjects>().PickedObject.transform.GetComponent<Interactable>().Interact();
                    Deselect2();
        }
        else{
            Deselect2();
        }
        

        
    }

    void SelectedObjet(Transform transform){
        ultimoReconocido = transform.gameObject;
    }

    void Deselect(){
        if(ultimoReconocido){
            ultimoReconocido = null;
        }
    }

    void SelectedObjet2(Transform transform){
        ultimoReconocidoRecoger = transform.gameObject;
    }

    void Deselect2(){
        if(ultimoReconocidoRecoger){
            ultimoReconocidoRecoger = null;
        }
    }

    void OnGUI() {

        if(ultimoReconocido){
            TextDetectInteractable.SetActive(true);
        } else{
            TextDetectInteractable.SetActive(false);
        }

        if(ultimoReconocidoRecoger){
            TextDetectDragable.SetActive(true);
        }else{
            TextDetectDragable.SetActive(false);
        }
    }
}


