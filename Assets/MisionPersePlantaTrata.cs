using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MisionPersePlantaTrata : Interactable
{
    public GameObject textos;

    public GameObject player;

    GameObject respaldo;

    bool primeraVez;

    public Transform posInicial;
    public GameObject referenciaLookAt;

    public Transform posMedio;
    public GameObject referenciaLookAtMedio;

    public Transform posFinal;
    public GameObject referenciaLookAtFinal;

    public Transform posInicial2;
    public GameObject referenciaLookAt2;

    public Transform posFinal2;
    public GameObject referenciaLookAtFinal2;

    bool volvioMisionMapa;
    bool banderasa;

    public GameObject npcActivarInfo;
    // Start is called before the first frame update
    void Start()
    {
        player= GameObject.Find("Player");

        textos = GameObject.Find("CanvasMisionesMTratar"); // hijos son los canvas perse

        if (PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            //Debug.Log(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua"));
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=4){
                //Debug.Log("entre akiaaa");
                banderasa=true;
                // activar NPC postMision
                enabled=false;
                //Debug.Log("aka");
                npcActivarInfo.SetActive(true);
                for(int i =0; i<21; i++){
                    gameObject.transform.GetChild(i).gameObject.SetActive(false);
                }
                gameObject.GetComponent<BoxCollider>().enabled=false;
                enabled=false;
                //desactivar este script y todos los textos porsiacaso
            }
            else if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=3){
                textos.transform.GetChild(4).gameObject.SetActive(true);
                //Debug.Log("aka2");
            }
            else if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=2){
                Debug.Log("aquiaas");
                textos.transform.GetChild(3).gameObject.SetActive(true);

                
            }
            else if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=1){
                textos.transform.GetChild(2).gameObject.SetActive(true);

                
            }
        }    
    }

    // Update is called once per frame
    void Update()
    {
        /* if (!PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            //activar pregunta mision
            textos.transform.GetChild(0).gameObject.SetActive(true);
        } */
        if (!banderasa){
            if (PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            //Debug.Log(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua"));
            /* if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=2){
                textos.transform.GetChild(3).gameObject.SetActive(true); // volver con el player objetivos

            }  */
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=3  && !volvioMisionMapa ){ // volver para terminar mision
                textos.transform.GetChild(4).gameObject.SetActive(true);
                // volver con el player
            }else{
                //termino la mision
                //textos.transform.GetChild(3).gameObject.SetActive(false);


                //desactivar collider, mesh de este char, activar otro char que tenga la info final


            }/* else if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=3){
                
            } */
        }
        }
        
    }

    public override void Interact(){ // preguntar mision
        base.Interact();

        if (!PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua") && !primeraVez){
            //activar pregunta mision
            textos.transform.GetChild(0).gameObject.SetActive(true);
            scriptsPlayer(false);
            
        }else if(!PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua") && primeraVez){
            player.transform.position = posFinal.position;
            player.transform.LookAt(referenciaLookAtFinal.transform);

            Invoke("primeraParte",0.03f); // cañeria
            Invoke("moverMedio",2.5f); // puerta para entrar no sea weon
            Invoke("moverFinal",5f); // volver pos inicial?
        }
        else if (PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            //Debug.Log(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua"));
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=3){
                PlayerPrefs.SetInt("PalancaPlantaTratamientoAgua", 4);
                PlayerPrefs.Save();
                volvioMisionMapa = true;
                textos.transform.GetChild(1).gameObject.SetActive(false);
                textos.transform.GetChild(2).gameObject.SetActive(false); 
                textos.transform.GetChild(3).gameObject.SetActive(false); 
                textos.transform.GetChild(4).gameObject.SetActive(false); 

                player.transform.position = posFinal2.position;
                player.transform.LookAt(referenciaLookAtFinal2.transform);

                Invoke("primeraParte2",0.1f);
                Invoke("moverFinal2",3f);

                //activar anim

                // desactivar este collider, activar char interno con interact de info
                //
            }
        }
    }

    public void PrimerVez(){
        primeraVez=true;
    }

    public void scriptsPlayer(bool nuevoV){
        player.GetComponent<Mascota>().enabled=nuevoV;
        player.GetComponent<CameraInteraction>().enabled=nuevoV;
        player.GetComponent<FpsCamera>().enabled=nuevoV;
        player.GetComponent<CharacterMovement>().enabled=nuevoV;
        

        if(nuevoV){
            
            Cursor.lockState= CursorLockMode.Locked;
        }

        if(!nuevoV){
            Cursor.lockState= CursorLockMode.None;
        }
        player.GetComponent<AbrirMapa>().enabled=nuevoV;
        player.GetComponent<Rigidbody>().isKinematic= !nuevoV;


    }


    void moverFinal(){
        player.transform.position = posFinal.position;
        player.transform.LookAt(referenciaLookAtFinal.transform);
        scriptsPlayer(true);
        textos.transform.GetChild(1).gameObject.SetActive(true);
        /* PlayerPrefs.SetInt("PalancaPlantaTratamientoAgua", 3);
        PlayerPrefs.Save(); */
    }

    void moverFinal2(){
        player.transform.position = posFinal2.position;
        player.transform.LookAt(referenciaLookAtFinal2.transform);
        scriptsPlayer(true);
        
        //activar npc con informacion, desactivar este sccript, desactivar los canvas
        npcActivarInfo.SetActive(true);
        /* npcActivarInfo.GetComponent<ActivarDialogo>().dialogoObjetivo.SetActive(true); */

        /* gameObject.GetComponent<MeshRenderer>().enabled=false; */
        for(int i =0; i<19; i++){
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        gameObject.GetComponent<BoxCollider>().enabled=false;
        enabled=false;
        gameObject.GetComponent<BoxCollider>().enabled=false;
        enabled=false;

        /* PlayerPrefs.SetInt("PalancaPlantaTratamientoAgua", 3);
        PlayerPrefs.Save(); */
    }

    void moverMedio(){
        player.transform.position = posMedio.position;
        player.transform.LookAt(referenciaLookAtMedio.transform);
        //scriptsPlayer(true);

        
        /* PlayerPrefs.SetInt("PalancaPlantaTratamientoAgua", 3);
        PlayerPrefs.Save(); */
    }

    void primeraParte(){
        player.transform.position = posInicial.position;
        scriptsPlayer(false);
        player.transform.LookAt(referenciaLookAt.transform);
        /* segundoPuzzle=true; */
    }

    void primeraParte2(){
        player.transform.position = posInicial2.position;
        scriptsPlayer(false);
        player.transform.LookAt(referenciaLookAt2.transform);
        /* segundoPuzzle=true; */
    }

}
