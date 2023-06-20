using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Temporizador : MonoBehaviour
{
    public AudioSource musica;

    [Tooltip("Tiempo Inicial en segundos")]
    public int tiempoInicial;

    [Tooltip("Escala de tiempo del Reloj")]
    [Range(-10.0f,10.0f)]
    public float escadaDelTiempo = 1;

    private Text myText;
    private float tiempoDelFrameConTimeScale = 0f;
    public float tiempoAMostrarEnSegundos = 0f;
    private float escadaDelTiempoAlPausar, escadaDelTiempoInicial;
    public bool estaPausado = false;

    public bool iniciarGame= true;

    // Start is called before the first frame update
    void Start()
    {
        escadaDelTiempoInicial = escadaDelTiempo;

        myText = GetComponent<Text>();
        //musica = GetComponent<AudioSource>();

        tiempoAMostrarEnSegundos = tiempoInicial;

        ActualizarReloj(tiempoInicial);
        Time.timeScale=0;
    }

    // Update is called once per frame
    void Update()
    {   
        if(iniciarGame == true){
            Time.timeScale= 1;
            tiempoDelFrameConTimeScale = Time.deltaTime * escadaDelTiempo;
            tiempoAMostrarEnSegundos -= tiempoDelFrameConTimeScale; // cambiar esto a += hace que avance
            ActualizarReloj(tiempoAMostrarEnSegundos);

            /* if(!estaPausado){
                Time.timeScale= 1;

                tiempoDelFrameConTimeScale = Time.deltaTime * escadaDelTiempo;

                tiempoAMostrarEnSegundos += tiempoDelFrameConTimeScale;
                ActualizarReloj(tiempoAMostrarEnSegundos);
            }else{
                Time.timeScale = 0f;
            } */

            /* if(Input.GetKeyDown("p") || Input.GetKeyDown("escape")){
                estaPausado=!estaPausado;
                transform.GetChild(0).gameObject.SetActive(estaPausado);
                transform.GetChild(1).gameObject.SetActive(estaPausado);
            } */

            /* if(Input.GetKeyDown(KeyCode.Return)){
                if(estaPausado ==true){
                    SceneManager.LoadScene("SampleScene");
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                    ResetTimer(); 
                }
            } */
        }
        /* if(Input.GetKeyDown("space")){
            if(iniciarGame!= true){
                Time.timeScale=1;
                transform.GetChild(3).gameObject.SetActive(false);
                musica.Play();
                iniciarGame=true;
            }
            

        } */
    }

    public void ActualizarReloj(float tiempoEnSegundos){
        int minutos = 0;
        int segundos = 0;
        string textoDelRejoj;

        if(tiempoEnSegundos < 0) tiempoEnSegundos = 0;

        minutos = (int)tiempoEnSegundos / 60;
        segundos = (int) tiempoEnSegundos % 60;

        textoDelRejoj = minutos.ToString("00") + ":" + segundos.ToString("00");

        myText.text = textoDelRejoj;
    }

    public void Pausar(){
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
    }

    public void Salir(){
        Application.Quit();
    }
}
