using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mascotaInteraction : MonoBehaviour
{
    private Transform cameraa;
    public float rayDistance;

   
    public GameObject TextDetectDragable;
    GameObject ultimoReconocidoRecoger=null;

    public bool mascotita;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        mascotita=false;
        if(!Equals(SceneManager.GetActiveScene().name, "PlayaProcedural")){//if(!Equals(SceneManager.GetActiveScene().name, "TP_PlantaTratamiento") || Equals(SceneManager.GetActiveScene().name, "PlantaTratamiento"))
            
            enabled=false;
        }

        if(Equals(SceneManager.GetActiveScene().name, "PlantaTratamientoAgua")){
            
            enabled=true;
        }

        cameraa = transform.GetChild(2);

        //TextDetectInteractable = transform.Find("Interactuar").gameObject;
        //TextDetectDragable = transform.Find("Recoger").gameObject;
        /*
        TextDetectInteractable.SetActive(false);
        TextDetectDragable.SetActive(false); */
    }

    // Update is called once per frame
    void Update()
    {
        ///////////////////////////////////////////////////////////////////////////////
        RaycastHit hit2;
        if (Physics.Raycast(cameraa.position, cameraa.forward, out hit2, rayDistance, LayerMask.GetMask("grabable") ) && player.GetComponent<PickUpObjects>().PickedObjectMascota == null && player.GetComponent<Mascota>().eresLaMascota && player.GetComponent<PickUpObjects>().PickedObject==null){
            if(hit2.collider.CompareTag("recogiblemascota")){
                Deselect2();
                if(player.GetComponent<PickUpObjects>().PickedObjectMascota ==null){
                    SelectedObjet2(hit2.transform);
                }else{
                    Deselect2();
                    /* if(Input.GetKeyDown(KeyCode.F)){
                        GetComponent<PickUpObjects>().PickedObject.transform.GetComponent<Interactable>().Interact();
                    } */
                }
                if(Input.GetKeyDown(KeyCode.F) && player.GetComponent<PickUpObjects>().PickedObjectMascota == null){ //agarrar objeto
                    mascotita = true;
                    hit2.transform.GetComponent<Interactable>().Interact();
                }
                else if(Input.GetKeyDown(KeyCode.F) && player.GetComponent<PickUpObjects>().PickedObjectMascota != null) {  //soltar objeto
                    player.GetComponent<PickUpObjects>().PickedObjectMascota.transform.GetComponent<Interactable>().Interact();
                    mascotita = false;
                }
            }

            
        }else if(Input.GetKeyDown(KeyCode.F) && player.GetComponent<PickUpObjects>().PickedObjectMascota != null && player.GetComponent<Mascota>().eresLaMascota ) {  //soltar objeto
                    player.GetComponent<PickUpObjects>().PickedObjectMascota.transform.GetComponent<Interactable>().Interact();
                    Deselect2();
                    mascotita = false;
        }
        else{
            Deselect2();
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
        if(ultimoReconocidoRecoger){
            TextDetectDragable.SetActive(true);
        }else{
            TextDetectDragable.SetActive(false);
        }
    }
}
