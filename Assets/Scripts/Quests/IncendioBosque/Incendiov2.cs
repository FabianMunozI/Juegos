using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;  
using System.Linq;

using Random=UnityEngine.Random;

// Script con Logica de la Mision.
public class Incendiov2 : MonoBehaviour
{
    // Parametros de la mision
    public int tiempoRestante = 10;  
    public float intervaloQuemarArbol = 1f;

    public GameObject llamasContenedor;
    public GameObject llamaPrefab;
    public GameObject questGiver;
    public GameObject Jugador;
    public GameObject baldeDeAgua;
    public GameObject pozo;
    public TextMeshProUGUI cuentaRegresiva; 
    public GameObject humoParticulas;
    public List<GameObject> arbolesQuemadosPrefabs;

    // UI - Timer
    public GameObject textHolder;
    public TextMeshProUGUI quest_text;
    
    // Estado Mision
    private bool Quest_started = false; // Mision en curso
    public bool missionDone = false; // Mision ya hecha

    // Personaje - Camara
    Vector3 posCamara;
    GameObject CamaraO;
    Vector3 VectorPartida;
    public bool inputTrue = false;
    public Animator controller;

    // Arboles
    public GameObject Trees_group;
    private List<int> tree_list = new List<int>();
    List<int> burneable_trees;

    public GameObject Guardabosque;

    public GameObject minimapIndicator;
    public GameObject infoIndicator;

    private int counter_helper = 0;


    void Start()
    {
        Jugador = GameObject.Find("Player");
        CamaraO = GameObject.Find("Camera");

        int tree_quant = Trees_group.transform.childCount;

        burneable_trees = Enumerable.Range(0, tree_quant).ToList();

        tree_list = GenerateRandom(4, 0, tree_quant - 1);

        quest_text.enabled = false;
        textHolder.SetActive(false);
        cuentaRegresiva.enabled = false;

        baldeDeAgua.GetComponent<Outline>().enabled = false;
        pozo.GetComponent<Outline>().enabled = false;

        foreach( var x in tree_list) {
                burneable_trees.Remove(x);
            }

        if(!missionDone)
        {
            minimapIndicator.SetActive(true);
            infoIndicator.SetActive(false);
        } else{
            infoIndicator.SetActive(true);
        }

        

    }

    

    void countDownTimer() {  

        if (tiempoRestante > 0) {  
            TimeSpan spanTime = TimeSpan.FromSeconds(tiempoRestante);  
            cuentaRegresiva.text = ".- Tiempo restante: " + spanTime.Minutes + " : " + spanTime.Seconds;  
            tiempoRestante--;  
            Invoke("countDownTimer", 1.0f);
            counter_helper++;

            if (counter_helper == intervaloQuemarArbol)
            {
                counter_helper = 0;
                QuemarNuevoArbol();
            }

        } else {  
            
            missionDone = true;


        }  
    }  

    void QuemarNuevoArbol() {
        // Quemar nuevo arbol.

        if (burneable_trees.Count > 0) {
            int random_idx = Random.Range(0, burneable_trees.Count);  
            int tree_index = burneable_trees.ElementAt(random_idx);
            burneable_trees.RemoveAt(random_idx);

            Transform randomtree = Trees_group.transform.GetChild(tree_index);
            var new_Llama = Instantiate(llamaPrefab,  randomtree.transform.position, Quaternion.Euler(-90,0,-180));
            new_Llama.GetComponent<Apagar>().associeted_child = tree_index;
            new_Llama.transform.parent = llamasContenedor.transform;
        }

    }

