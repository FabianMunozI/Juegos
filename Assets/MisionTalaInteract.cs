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
    public GameObject FinMision;
    MisionTala misionTalaScript;

    public GameObject[] ListaElementosActivar;

    GameObject player;
    public bool bandera= true;
    public bool bandera2EntregarMision= false;

    public GameObject NpcInfoTala;
    GameObject activarObjetoCantArboles;
    // Start is called before the first frame update
    void Start()
    {
        NpcInfoTala= GameObject.Find("NpcInfoTala");

        player = GameObject.Find("Player");
        misionTalaScript = GetComponent<MisionTala>();
        CanvasMisionTala = GameObject.Find("MisionTala");
        preguntarMision = CanvasMisionTala.transform.GetChild(0).gameObject;
        NoHayMision = CanvasMisionTala.transform.GetChild(1).gameObject;

        FinMision= CanvasMisionTala.transform.GetChild(5).gameObject;

        activarObjetoCantArboles = CanvasMisionTala.transform.GetChild(7).gameObject;
    }

     public override void Interact()
    {
        base.Interact();
        if(bandera){
            preguntarMision.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            ScriptsPlayer(false);
        }else if(bandera2EntregarMision){
            Invoke("npcGenerarFin",0.25f);
            misionTalaScript.MisionTalaObject.transform.GetChild(4).gameObject.SetActive(false);
            gameObject.SetActive(false);

            activarObjetoCantArboles.SetActive(true);
            activarObjetoCantArboles.GetComponent<scriptCantArboles>().cantidad = misionTalaScript.tiempoMision/3;
        }
        else{
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

    void npcGenerarFin(){
        NpcInfoTala.transform.GetChild(0).gameObject.SetActive(true);
    }
}
