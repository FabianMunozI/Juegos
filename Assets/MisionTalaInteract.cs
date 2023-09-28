using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionTalaInteract : Interactable
{
    [HideInInspector]
    public GameObject CanvasMisionTala;

    [HideInInspector]
    public GameObject preguntarMision;

    [HideInInspector]
    public GameObject NoHayMision;
    MisionTala misionTalaScript;

    public GameObject[] ListaElementosActivar;

    GameObject player;
    public bool bandera= true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        misionTalaScript = GetComponent<MisionTala>();
        CanvasMisionTala = GameObject.Find("MisionTala");
        preguntarMision = CanvasMisionTala.transform.GetChild(0).gameObject;
        NoHayMision = CanvasMisionTala.transform.GetChild(1).gameObject;
    }

     public override void Interact()
    {
        base.Interact();
        if(bandera){
            preguntarMision.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            ScriptsPlayer(false);
        }else{
            NoHayMision.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            ScriptsPlayer(false);
        }
        

    }

    public void iniciarMisionTala(){
        for(int i=0; i< ListaElementosActivar.Length; i++){
            ListaElementosActivar[i].SetActive(true);
        }

        misionTalaScript.enabled=true;
    }

    public void BloquearCursor(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    

    public void ScriptsPlayer(bool cambiarA){
        //player.GetComponent<CameraInteraction>().enabled = cambiarA;
        player.GetComponent<FpsCamera>().enabled = cambiarA;
        player.GetComponent<AbrirMapa>().enabled = cambiarA;
        player.GetComponent<CharacterMovement>().enabled = cambiarA;
    }

    public void DesactivarInteract(){
        enabled=false;
        GetComponent<Collider>().enabled=false;
    }

    public void cambiarFuncInteract(){
        bandera = false;
    }
}