    private void replace_burned_trees()
    {

        for (int i = 0; i < llamasContenedor.transform.childCount; i++)
        {
            if(llamasContenedor.transform.GetChild(i).GetComponent<Apagar>().live)
            {
                llamasContenedor.transform.GetChild(i).GetComponent<Apagar>().live = false;
                int tree_asc = llamasContenedor.transform.GetChild(i).GetComponent<Apagar>().associeted_child;
                Vector3 pos_tree = llamasContenedor.transform.GetChild(i).transform.position;

                var burned_tree = Instantiate(arbolesQuemadosPrefabs.ElementAt(Random.Range(0, arbolesQuemadosPrefabs.Count - 1)),  pos_tree, Quaternion.Euler(0,0,0));

                llamasContenedor.transform.GetChild(i).gameObject.SetActive(false);
                Instantiate(humoParticulas,  pos_tree, Quaternion.Euler(-90,0,-180));
                Trees_group.transform.GetChild(tree_asc).gameObject.SetActive(false);
    
                //vivos.Add(llamasContenedor.transform.GetChild(i).GetComponent<Apagar>().associeted_child);
            }
        }
    }

    void Update()
    {
        
        // Mision en Progreso.
        if (Quest_started)
        {
           
            quest_text.text = ".- Arboles incendiados: " + count_actives(llamasContenedor) ;

            // Terminada por condicionalidad 
            if (llamasContenedor.transform.childCount == 0)
            {
                missionDone = true;
            }
        }

        // MISION INICIADA
        if (!Quest_started && !(missionDone) && questGiver.GetComponent<Quest_Starter>().misionAceptada ){ //

            //OnOffPlayer();
            //VectorPartida = CamaraO.transform.position;

            //if(Jugador.GetComponent<CharacterMovement>().EstaParado==false){
            //    CamaraO.transform.position= Jugador.transform.position + Jugador.GetComponent<CharacterMovement>().InitialPos;
            //    Jugador.GetComponent<CharacterMovement>().EstaParado=true;
            //}else{

            //}

            //Jugador.transform.position = new Vector3(7,1,20);
            //Jugador.transform.rotation = Quaternion.Euler(0, -93.019f,0);
            //CamaraO.transform.position = new Vector3(0,0.69f,0);
            //CamaraO.transform.rotation = Quaternion.Euler(0,0,0);
            //CamaraO.transform.rotation=Jugador.transform.rotation;

            //posCamara = CamaraO.transform.position;

            //controller.SetTrigger("Start2");

            //Invoke("OnOffPlayer", 14.5f);

            minimapIndicator.SetActive(false);

            Quest_started = true;
            questGiver.SetActive(false);

            
            foreach( var x in tree_list) {
                //Debug.Log( x.ToString());
                Transform randomtree = Trees_group.transform.GetChild(x);
                //Debug.Log( randomChild.transform.position);
                var new_Llama = Instantiate(llamaPrefab,  randomtree.transform.position, Quaternion.Euler(-90,0,-180));
                new_Llama.GetComponent<Apagar>().associeted_child = x;
                new_Llama.transform.parent = llamasContenedor.transform;
            }

            quest_text.enabled = true;
            textHolder.SetActive(true);
            cuentaRegresiva.enabled = true;
            baldeDeAgua.GetComponent<Outline>().enabled = true;
            pozo.GetComponent<Outline>().enabled = true;

            Debug.Log("[Quest] - Mision Incendio Iniciada.");
            countDownTimer();
        }


        // Mision terminada
        if (missionDone)
        {
            infoIndicator.SetActive(true);
            Quest_started = false;
            cuentaRegresiva.text = "";
            quest_text.text = "Mision Terminada!";
            replace_burned_trees();
            baldeDeAgua.GetComponent<Outline>().enabled = false;
            pozo.GetComponent<Outline>().enabled = false;

            //apagarTiempoMision = true;
            //missionDone = true;
            //quest_text.text = "Mision Exitosa  :)";
            //Quest_started = false;
            //cuentaRegresiva.SetActive(true);
            Guardabosque.SetActive(true);
            Invoke("desactivarTexto",5f);
        }



    }

    void desactivarTexto(){ 
        cuentaRegresiva.enabled = false;
        quest_text.enabled = false;
        textHolder.SetActive(false);
    }

    private int count_actives(GameObject currentObj)
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

    public static List<int> GenerateRandom(int count, int min, int max)
        {

            List<int> list = new List<int>();
            int Rand;
            for (int j = 0; j < count; j++)
            {    
                do 
                {
                    Rand = Random.Range(min,max);
                } while(list.Contains(Rand));
    
                list.Add(Rand);
            }
    
            return list;
        }





}
