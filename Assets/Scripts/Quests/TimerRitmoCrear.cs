using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TimerRitmoCrear : MonoBehaviour
{
    public AudioSource musica;

    [Tooltip("Tiempo Inicial en segundos")]
    public int tiempoInicial;

    [Tooltip("Escala de tiempo del Reloj")]
    [Range(-10.0f,10.0f)]
    public float escadaDelTiempo = 1;

    private Text myText;
    private float tiempoDelFrameConTimeScale = 0f;
    public float tiempoAMostrarEnSegundos = -3f;
    private float escadaDelTiempoAlPausar, escadaDelTiempoInicial;
    

    bool iniciarGame;

    List<float> listaGenerarQ = new List<float>();
    List<float> listaGenerarW = new List<float>();
    List<float> listaGenerarO = new List<float>();
    List<float> listaGenerarP = new List<float>();


    // Start is called before the first frame update
    void Start()
    {
        iniciarGame = false;
        escadaDelTiempoInicial = escadaDelTiempo;

        //myText = GetComponent<Text>();
        //musica = GetComponent<AudioSource>();

        tiempoAMostrarEnSegundos = tiempoInicial;

        //ActualizarReloj(tiempoInicial);
        Time.timeScale=0;
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.X) && iniciarGame){
            iniciarGame = false;
            //musica.Stop();
            for(int i =0; i< listaGenerarQ.Count-1;i++){
                Debug.Log("Q: " + listaGenerarQ[i].ToString());
            }
            for(int i =0; i< listaGenerarW.Count-1;i++){
                Debug.Log("W: " + listaGenerarW[i].ToString());
            }
            for(int i =0; i< listaGenerarO.Count-1;i++){
                Debug.Log("O: " + listaGenerarO[i].ToString());
            }
            for(int i =0; i< listaGenerarP.Count-1;i++){
                Debug.Log("P: "+listaGenerarP[i].ToString());
            }
        }

        if(Input.GetKeyDown(KeyCode.C) && !iniciarGame){
            iniciarGame = true;
            Invoke("delayM",3f);
        }

        

        if(iniciarGame == true){
            Time.timeScale= 1;
            tiempoDelFrameConTimeScale = Time.deltaTime * escadaDelTiempo;
            tiempoAMostrarEnSegundos += tiempoDelFrameConTimeScale; // cambiar esto a += hace que avance
            
            if(Input.GetKeyDown(KeyCode.Q)){
                listaGenerarQ.Add(tiempoAMostrarEnSegundos);
            }
            if(Input.GetKeyDown(KeyCode.W)){
                listaGenerarW.Add(tiempoAMostrarEnSegundos);
            }
            if(Input.GetKeyDown(KeyCode.O)){
                listaGenerarO.Add(tiempoAMostrarEnSegundos);
            }
            if(Input.GetKeyDown(KeyCode.P)){
                listaGenerarP.Add(tiempoAMostrarEnSegundos);
            }
            //ActualizarReloj(tiempoAMostrarEnSegundos);
        }
    }

    /* public void ActualizarReloj(float tiempoEnSegundos){
        int minutos = 0;
        int segundos = 0;
        string textoDelRejoj;

        if(tiempoEnSegundos < 0) tiempoEnSegundos = 0;

        minutos = (int)tiempoEnSegundos / 60;
        segundos = (int) tiempoEnSegundos % 60;

        
        
        textoDelRejoj = minutos.ToString("00") + ":" + segundos.ToString("00");
        string text1 = "- Tiempo Restante:         ";
        myText.text = text1 + textoDelRejoj;
        
        
    } */

    /* public void Pausar(){
        if(!estaPausado){

            estaPausado = true;
            escadaDelTiempoAlPausar = escadaDelTiempo;
            escadaDelTiempo = 0;
        }
    }

    public void Continuar(){
        if(estaPausado){

            estaPausado = false;
            escadaDelTiempo = escadaDelTiempoAlPausar;
        }
    }
    public void ResetTimer(){
        estaPausado = false;
        escadaDelTiempo = escadaDelTiempoInicial;
        tiempoAMostrarEnSegundos = tiempoInicial;
        ActualizarReloj(tiempoAMostrarEnSegundos);
    } */

    /* public void Salir(){
        Application.Quit();
    } */

    void delayM(){
        musica.Play();
    }
}
