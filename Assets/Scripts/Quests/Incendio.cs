using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Incendio : MonoBehaviour
{
    private bool Quest_started;
    public Material color1;
    public Material color2;
    private GameObject Llamas;
    public TextMeshProUGUI quest_text;
    private int n_llamas;
    public GameObject Balde_Agua;
    public GameObject Pozo;

    public Vector3 cam_pos;
    public Vector3 playerrot;
    public Vector3 camrot;

    // Camara
    GameObject Jugador;
    Vector3 posCamara;
    GameObject CamaraO;
    Vector3 VectorPartida;
    public bool inputTrue = false;
    public Animator controller;

    public GameObject Guardabosque;

    bool missionDone = false;

    public GameObject fogata;

    void Start()
    {
        Quest_started = false;
        Llamas = GameObject.Find("Llamas");
        Llamas.SetActive(false);
        quest_text.enabled = false;
        n_llamas = Llamas.transform.childCount;

        Balde_Agua = GameObject.Find("Wooden_Bucket");
        Balde_Agua.GetComponent<Outline>().enabled = false;

        Pozo = GameObject.Find("SM_Bld_Well_01");
        Pozo.GetComponent<Outline>().enabled = false;

        Jugador= GameObject.Find("Player");
        CamaraO = GameObject.Find("Camera");

        fogata= GameObject.Find("Plataforma");
    }

    void Update()
    {

        int llamas_restantes = counter_actives(Llamas);
        if (Quest_started)
        {
            quest_text.text = "¡Apaga los Arboles!\n- "+(n_llamas - llamas_restantes )+"/"+n_llamas+" Arboles Apagados";
        }


        if (!Quest_started && !(missionDone) && Input.GetKeyDown(KeyCode.E) && fogata.GetComponent<activarMisionBosque1>().misionActivada){

            OnOffPlayer();
            VectorPartida = CamaraO.transform.position;

            if(Jugador.GetComponent<CharacterMovement>().EstaParado==false){
                CamaraO.transform.position= Jugador.transform.position + Jugador.GetComponent<CharacterMovement>().InitialPos;
                Jugador.GetComponent<CharacterMovement>().EstaParado=true;
            }else{

            }

            Jugador.transform.position = new Vector3(7,1,20);
            Jugador.transform.rotation = Quaternion.Euler(0, -93.019f,0);
            //CamaraO.transform.position = new Vector3(0,0.69f,0);
            //CamaraO.transform.rotation = Quaternion.Euler(0,0,0);
            CamaraO.transform.rotation=Jugador.transform.rotation;

            posCamara = CamaraO.transform.position;

            controller.SetTrigger("Start2");

            Invoke("OnOffPlayer", 14.5f);

            Quest_started = true;
            Llamas.SetActive(true);
            quest_text.enabled = true;
            Balde_Agua.GetComponent<Outline>().enabled = true;
            Pozo.GetComponent<Outline>().enabled = true;
        }

        if (llamas_restantes == 0)
        {
            missionDone = true;
            quest_text.text = "¡Mision Completada!";
            Quest_started = false;
            Guardabosque.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            this.gameObject.GetComponent<MeshRenderer>().material.color = color2.color;
        }
    }

    private int counter_actives(GameObject currentObj)
    {
        int count = 0;
        for (int i = 0; i < currentObj.transform.childCount; i++)
        {
            if(currentObj.transform.GetChild(i).gameObject.activeSelf)
            count++;
        }
        return count;
    }


    public void OnOffPlayer(){
        inputTrue = !inputTrue; // se vuelve a true el valor // inicia false en el start
        //Jugador.GetComponent<Rigidbody>().isKinematic = inputTrue;
        Jugador.GetComponent<CharacterMovement>().AnimacionOn = !inputTrue; // frezea al player mientras esta la animacion
        Jugador.GetComponent<FpsCamera>().animacionOn = !inputTrue;
        //BasuraActivar.SetActive(inputTrue); // activa y desactiva la basura

        if(inputTrue==false){ // si ya se inicio la mision y se esta terminando / devolviendo controles al player
        // hay que desactivar que la mision pueda ser tomada
            //referenciaExclamacion.SetActive(inputTrue);
            controller.SetTrigger("End2");
            //Debug.Log("volvieron los controles");
            // referenciaExclamacion.SetActive(false);
            this.GetComponent<MeshRenderer>().material.color=Color.green;
            
            
        }

    }

}
