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

    public GameObject TextDetectDialogue;
    GameObject ultimoReconocidoDialogue=null;


    public GameObject TextDetectBasura;
    public GameObject ultimoReconocidoBasura=null;

    GameObject Activar;

    [SerializeField] public InventoryItem itemPrefab;
    private GameObject Iv;

    static public bool interactionDialogue = false;

    GameObject mascota;
    // Start is called before the first frame update
    void Start()
    {
        mascota = GameObject.Find("CMascota").transform.GetChild(0).gameObject;
        cameraa = transform.Find("Camera");
        Iv = GameObject.Find("Inventario");

        //TextDetectInteractable = transform.Find("Interactuar").gameObject;
        //TextDetectDragable = transform.Find("Recoger").gameObject;
        /*
        TextDetectInteractable.SetActive(false);
        TextDetectDragable.SetActive(false); */
    }

    // Update is called once per frame
    void Update()
    {
        if(interactionDialogue){
            Deselect();
            Deselect2();
            Deselect3();
            Deselect4();
        }
        else{
            Debug.DrawRay(cameraa.position, cameraa.forward * rayDistance, Color.red);
            RaycastHit hit1;
            if (Physics.Raycast(cameraa.position, cameraa.forward, out hit1, rayDistance, LayerMask.GetMask("interactable"))){
                Deselect();
                SelectedObjet(hit1.transform);
                if(Input.GetKeyDown(KeyCode.E)){ // letra e
                    hit1.transform.GetComponent<Interactable>().Interact();
                }
            }else{
                Deselect();
            }
            ///////////////////////////////////////////////////////////////////////////////
            RaycastHit hit2;
            if (Physics.Raycast(cameraa.position, cameraa.forward, out hit2, rayDistance, LayerMask.GetMask("grabable")) && this.GetComponent<PickUpObjects>().PickedObjectMascota == null && !GetComponent<Mascota>().eresLaMascota){
                Deselect2();
                if(this.GetComponent<PickUpObjects>().PickedObject ==null){
                    SelectedObjet2(hit2.transform);
                }else{
                    Deselect2();
                    /* if(Input.GetKeyDown(KeyCode.F)){
                        GetComponent<PickUpObjects>().PickedObject.transform.GetComponent<Interactable>().Interact();
                    } */
                }
                if(Input.GetKeyDown(KeyCode.F) && this.GetComponent<PickUpObjects>().PickedObject == null && this.GetComponent<PickUpObjects>().PickedObjectMascota == null && mascota.GetComponent<mascotaInteraction>().mascotita==false && !GetComponent<Mascota>().eresLaMascota){ //agarrar objeto
                    //Debug.Log(this.GetComponent<PickUpObjects>().PickedObjectMascota);
                    hit2.transform.GetComponent<Interactable>().Interact();

                    if(this.GetComponent<PickUpObjects>().PickedObject != null)
                    {
                        if (this.GetComponent<PickUpObjects>().PickedObject.GetComponent<VincularObjetoInventario>() != null)
                        {

                            if (this.GetComponent<PickUpObjects>().PickedObject.GetComponent<VincularObjetoInventario>().item_vinculado != null)
                            {
                                Iv.GetComponent<Inventory>().SpawnHotBarItem(this.GetComponent<PickUpObjects>().PickedObject.GetComponent<VincularObjetoInventario>().item_vinculado);
                            } else {
                                Debug.Log("WARNING: OBJETO NO TIENE ITEM ASIGNADO");
                                Iv.GetComponent<Inventory>().SpawnHotBarItem();
                            }

                        }
                    }
                }
                else if(Input.GetKeyDown(KeyCode.F) && GetComponent<PickUpObjects>().PickedObject != null) {  //soltar objeto
                    Debug.Log("b");
                    GetComponent<PickUpObjects>().PickedObject.transform.GetComponent<Interactable>().Interact();
                    Iv.GetComponent<Inventory>().SoltarObjeto();
                }
            }else if(Input.GetKeyDown(KeyCode.F) && GetComponent<PickUpObjects>().PickedObject != null) {  //soltar objeto
                        Debug.Log("a");
                        GetComponent<PickUpObjects>().PickedObject.transform.GetComponent<Interactable>().Interact();
                        Deselect2();
                        Iv.GetComponent<Inventory>().SoltarObjeto();
            }
            else{
                Deselect2();
            }

            ///////////////////////////////////////////////////////////////////////////////
            RaycastHit hit3;
            if (Physics.Raycast(cameraa.position, cameraa.forward, out hit3, rayDistance, LayerMask.GetMask("dialogable"))){
                Deselect3();
                SelectedObjet3(hit3.transform);
                if(Input.GetKeyDown(KeyCode.E)){ // letra e
                    hit3.transform.GetComponent<Interactable>().Interact();
                }
            }else{
                Deselect3();
            }
            ///////////////////////////////////////////////////////////////////////////////
            RaycastHit hit4;
            if (Physics.Raycast(cameraa.position, cameraa.forward, out hit4, rayDistance, LayerMask.GetMask("basurero")) && this.GetComponent<PickUpObjects>().PickedObject!=null && GetComponent<PickUpObjects>().PickedObject.CompareTag("basura")){
                SelectedObjet4(hit4.transform);
            }
            else{
                Deselect4();
            }

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

    void SelectedObjet3(Transform transform){
        ultimoReconocidoDialogue = transform.gameObject;
    }
    void Deselect3(){
        if(ultimoReconocidoDialogue){
            ultimoReconocidoDialogue = null;
        }
    } 

    void SelectedObjet4(Transform transform){
        ultimoReconocidoBasura = transform.gameObject;
    }

    void Deselect4(){
        if(ultimoReconocidoBasura){
            ultimoReconocidoBasura = null;
        }
    }

    void OnGUI() {
        if (ultimoReconocido is not null)
        {
            if (ultimoReconocido)
            {
                TextDetectInteractable.SetActive(true);
            }
            else
            {
                TextDetectInteractable.SetActive(false);
            }
        }

        if (ultimoReconocidoRecoger is not null)
        {
            if (ultimoReconocidoRecoger)
            {
                TextDetectDragable.SetActive(true);
            }
            else
            {
                TextDetectDragable.SetActive(false);
            }
        }

        if (ultimoReconocidoDialogue is not null)
        {
            if (ultimoReconocidoDialogue)
            {
                TextDetectDialogue.SetActive(true);
            }
            else
            {
                TextDetectDialogue.SetActive(false);
            }
        }
        
        if (ultimoReconocidoBasura is not null)
        {
            if (ultimoReconocidoBasura)
            {
                TextDetectBasura.SetActive(true);
            }
            else
            {
                TextDetectBasura.SetActive(false);
            }
        }
    }
}


