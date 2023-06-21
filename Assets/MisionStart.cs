using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionStart : MonoBehaviour
{

    public GameObject objetosActivar;

    public GameObject BasuraActivar;


    public Material color1;
    public Material color2;

    bool estaSobreLaPlataforma = false;

    public GameObject activarPreguntaParaIniciarMision;

    GameObject Jugador;

    public Animator controller;

    public GameObject referenciaExclamacion;

    public bool inputTrue = false;

    Vector3 posCamara;

    GameObject CamaraO;
    Vector3 VectorPartida;

    bool eActivado;

    ///////////////////////////Desde Aki variables para la mision en si

    public int Vidas = 1;

    public int ObjBienPuestos = 0;

    int target = 2;

    //////////////////////////////////////////////


    // Start is called before the first frame update
    void Start()
    {
        Jugador= GameObject.Find("Player");
        CamaraO = GameObject.Find("Camera");
        eActivado = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(estaSobreLaPlataforma){
            if(Input.GetKey(KeyCode.E) && !eActivado){
                eActivado=true;
                OnOffPlayer();  // activa o desactiva los controles del player

                VectorPartida = CamaraO.transform.position;
                
                if(Jugador.GetComponent<CharacterMovement>().EstaParado==false){
                    CamaraO.transform.position= Jugador.transform.position + Jugador.GetComponent<CharacterMovement>().InitialPos;
                    Jugador.GetComponent<CharacterMovement>().EstaParado=true;
                }
                
                Jugador.transform.position = new Vector3(74, 1 , 42.5f);
                Jugador.transform.rotation = Quaternion.Euler(0,0,0);
                CamaraO.transform.rotation = Quaternion.Euler(0,0,0);

                posCamara = CamaraO.transform.position; // guardo la pos inicial de la camara respecto al padre

                BasuraActivar.SetActive(true);
                objetosActivar.SetActive(true);
                controller.SetTrigger("Start");
                activarPreguntaParaIniciarMision.SetActive(false);

                

                Invoke("OnOffPlayer", 8f);

            }
        }

        if(Vidas==0){
            Debug.Log("perdiste");
        }

        if(ObjBienPuestos==target){
            Debug.Log("Completaste Mision");
        }

        //falta el timer = 0
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && !eActivado){
            this.gameObject.GetComponent<MeshRenderer>().material.color = color1.color;
            estaSobreLaPlataforma = true;
            activarPreguntaParaIniciarMision.SetActive(true);
        } // quiza podemos añadir mision en curso o algo asi 
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player") && !eActivado){
            this.gameObject.GetComponent<MeshRenderer>().material.color = color2.color;
            estaSobreLaPlataforma = false;
            activarPreguntaParaIniciarMision.SetActive(false);
        }
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
            controller.SetTrigger("End");
            //Debug.Log("volvieron los controles");
            referenciaExclamacion.SetActive(false);
            this.GetComponent<MeshRenderer>().material.color=Color.green;
            estaSobreLaPlataforma = false;
            
            
        }

    }


}
