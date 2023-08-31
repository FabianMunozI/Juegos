using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Invasores : MonoBehaviour
{

    public GameObject questGiver;

    public List<GameObject> invasoresPrefab;
    public List<GameObject> buenosPrefab;
    public GameObject verificadorPrefab;
    List<GameObject> spawnVerificador;

    
    // Estado Mision
    public bool Quest_started = false; // Mision en curso
    public bool missionDone = false; // Mision ya hecha

    // Personaje - Camara
    Vector3 posCamara;
    GameObject CamaraO;
    Vector3 VectorPartida;
    public bool inputTrue = false;
    public GameObject Jugador;

    // UI
    public GameObject questTracker;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questText;


    List <GameObject> bolasVerificadoras = new List<GameObject>();
    List <GameObject> invasores = new List<GameObject>();
    List <GameObject> buenos = new List<GameObject>();

    bool invocarCamara = false;
    bool invocarCamara2 = false;

    Quaternion rotOriginal;

    public bool CameraBusy = false;

    public GameObject particulas;
    public GameObject toLook;

    public GameObject minimapIndicator;



    // Start is called before the first frame update
    void Start()
    {
        particulas.SetActive(false);
        Jugador = GameObject.Find("Player");
        CamaraO = GameObject.Find("Camera");

        for(int i = 0 ; i < 5; i++)
        {
            bolasVerificadoras.Add(
                Instantiate(verificadorPrefab,
                new Vector3(Random.Range(-20, 20), 20  , Random.Range(-20, 20)  ),
                Quaternion.Euler(0,0,0))
                );
        }

        questTracker.SetActive(false);


        if(!missionDone)
        {
            minimapIndicator.SetActive(true);
        } 

    }

    // Update is called once per frame
    void Update()
    {

        if (ZonaDeposito.jaulasAdentro >= 2 && !missionDone)
        {
            particulas.SetActive(false);
            missionDone = true;
            Quest_started = false;

            OnOffPlayer();

            questTitle.text = "Mision Terminada!";
            questText.text = "Observa como vuelve la vida al bosque.";

            int cont = 0;

            foreach( var buenoPrefab in buenosPrefab)
            {
                bool instanciado = false;
                if (cont > bolasVerificadoras.Count)
                {
                    cont = 0;
                }

                int intentos = 0;

                while(instanciado == false)
                {
                    if (intentos == bolasVerificadoras.Count)
                    {
                        buenos.Add(Instantiate(
                            buenoPrefab,
                            new Vector3(0,0,0),
                            Quaternion.Euler(0,0,0)
                        ));

                        buenos.Add(Instantiate(
                            buenoPrefab,
                            new Vector3(-1,0,1),
                            Quaternion.Euler(0,0,0)
                        ));

                        buenos.Add(Instantiate(
                            buenoPrefab,
                            new Vector3(1,0,1),
                            Quaternion.Euler(0,0,0)
                        ));

                        instanciado = true;
                    }

                    if (bolasVerificadoras[cont].GetComponent<Verificador>().collisions > 0)
                    {
                        buenos.Add(Instantiate(
                            buenoPrefab,
                            bolasVerificadoras[cont].transform.position,
                            Quaternion.Euler(0,0,0)
                        ));

                        buenos.Add(Instantiate(
                            buenoPrefab,
                            bolasVerificadoras[cont].transform.position + new Vector3(-1,0,1),
                            Quaternion.Euler(0,0,0)
                        ));

                        buenos.Add(Instantiate(
                            buenoPrefab,
                            bolasVerificadoras[cont].transform.position + new Vector3(1,0,1),
                            Quaternion.Euler(0,0,0)
                        ));

                        instanciado = true;
                    }
                    intentos ++;
                    cont ++;
                }
    
                
            }



            rotOriginal = CamaraO.transform.rotation;

            invocarCamara2 = true;
            Invoke("desactivarTexto",7f);
        }

        if (invocarCamara2)
        {
            CameraBusy = true;
            AjustarCamara2();

            invocarCamara2 = false;

        }

        // Apenas se inicia la mision
        if (!Quest_started && !(missionDone) && questGiver.GetComponent<Quest_Starter>().misionAceptada )
        {
            minimapIndicator.SetActive(false);
            particulas.SetActive(true);
            OnOffPlayer();
            questGiver.SetActive(false);

            int cont = 0;

            foreach( var invasorPrefab in invasoresPrefab)
            {
                bool instanciado = false;
                if (cont > bolasVerificadoras.Count)
                {
                    cont = 0;
                }

                int intentos = 0;

                while(instanciado == false)
                {
                    if (intentos == bolasVerificadoras.Count)
                    {
                        invasores.Add(Instantiate(
                            invasorPrefab,
                            new Vector3(0,0,0),
                            Quaternion.Euler(0,0,0)
                        ));

                        instanciado = true;
                    }

                    if (bolasVerificadoras[cont].GetComponent<Verificador>().collisions > 0)
                    {
                        invasores.Add(Instantiate(
                            invasorPrefab,
                            bolasVerificadoras[cont].transform.position,
                            Quaternion.Euler(0,0,0)
                        ));

                        instanciado = true;
                    }
                    intentos ++;
                    cont ++;
                }
    
                
            }

            rotOriginal = CamaraO.transform.rotation;

            Quest_started = true;
            invocarCamara = true;

        }

        if (invocarCamara)
        {
            CameraBusy = true;
            AjustarCamara();

            invocarCamara = false;

        }

        if (Quest_started && !CameraBusy)
        {
            questText.text = ".- Especies capturadas: "+ ZonaDeposito.jaulasAdentro +"/2.";

        }


        



        
    }

    void desactivarTexto(){ 
        questTracker.SetActive(false);
    }

    int actual_id = 0;
    


    public void AjustarCamara()
    {
        CamaraO.transform.localPosition = new Vector3(0,0.69f,0);
        CamaraO.transform.rotation = rotOriginal;

        if (actual_id < invasores.Count)
        {

            Vector3 pos = invasores[actual_id].transform.position - Jugador.transform.position;
            pos += new Vector3 (5,5,5);

            rotOriginal = CamaraO.transform.rotation;

            CamaraO.transform.Translate(pos, Space.World);
            CamaraO.transform.LookAt(invasores[actual_id].transform);



            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name != invasores[actual_id].transform.name)
                {
                    CamaraO.transform.localPosition = new Vector3(0,0.69f,0);
                    CamaraO.transform.rotation = rotOriginal;
                    pos = invasores[actual_id].transform.position - Jugador.transform.position;
                    pos += new Vector3 (-5,5,-5);
                    CamaraO.transform.Translate(pos, Space.World);
                    CamaraO.transform.LookAt(invasores[actual_id].transform);
                    if (hit.transform.name != invasores[actual_id].transform.name)
                    {
                        CamaraO.transform.localPosition = new Vector3(0,0.69f,0);
                        CamaraO.transform.rotation = rotOriginal;
                        pos = invasores[actual_id].transform.position - Jugador.transform.position;
                        pos += new Vector3 (5,5,-5);
                        CamaraO.transform.Translate(pos, Space.World);
                        CamaraO.transform.LookAt(invasores[actual_id].transform);

                        if (hit.transform.name != invasores[actual_id].transform.name)
                        {
                            CamaraO.transform.localPosition = new Vector3(0,0.69f,0);
                            CamaraO.transform.rotation = rotOriginal;
                            pos = invasores[actual_id].transform.position - Jugador.transform.position;
                            pos += new Vector3 (-5,5,5);
                            CamaraO.transform.Translate(pos, Space.World);
                            CamaraO.transform.LookAt(invasores[actual_id].transform);
                        }
                    }
                }
            }
            
            

            if (actual_id != invasores.Count)
            {
                actual_id ++;
                Invoke("AjustarCamara",2f);
            }
        } else {
            
            Vector3 pos = particulas.transform.position - Jugador.transform.position;
            pos += new Vector3 (-5,5,-5);

            rotOriginal = CamaraO.transform.rotation;

            CamaraO.transform.Translate(pos, Space.World);
            CamaraO.transform.LookAt(particulas.transform);

            Invoke("OnOffPlayer",2f);
        }
    }


    int actual_id2 = 0;


    public void AjustarCamara2()
    {
        CamaraO.transform.localPosition = new Vector3(0,0.69f,0);
        CamaraO.transform.rotation = rotOriginal;

        if (actual_id2 < buenos.Count)
        {

            Vector3 pos = buenos[actual_id2].transform.position - Jugador.transform.position;
            pos += new Vector3 (5,5,5);

            rotOriginal = CamaraO.transform.rotation;

            CamaraO.transform.Translate(pos, Space.World);
            CamaraO.transform.LookAt(buenos[actual_id2].transform);

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name != buenos[actual_id2].transform.name)
                {
                    CamaraO.transform.localPosition = new Vector3(0,0.69f,0);
                    CamaraO.transform.rotation = rotOriginal;
                    pos = buenos[actual_id2].transform.position - Jugador.transform.position;
                    pos += new Vector3 (-5,5,-5);
                    CamaraO.transform.Translate(pos, Space.World);
                    CamaraO.transform.LookAt(buenos[actual_id2].transform);
                    if (hit.transform.name != buenos[actual_id2].transform.name)
                    {
                        CamaraO.transform.localPosition = new Vector3(0,0.69f,0);
                        CamaraO.transform.rotation = rotOriginal;
                        pos = buenos[actual_id2].transform.position - Jugador.transform.position;
                        pos += new Vector3 (5,5,-5);
                        CamaraO.transform.Translate(pos, Space.World);
                        CamaraO.transform.LookAt(buenos[actual_id2].transform);

                        if (hit.transform.name != buenos[actual_id2].transform.name)
                        {
                            CamaraO.transform.localPosition = new Vector3(0,0.69f,0);
                            CamaraO.transform.rotation = rotOriginal;
                            pos = buenos[actual_id2].transform.position - Jugador.transform.position;
                            pos += new Vector3 (-5,5,5);
                            CamaraO.transform.Translate(pos, Space.World);
                            CamaraO.transform.LookAt(buenos[actual_id2].transform);
                        }
                    }
                }
            }
            
            

            if (actual_id2 < buenos.Count)
            {
                actual_id2 += 3;
                Invoke("AjustarCamara2",2f);
            }
        } else {
            OnOffPlayer();
        }
    }

    public void OnOffPlayer(){
        inputTrue = !inputTrue; // se vuelve a true el valor // inicia false en el start
        //Jugador.GetComponent<Rigidbody>().isKinematic = inputTrue;
        Jugador.GetComponent<CharacterMovement>().AnimacionOn = !inputTrue; // frezea al player mientras esta la animacion
        Jugador.GetComponent<FpsCamera>().animacionOn = !inputTrue;
        //BasuraActivar.SetActive(inputTrue); // activa y desactiva la basura

        if(inputTrue==false){


            CamaraO.transform.localPosition = new Vector3(0,0.69f,0);
            CamaraO.transform.rotation = rotOriginal;
            questTracker.SetActive(true);

            CameraBusy = false;
            
        }

    }
}
