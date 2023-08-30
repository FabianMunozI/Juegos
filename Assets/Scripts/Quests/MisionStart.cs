using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MisionStart : MonoBehaviour
{

    public GameObject objetosActivar;

    public GameObject BasuraActivar;

    public GameObject NpcActivar;

    public GameObject NpcCompletadoActivar;

    ////////////////////////////////////////////

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

    public int Vidas = 2;
    public int vidasTotales;

    public int ObjBienPuestos = 0;

    int target = 2;

    //////////////////////////////////////////////

    public GameObject referenciaCanvas;
    public Text objReciclados;

    public Text TextVidas;

    public Text tiempoReferenciaTexto;

    public bool TiempoPerdiste=false;

    public bool yaPerdio=false;
    public bool yaGano=false;

    bool todoListo;

    // Start is called before the first frame update
    void Start()
    {
        todoListo = false;
        vidasTotales=Vidas;
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

                

                NpcActivar.SetActive(true);
                BasuraActivar.SetActive(true);
                //objetosActivar.SetActive(true);
                controller.SetTrigger("Start");
                activarPreguntaParaIniciarMision.SetActive(false); // desactivas mensaje de iniciar mision

                for(int i =0; i< objetosActivar.transform.childCount; i++){
                    objetosActivar.transform.GetChild(i).gameObject.GetComponent<Outline>().enabled=true;
                }

                Invoke("OnOffPlayer", 8f);

            }
        }

        if((Vidas==0 || TiempoPerdiste) && yaPerdio==false){ //o si el tiempo = 0 



            referenciaCanvas.transform.GetChild(1).gameObject.SetActive(false);
            referenciaCanvas.transform.GetChild(2).gameObject.SetActive(false);
            referenciaCanvas.transform.GetChild(3).gameObject.SetActive(false);
            
            referenciaCanvas.transform.GetChild(4).gameObject.SetActive(true);
        }

        if(yaPerdio==true || yaGano==true){
            GameObject pickedPlayer = Jugador.GetComponent<PickUpObjects>().PickedObject;
            if(pickedPlayer != null){
                if(pickedPlayer.CompareTag("basura")){
                    pickedPlayer.GetComponent<Interactable>().Interact();
                    pickedPlayer.SetActive(false);
                }
            }
            BasuraActivar.SetActive(false);
            NpcCompletadoActivar.SetActive(true);

            referenciaCanvas.SetActive(false);

            for(int i =0; i< objetosActivar.transform.childCount; i++){
                objetosActivar.transform.GetChild(i).gameObject.GetComponent<Outline>().enabled=false;
            }
            //desactivar todo
            GetComponent<MisionStart>().enabled=false;
        }

        if(ObjBienPuestos==target && !todoListo){
            todoListo=true;
            referenciaCanvas.transform.GetChild(1).gameObject.SetActive(false);
            referenciaCanvas.transform.GetChild(2).gameObject.SetActive(false);
            referenciaCanvas.transform.GetChild(3).gameObject.SetActive(false);
            BasuraActivar.SetActive(false);

            referenciaCanvas.transform.GetChild(5).gameObject.SetActive(true);
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
        if(inputTrue ==true){
            for(int i = 0; i<BasuraActivar.transform.childCount; i++){
                for(int x=0; x < BasuraActivar.transform.GetChild(i).childCount; x++){

                    BasuraActivar.transform.GetChild(i).GetChild(x).gameObject.GetComponent<Outline>().enabled = true;
                    
                }
                
            }
        }
        Jugador.GetComponent<CharacterMovement>().AnimacionOn = !inputTrue; // frezea al player mientras esta la animacion
        Jugador.GetComponent<FpsCamera>().animacionOn = !inputTrue;
        
        
        //BasuraActivar.SetActive(inputTrue); // activa y desactiva la basura

        if(inputTrue==false){ // si ya se inicio la mision y se esta terminando / devolviendo controles al player
        // hay que desactivar que la mision pueda ser tomada
            //referenciaExclamacion.SetActive(inputTrue);
            referenciaCanvas.SetActive(true);
            string texto = "- Objetos Recolectados :" + ObjBienPuestos.ToString("0") + " / " + target.ToString("0");
            objReciclados.text = texto;
            string texto2 = "- Intentos Restantes: " + Vidas.ToString("0") + " / "+ vidasTotales.ToString("0") ;
            TextVidas.text = texto2;

            controller.SetTrigger("End");
            //Debug.Log("volvieron los controles");
            referenciaExclamacion.SetActive(false);
            this.GetComponent<MeshRenderer>().material.color=Color.green;
            estaSobreLaPlataforma = false;
            
            Invoke("NpcDesactivar", 22f);
            
            
            
        }

    }

    public void actualizarTextRecolectados(){
        string texto = "- Objetos Recolectados :" + ObjBienPuestos.ToString("0") + " / " + target.ToString("0");
        objReciclados.text = texto;
    }

    public void actualizarTextVidas(){
        string texto = "- Intentos Restantes: " + Vidas.ToString("0") + " / "+ vidasTotales.ToString("0") ;
        TextVidas.text = texto;
    }

    public void NpcDesactivar(){
        NpcActivar.SetActive(false);
    }

}
