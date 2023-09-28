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

    public GameObject[] AnimacionElems;

    GameObject UnoAnim;
    GameObject DosAnim;

    MisionTalaInteract misionTalaInteract;

    int i =0;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");

        misionTalaInteract = GetComponent<MisionTalaInteract>();
        misionTalaInteract.ScriptsPlayer(false);
        Player.GetComponent<Collider>().isTrigger = true;
        Player.GetComponent<Rigidbody>().isKinematic = true;

        animacionStart = true;
        

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
            misionTalaInteract.ScriptsPlayer(true);
            Player.GetComponent<Collider>().isTrigger = false;
            Player.GetComponent<Rigidbody>().isKinematic = false;
            tiempoMision += Time.deltaTime;
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
}
