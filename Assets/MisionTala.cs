using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionTala : MonoBehaviour
{
    GameObject Player;
    public bool volverCheckPoint;

    public bool permisoUno;
    public bool permisoDos;
    public bool permisoTres;

    public GameObject permiso1;
    public GameObject permiso2;
    public GameObject permiso3;

    public Transform[] checkPoints;

    public float tiempoMision; // cantidad arboles cortados Ilegalmente/ sin permisos

    bool animacionStart;
    Vector3 initialDistance;
    float distanceCamPos;

    public bool animacionStart2;
    Vector3 initialDistance2;
    public float distanceCamPos2;

    public GameObject teEstanBuscandoObject;

    public GameObject[] AnimacionElems;
    public GameObject[] AnimacionElems2;

    GameObject UnoAnim;
    GameObject DosAnim;

    MisionTalaInteract misionTalaInteract;
    bool pistasEnter;
    int i =0;
    int ii=0;
    GameObject pistas;
    bool banderaScriptsPlayer;

    bool ObjetivosBandera;
    public GameObject Objetivos;
    public GameObject MisionTalaObject;

    public float tiempoParar;
    bool banderaObjetivoDos;

    public GameObject Rutas2;
    public GameObject NpcPersigue2;
    // Start is called before the first frame update
    void Start()
    {
        

        banderaObjetivoDos = true;
        MisionTalaObject = GameObject.Find("MisionTala");
        ObjetivosBandera = true;

        teEstanBuscandoObject= MisionTalaObject.transform.GetChild(6).gameObject;

        banderaScriptsPlayer=true;
        pistas = MisionTalaObject.transform.GetChild(2).gameObject;
        Objetivos = MisionTalaObject.transform.GetChild(3).gameObject;
        pistasEnter = true;
        Player = GameObject.Find("Player");

        misionTalaInteract = GetComponent<MisionTalaInteract>();
        misionTalaInteract.ScriptsPlayer(false);
        Player.GetComponent<Collider>().isTrigger = true;
        Player.GetComponent<Rigidbody>().isKinematic = true;

        animacionStart = true;
        animacionStart2 = false;
        

        initialDistance = AnimacionElems[i].transform.position - Player.transform.position;

        distanceCamPos = (Vector3.Distance(Player.transform.position, AnimacionElems[i].transform.position));
    }

    // Update is called once per frame
    void Update()
    {
       
        if(animacionStart){
            float multiplicador= 3f;
            if(i >2 && i!=5){
                multiplicador=8;
            }
            Player.transform.position += initialDistance * Time.deltaTime * 1/(distanceCamPos/multiplicador) *5;
        
            Vector3 direct = (AnimacionElems[i].transform.GetChild(0).position - Player.transform.position).normalized;
            Quaternion rotGoal = Quaternion.LookRotation(direct);
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, rotGoal, 8f *Time.deltaTime);
            if(Vector3.Distance(Player.transform.position, AnimacionElems[i].transform.position) <=3){
                i+=1;
                if(i==6){
                    animacionStart=false;
                    Player.transform.position = transform.position - new Vector3(0, 0, 3f);
                    Player.transform.LookAt(transform.position);
                }
                else{
                    distanceCamPos = (Vector3.Distance(Player.transform.position, AnimacionElems[i].transform.position)) /* / 4f */; // distancia dividido tiempo
                    initialDistance = AnimacionElems[i].transform.position - Player.transform.position;
                }
                
            }
            
        }
        else if(!animacionStart){
            if(banderaScriptsPlayer){
                banderaScriptsPlayer = false;
                misionTalaInteract.ScriptsPlayer(true);
                Player.GetComponent<Collider>().isTrigger = false;
                Player.GetComponent<Rigidbody>().isKinematic = false;
            }
            
            tiempoMision += Time.deltaTime;

            if(tiempoMision>=2f && pistasEnter){
                pistasEnter=false;

                Cursor.lockState = CursorLockMode.None;
                pistas.SetActive(true);
                ScriptsPlayer(false);
                Player.GetComponent<FpsCamera>().enabled = false;
            }

            if(Player.transform.position.x<-10 && !pistasEnter && ObjetivosBandera){
                ObjetivosBandera=false;

                //Cursor.lockState = CursorLockMode.None;
                Objetivos.SetActive(true);
                //ScriptsPlayer(false);
                //Player.GetComponent<FpsCamera>().enabled = false;
            }

            if(permisoTres && tiempoMision>tiempoParar && banderaObjetivoDos){
                banderaObjetivoDos = false;

                Objetivos.gameObject.SetActive(false);
                
            }

            if(animacionStart2){
                float multiplicador= 4f;
                if(ii >4){
                    multiplicador=8;
                }
                Player.transform.position += initialDistance2 * Time.deltaTime * 1/(distanceCamPos2/multiplicador) *5;

                Vector3 direct = (AnimacionElems2[ii].transform.GetChild(0).position - Player.transform.position).normalized;
                Quaternion rotGoal = Quaternion.LookRotation(direct);
                Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, rotGoal, 8f *Time.deltaTime);
                if(Vector3.Distance(Player.transform.position, AnimacionElems2[ii].transform.position) <=3){
                    ii+=1;
                    if(ii==4){
                        animacionStart2=false;
                        Player.transform.position = AnimacionElems2[0].transform.parent.GetChild(4).transform.position + new Vector3(0, 0, 3f);
                        Player.transform.LookAt(AnimacionElems2[0].transform.parent.transform.GetChild(4).GetChild(0).transform);
                        Player.GetComponent<Collider>().isTrigger = false;
                        Player.GetComponent<Rigidbody>().isKinematic = false;
                        Player.GetComponent<FpsCamera>().enabled = true;

                        misionTalaInteract.bandera2EntregarMision=true;

                    }
                    else{
                        distanceCamPos2 = (Vector3.Distance(Player.transform.position, AnimacionElems2[ii].transform.position)) /* / 4f */; // distancia dividido tiempo
                        initialDistance2 = AnimacionElems2[ii].transform.position - Player.transform.position;
                    }
                }
            }
        }


    }

    public void VolverCheckPoint(){
        if(permisoTres){ // te agarraron de vuelta en la mision
            /* permisoTres=false;
            permiso3.SetActive(true); */

            Player.transform.position = checkPoints[3].position;
            Player.transform.LookAt(checkPoints[3].GetChild(0).transform);

        }
        else if(permisoDos){
            /* permisoDos=false;
            permiso2.SetActive(true); */
            Player.transform.position = checkPoints[2].position;
            Player.transform.LookAt(checkPoints[2].GetChild(0).transform);
        }
        else if(permisoUno){
            /* permisoUno=false;
            permiso1.SetActive(true); */

            Player.transform.position = checkPoints[1].position;
            Player.transform.LookAt(checkPoints[1].GetChild(0).transform);
        }
        else{
            Player.transform.position = checkPoints[0].position;

            /* Vector3 direct = (checkPoints[0].position - player.transform.position).normalized;
            Quaternion rotGoal = Quaternion.LookRotation(direct); */
            Player.transform.LookAt(checkPoints[0].GetChild(0).transform);

        }

    }

    public void ScriptsPlayer(bool cambiarA){
        //player.GetComponent<CameraInteraction>().enabled = cambiarA;
        Player.GetComponent<FpsCamera>().enabled = cambiarA;
        Player.GetComponent<AbrirMapa>().enabled = cambiarA;
        Player.GetComponent<CharacterMovement>().enabled = cambiarA;
    }

    public void Iniciar2daAnimacion(){
        initialDistance2=AnimacionElems2[ii].transform.position - Player.transform.position;
        distanceCamPos2 = (Vector3.Distance(Player.transform.position, AnimacionElems2[ii].transform.position));
        Player.GetComponent<Collider>().isTrigger = true;
        Player.GetComponent<Rigidbody>().isKinematic = true;
        Player.GetComponent<FpsCamera>().enabled = false;

        animacionStart2=true;
        Rutas2.SetActive(true);
        NpcPersigue2.SetActive(true);
        Invoke("activarObjetivo2", 8f);
    }

    void activarObjetivo2(){
        MisionTalaObject.transform.GetChild(4).gameObject.SetActive(true);
    }
}
