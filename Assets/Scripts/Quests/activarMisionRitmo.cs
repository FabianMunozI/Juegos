using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class activarMisionRitmo : Interactable
{
    // Start is called before the first frame update
    public GameObject MisionRitmo;

    GameObject referenciaInicioRitmo;
    GameObject player;

    GameObject CScenarioRef;

    GameObject refAlcalde;

    GameObject refPosCamAlcalde;

    GameObject microfonoRef;
    bool activarUpdate;
    float distanceCamPos;

    Vector3 initialDistance;

    bool segUpdate;

    public GameObject NpcGenerar;

    public GameObject PadrePosicionesGenerarNPCs;

    List<GameObject> NpcsGenerados0;
    List<GameObject> NpcsGenerados1;
    List<GameObject> NpcsGenerados2;
    List<GameObject> NpcsGenerados3;

    int listaPreferenciaEliminar;

    int posGenerar;
    bool sellamoAutoridad;

    void Start()
    {
        sellamoAutoridad = false;
        listaPreferenciaEliminar = 0;

        NpcsGenerados0 = new List<GameObject>();
        NpcsGenerados1 = new List<GameObject>();
        NpcsGenerados2 = new List<GameObject>();
        NpcsGenerados3 = new List<GameObject>();
        
        posGenerar = 0;
        PadrePosicionesGenerarNPCs = transform.GetChild(24).gameObject;


        referenciaInicioRitmo = GameObject.Find("PosPlayerInicialRitmo");
        MisionRitmo = GameObject.Find("Critmo");
        MisionRitmo.transform.GetChild(0).GetChild(0).GetComponent<RitmoTeclas>().referenciaScriptIniciarMisionRitmo= this;

        CScenarioRef = GameObject.Find("CScenario");

        refAlcalde = CScenarioRef.transform.GetChild(0).GetChild(0).gameObject;
        refPosCamAlcalde = CScenarioRef.transform.GetChild(0).GetChild(1).gameObject;

        player = GameObject.Find("Player");
        microfonoRef = GameObject.Find("Podio").transform.GetChild(0).gameObject;
    }

    void Update(){
        if(activarUpdate){
            player.transform.position += initialDistance * 1/distanceCamPos * Time.deltaTime * 4;

            Vector3 direct = (refAlcalde.transform.position - player.transform.position).normalized;
            Quaternion rotGoal = Quaternion.LookRotation(direct);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotGoal, 20f *Time.deltaTime);

            if(Vector3.Distance(player.transform.position, refPosCamAlcalde.transform.position) <=0.5){
                activarUpdate=false;
                Invoke("activarInvoke2daAnimacion",1f);
                distanceCamPos = (Vector3.Distance(player.transform.position, microfonoRef.transform.GetChild(3).transform.position)) /* / 4f */; // distancia dividido tiempo
                initialDistance = microfonoRef.transform.GetChild(3).transform.position - player.transform.position;
            } 
        }

        if(segUpdate){
            player.transform.position += initialDistance * 1/distanceCamPos * Time.deltaTime * 4;

            Vector3 direct = (microfonoRef.transform.parent.GetChild(2).transform.position - player.transform.position).normalized;
            Quaternion rotGoal = Quaternion.LookRotation(direct);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotGoal, 2f *Time.deltaTime);

            if(Vector3.Distance(player.transform.position, microfonoRef.transform.GetChild(3).transform.position) <=0.5){
                segUpdate = false;
                player.GetComponent<Collider>().isTrigger = false;
                player.GetComponent<Rigidbody>().isKinematic = false;

                // prender objetivos mision
                MisionRitmo.transform.GetChild(2).gameObject.SetActive(true);
                MisionRitmo.transform.GetChild(3).gameObject.SetActive(true);

            }
        }

    }

    public override void Interact(){ // preguntar mision
        base.Interact();
        MisionRitmo.transform.GetChild(1).gameObject.SetActive(true); // aceptar o rechazar mision
        Cursor.lockState = CursorLockMode.Confined;
        SetActivePlayerScripts(false);

    }

    public void iniciarMision(){
        // ACTIVAR POLITICO, Y NPCS
        CScenarioRef.transform.GetChild(0).gameObject.SetActive(true);

        ///////// paneo de camara suave
        
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Collider>().isTrigger = true;

        //posiciona al player para iniciar animacion bien,
        player.transform.position = referenciaInicioRitmo.transform.position;
        Vector3 targetPosition = transform.position;
        targetPosition.y = player.transform.position.y; 
        player.transform.LookAt(targetPosition); 
        player.transform.GetChild(0).GetComponent<Camera>().transform.rotation = referenciaInicioRitmo.transform.rotation;
        

        distanceCamPos = (Vector3.Distance(player.transform.position, refPosCamAlcalde.transform.position)) /* / 4f */; // distancia dividido tiempo
        initialDistance = refPosCamAlcalde.transform.position - player.transform.position;
        activarUpdate = true;


        MisionRitmo.transform.GetChild(1).gameObject.SetActive(false); 

        //////// desactivar mensaje de interaccion para
        GameObject.Find("Interactuar").SetActive(false); 
    }
    public void noIniciar(){
        MisionRitmo.transform.GetChild(1).gameObject.SetActive(false);
        SetActivePlayerScripts(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void activarInvoke2daAnimacion(){
        segUpdate = true;
    }

    void SetActivePlayerScripts(bool valor){
        player.GetComponent<AbrirMapa>().enabled = valor;
        player.GetComponent<Mascota>().enabled = valor;
        player.GetComponent<CharacterMovement>().enabled = valor;
        player.GetComponent<FpsCamera>().enabled = valor;
        player.GetComponent<CameraInteraction>().enabled = valor;
    }
    
    public void llamarNpc(){
        GameObject a = Instantiate(NpcGenerar, PadrePosicionesGenerarNPCs.transform.GetChild(posGenerar));
        a.GetComponent<NPCmov>().posGenerado = posGenerar;
        a.GetComponent<NPCmov>().posGenObj = PadrePosicionesGenerarNPCs.transform.GetChild(posGenerar).gameObject;

        if(posGenerar==0){
            NpcsGenerados0.Add(a);

        }else if(posGenerar == 1 ){
            NpcsGenerados1.Add(a);

        }else if(posGenerar == 2 ){
            NpcsGenerados2.Add(a);

        }else if(posGenerar == 3 ){
            NpcsGenerados3.Add(a);
        }
            

        posGenerar += 1;
        if(posGenerar>3){
            posGenerar=0;
        }

    }

    public void quitarNpc(){
        List<GameObject> nuevaLista;

        if(listaPreferenciaEliminar==0){
            nuevaLista = NpcsGenerados0;

        }else if(listaPreferenciaEliminar == 1 ){
            nuevaLista = NpcsGenerados1;

        }else if(listaPreferenciaEliminar == 2 ){
            nuevaLista = NpcsGenerados2;

        }else if(listaPreferenciaEliminar == 3 ){
            nuevaLista = NpcsGenerados3;
        }else{
            nuevaLista = new List<GameObject>();
            Debug.Log("error funcion quitar NPC");
        }

        

        bool bandera = true;

        for(int i=0; i< nuevaLista.Count; i++){

            if(nuevaLista[i] != null){  // destruir NPC y hacer que los otros caminen, tambien quitar NPC de la lista
                
                Destroy(nuevaLista[i]);
                
                caminarAgainNpc(listaPreferenciaEliminar);

                nuevaLista.Remove(nuevaLista[i]);

                bandera = false;
                break;
            }

        }

        listaPreferenciaEliminar+=1;
        if(listaPreferenciaEliminar>3){
            listaPreferenciaEliminar = 0;
        }

        if(bandera){
            Debug.Log("hubo un error en elimnar NPC's");
        }

    }

    void caminarAgainNpc(int listNum){
        List<GameObject> Reiniciar;

        if(listNum==0){
            Reiniciar = NpcsGenerados0;

        }else if(listNum == 1 ){
            Reiniciar = NpcsGenerados1;

        }else if(listNum == 2 ){
            Reiniciar = NpcsGenerados2;

        }else if(listNum == 3 ){
            Reiniciar = NpcsGenerados3;
        }else{
            Reiniciar = new List<GameObject>();
            Debug.Log("Error hacer caminar NPC again");
        }
        
        for(int i = 1; i < Reiniciar.Count; i++){

            if(!Reiniciar[i].GetComponent<NPCmov>().primeraFase){
                Reiniciar[i].GetComponent<NPCmov>().segundaFase=true;
                Reiniciar[i].GetComponent<NPCmov>().quedarQuieto=false;
                Reiniciar[i].GetComponent<Collider>().isTrigger = true;
                Reiniciar[i].GetComponent<Animator>().SetTrigger("caminar");
            }

        }

    }

    public void llamarAutoridad(){

        if(!sellamoAutoridad){
            GameObject Autoridad = transform.GetChild(25).GetChild(0).GetChild(0).gameObject;
            Autoridad.GetComponent<MovAutoridad>().enabled = true;

            sellamoAutoridad = true;
        }

    }

}
