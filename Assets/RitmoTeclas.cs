using System;
using System.Collections;
using System.Collections.Generic;
using GLTF.Schema;
using UnityEngine;
using UnityEngine.UI;

public class RitmoTeclas : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject actual1;  // rojo
    public GameObject brillante1;

    public GameObject actual2;  //amarillo
    public GameObject brillante2;
 
    public GameObject actual3;  //verde
    public GameObject brillante3;

    public GameObject actual4;  //azul
    public GameObject brillante4;

    float tiempo;
    
    GameObject tileRoja;
    GameObject tileAmarilla;
    GameObject tileVerde;
    GameObject tileAzul;

    public AudioSource Error;

    //384 teclas en total
    Dictionary<float,string> generarT = new Dictionary<float, string>() {
        { 6.239f, "W" },
        { 6.368f, "P" },
        { 6.554f, "Q" },
        { 6.891f, "O" },
        { 7.062f, "W" },
        { 7.359f, "P" },
        { 7.645f, "W" },
        { 7.760f, "O" },
        { 7.891f, "W" },
        { 8.220f, "O" },
        { 8.336f, "W" },
        { 8.491f, "O" },
        { 11.034f, "O" },
        { 11.132f, "P" },
        { 11.380f, "W" },
        { 11.658f, "O" },
        { 11.791f, "Q" },
        { 13.423f, "Q" },
        { 13.424f, "W" },//13.435
        { 13.893f, "P" },
        { 13.892f, "O" },
        { 14.384f, "Q" },
        { 14.385f, "W" },//14.402
        { 14.794f, "P" },
        { 14.825f, "O" },
        { 15.142f, "W" },
        { 15.284f, "Q" },
        { 15.398f, "P" },
        { 15.565f, "Q" },
        { 15.862f, "W" },
        { 15.861f, "O" },
        { 16.044f, "P" },
        { 16.200f, "Q" },
        { 16.482f, "O" },
        { 16.599f, "W" },
        { 16.764f, "P" },
        { 17.210f, "W" },
        { 17.355f, "O" },
        { 17.495f, "W" },
        { 17.795f, "O" },
        { 17.918f, "W" },
        { 18.054f, "O" },
        { 18.229f, "Q" },
        { 18.378f, "P" },
        { 18.542f, "W" },
        { 18.846f, "Q" },
        { 18.997f, "O" },
        { 19.157f, "W" },
        { 19.320f, "P" },
        { 19.662f, "O" },
        { 19.757f, "W" },
        { 19.828f, "Q" },
        { 20.185f, "O" },
        { 20.302f, "W" },
        { 20.497f, "O" },
        { 20.644f, "Q" },
        { 20.755f, "P" },
        { 20.957f, "O" },
        { 21.108f, "W" },
        { 21.468f, "Q" },
        { 21.653f, "P" },
        { 22.025f, "O" },
        { 22.175f, "Q" },
        { 22.488f, "P" },
        { 22.751f, "Q" },
        { 23.107f, "W" },
        { 23.481f, "O" },
        { 23.985f, "P" },
        { 24.370f, "W" },
        { 24.713f, "P" },
        { 24.859f, "Q" },
        { 24.960f, "O" },
        { 25.195f, "P" },
        { 25.507f, "Q" },
        { 25.518f, "W" },
        { 25.783f, "P" },
        { 26.025f, "W" },
        { 26.172f, "O" },
        { 26.492f, "W" },
        { 26.838f, "Q" },
        { 27.114f, "P" },
        { 27.396f, "W" },
        { 27.536f, "O" },
        { 27.656f, "W" },
        { 28.124f, "P" },
        { 28.416f, "W" },
        { 28.604f, "O" },
        { 28.879f, "W" },
        { 29.238f, "Q" },
        { 29.472f, "O" },
        { 29.626f, "P" },
        { 29.839f, "W" },
        { 30.022f, "Q" },
        { 30.540f, "W" },
        { 30.836f, "Q" },
        { 30.999f, "P" },
        { 31.312f, "Q" },
        { 31.596f, "O" },
        { 31.930f, "P" },
        { 32.186f, "W" },
        { 32.333f, "O" },
        { 32.494f, "W" },
        { 32.824f, "P" },
        { 33.037f, "P" },
        { 33.358f, "P" },
        { 33.762f, "W" },
        { 33.929f, "W" },
        { 34.231f, "Q" },
        { 34.392f, "Q" },
        { 34.519f, "O" },
        { 34.773f, "P" },
        { 35.038f, "Q" },
        { 35.355f, "P" },
        { 35.653f, "W" },
        { 35.815f, "O" },
        { 36.098f, "Q" },
        { 36.417f, "W" },
        { 36.655f, "O" },
        { 36.812f, "P" },
        { 37.114f, "W" },
        { 37.244f, "Q" },
        { 37.804f, "P" },
        { 38.053f, "W" },
        { 38.218f, "O" },
        { 38.536f, "Q" },
        { 38.811f, "P" },
        { 39.075f, "Q" },
        { 39.389f, "W" },
        { 39.488f, "O" },
        { 39.656f, "P" },
        { 39.830f, "Q" },
        { 40.074f, "P" },
        { 40.281f, "W" },
        { 40.504f, "O" },
        { 40.661f, "Q" },
        { 40.897f, "O" },
        { 41.084f, "W" },
        { 41.386f, "P" },
        { 41.545f, "W" },
        { 41.649f, "O" },
        { 41.982f, "Q" },
        { 42.073f, "W" },
        { 42.246f, "O" },
        { 42.392f, "O" },
        { 42.832f, "Q" },
        { 43.002f, "Q" },
        { 43.398f, "P" },
        { 43.558f, "P" },
        { 44.043f, "O" },
        { 44.056f, "W" },
        { 44.202f, "W" },
        { 44.203f, "O" },
        { 44.651f, "P" },
        { 44.679f, "Q" },
        { 44.962f, "W" },
        { 45.157f, "Q" },
        { 45.299f, "O" },
        { 45.407f, "P" },
        { 45.699f, "W" },
        { 46.029f, "W" },
        { 46.317f, "W" },
        { 46.630f, "Q" },
        { 46.769f, "W" },
        { 46.902f, "O" },
        { 47.048f, "P" },
        { 47.348f, "W" },
        { 47.568f, "Q" },
        { 47.700f, "O" },
        { 47.782f, "P" },
        { 48.098f, "O" },
        { 48.427f, "O" },
        { 48.735f, "O" },
        { 48.991f, "W" },
        { 49.106f, "Q" },
        { 49.303f, "O" },
        { 49.438f, "P" },
        { 49.743f, "Q" },
        { 49.892f, "W" },
        { 50.063f, "O" },
        { 50.196f, "P" },
        { 50.498f, "O" },
        { 50.823f, "O" },
        { 51.105f, "O" },
        { 51.438f, "W" },
        { 51.523f, "Q" },
        { 51.680f, "O" },
        { 51.797f, "P" },
        { 52.156f, "Q" },
        { 52.230f, "W" },
        { 52.443f, "O" },
        { 52.548f, "P" },
        { 52.896f, "W" },
        { 53.219f, "W" },
        { 53.567f, "W" },
        { 53.831f, "Q" },
        { 53.964f, "W" },
        { 54.066f, "O" },
        { 54.178f, "P" },
        { 54.303f, "Q" },
        { 54.880f, "O" },
        { 55.454f, "W" },
        { 56.023f, "P" },
        { 56.182f, "W" },
        { 56.314f, "O" },
        { 56.506f, "Q" },
        { 56.647f, "P" },
        { 57.284f, "O" },
        { 57.860f, "W" },
        { 58.432f, "P" },
        { 58.556f, "W" },
        { 58.695f, "O" },
        { 58.860f, "Q" },
        { 58.999f, "W" },
        { 59.091f, "P" },
        { 59.637f, "P" },
        { 60.234f, "Q" },
        { 60.853f, "O" },
        { 61.435f, "W" },
        { 61.552f, "Q" },
        { 61.658f, "O" },
        { 61.793f, "P" },
        { 62.206f, "W" },
        { 62.440f, "P" },
        { 62.631f, "W" },
        { 63.352f, "P" },
        { 63.569f, "Q" },
        { 63.809f, "W" },
        { 64.251f, "O" },
        { 64.756f, "P" },
        { 65.172f, "W" },
        { 65.604f, "O" },
        { 66.130f, "Q" },
        { 66.561f, "P" },
        { 66.960f, "W" },
        { 67.403f, "O" },
        { 67.880f, "Q" },
        { 68.343f, "P" },
        { 68.690f, "W" },
        { 69.072f, "O" },
        { 69.564f, "P" },
        { 70.011f, "W" },
        { 70.444f, "P" },
        { 70.923f, "O" },
        { 71.396f, "P" },
        { 71.772f, "W" },
        { 72.297f, "O" },
        { 72.687f, "Q" },
        { 73.102f, "W" },
        { 73.436f, "P" },
        { 73.463f, "Q" },
        { 74.586f, "W" },
        { 74.597f, "O" },
        { 74.830f, "P" },
        { 74.988f, "Q" },
        { 75.203f, "O" },
        { 75.411f, "W" },
        { 75.718f, "P" },
        { 75.981f, "W" },
        { 76.108f, "O" },
        { 76.264f, "W" },
        { 76.595f, "O" },
        { 76.734f, "W" },
        { 76.868f, "O" },
        { 77.075f, "Q" },
        { 77.182f, "P" },
        { 77.356f, "Q" },
        { 77.484f, "W" },
        { 77.706f, "O" },
        { 77.919f, "W" },
        { 78.063f, "P" },
        { 78.424f, "O" },
        { 78.520f, "W" },
        { 78.624f, "Q" },
        { 79.001f, "O" },
        { 79.136f, "W" },
        { 79.284f, "O" },
        { 79.435f, "Q" },
        { 79.547f, "P" },
        { 79.705f, "W" },
        { 79.824f, "Q" },
        { 80.190f, "P" },
        { 80.504f, "Q" },
        { 80.852f, "O" },
        { 80.970f, "W" },
        { 81.297f, "P" },
        { 81.545f, "W" },
        { 81.946f, "Q" },
        { 82.299f, "P" },
        { 82.308f, "O" },
        { 82.717f, "Q" },
        { 83.158f, "P" },
        { 83.183f, "O" },
        { 83.481f, "W" },
        { 83.613f, "Q" },
        { 83.780f, "P" },
        { 83.925f, "Q" },
        { 84.181f, "O" },
        { 84.200f, "W" },
        { 84.422f, "P" },
        { 84.607f, "Q" },
        { 84.869f, "O" },
        { 85.015f, "W" },
        { 85.180f, "P" },
        { 85.620f, "W" },
        { 85.767f, "O" },
        { 85.921f, "W" },
        { 86.213f, "O" },
        { 86.337f, "W" },
        { 86.481f, "O" },
        { 86.638f, "Q" },
        { 86.795f, "P" },
        { 86.957f, "Q" },
        { 87.254f, "W" },
        { 87.365f, "O" },
        { 87.586f, "W" },
        { 87.709f, "P" },
        { 88.037f, "O" },
        { 88.138f, "W" },
        { 88.244f, "Q" },
        { 88.557f, "O" },
        { 88.720f, "W" },
        { 88.885f, "O" },
        { 89.053f, "Q" },
        { 89.197f, "P" },
        { 89.362f, "W" },
        { 89.468f, "Q" },
        { 89.771f, "O" },
        { 90.072f, "P" },
        { 90.415f, "O" },
        { 90.569f, "W" },
        { 90.871f, "P" },
        { 91.091f, "Q" },
        { 91.486f, "O" },
        { 91.496f, "W" },
        { 91.921f, "P" },
        { 91.931f, "O" },
        { 92.366f, "P" },
        { 92.367f, "Q" },
        { 92.761f, "O" },
        { 92.762f, "W" },
        { 93.130f, "P" },
        { 93.276f, "W" },
        { 93.385f, "O" },
        { 93.526f, "P" },
        { 93.856f, "Q" },
        { 93.865f, "W" },
        { 94.226f, "P" },
        { 94.243f, "O" },
        { 94.678f, "Q" },
        { 94.725f, "P" },
        { 95.188f, "W" },
        { 95.189f, "O" },
        { 95.516f, "P" },
        { 95.674f, "W" },
        { 95.785f, "O" },
        { 95.904f, "P" },
        { 96.249f, "Q" },
        { 96.250f, "W" },
        { 96.700f, "P" },
        { 96.717f, "O" },
        { 97.140f, "Q" },
        { 97.152f, "P" },
        { 97.583f, "W" },
        { 97.594f, "O" },
        { 97.925f, "P" },
        { 98.064f, "W" },
        { 98.185f, "O" },
        { 98.359f, "P" },
        { 98.653f, "Q" },
        { 98.665f, "W" },
        { 99.158f, "P" },
        { 99.169f, "O" },
        { 99.525f, "Q" },
        { 99.526f, "W" },//99.543
        { 100.051f, "P" },
        { 100.052f, "O" },
        { 100.482f, "Q" },
        { 100.483f, "W" },//100.495
        { 100.905f, "O" },
        { 100.906f, "P" },

        { 101.305f, "O" },
        { 101.306f, "W" },
        { 101.705f, "P" },
        { 101.706f, "Q" }

    };
    public AudioSource musica;

    private float escadaDelTiempoAlPausar, escadaDelTiempoInicial;

    public float escadaDelTiempo = 1;
    bool iniciarGame = false;

    public float tiempoInicial = -7.75f;  // esto era int

    private Text myText;
    private float tiempoDelFrameConTimeScale = 0f;
    public float tiempoAMostrarEnSegundos = -3f;

    float margen;

    public GameObject Rojo;
    public GameObject Amari;
    public GameObject Verde;
    public GameObject Azul;

    public GameObject PRojo;
    public GameObject PAmari;
    public GameObject PVerde;
    public GameObject PAzul;

    Canvas canvas;

    float tiempoCaidaTeclas;

    float delay = 0f;

    public GameObject bien;
    public GameObject increible;

    public GameObject mal;

    Vector3 posGenerarMensajes;

    bool DesactivarTeclas = false;

    float retrasoM = 5f; // valor para que inicie antes la musica

    GameObject Player;
    void Start()
    {
        //// DESACTIVAR MOVIMIENTO PLAYER
        Player = GameObject.Find("Player");
        DesactivarScriptsPlayer(false);

        ///////////////////////MISION RITMO
        posGenerarMensajes = transform.GetChild(4).position;

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        tiempoCaidaTeclas = 1.355f;
        margen = 0.125f;

        actual1 = transform.GetChild(0).GetChild(0).gameObject;
        brillante1 = transform.GetChild(0).GetChild(1).gameObject;
        brillante1.SetActive(false);

        actual2 = transform.GetChild(1).GetChild(0).gameObject;
        brillante2 = transform.GetChild(1).GetChild(1).gameObject;
        brillante2.SetActive(false);

        actual3 = transform.GetChild(2).GetChild(0).gameObject;
        brillante3 = transform.GetChild(2).GetChild(1).gameObject;
        brillante3.SetActive(false);

        actual4 = transform.GetChild(3).GetChild(0).gameObject;
        brillante4 = transform.GetChild(3).GetChild(1).gameObject;
        brillante4.SetActive(false);

        tiempo=0.1f;



        iniciarGame = false;
        escadaDelTiempoInicial = escadaDelTiempo;

        //myText = GetComponent<Text>();
        //musica = GetComponent<AudioSource>();

        tiempoAMostrarEnSegundos = tiempoInicial;
        

        Time.timeScale=1;  // esto estaba en 0
    }

    // Update is called once per frame
    void Update()
    {


        if(Input.GetKeyDown(KeyCode.C) && !iniciarGame){
            iniciarGame = true;
            Invoke("delayM",7f-retrasoM); // estaba en 7, lo puse en 4 y ahora se le resta la variable retrasoM al tiempo de invocacion
            
            foreach(KeyValuePair<float, string> gen in generarT){
                if(gen.Value=="Q"){
                    Invoke("initR",gen.Key + delay - retrasoM);
                }else if(gen.Value=="W"){
                    Invoke("initA",gen.Key + delay - retrasoM);
                }
                else if(gen.Value=="O"){
                    Invoke("initV",gen.Key + delay - retrasoM);
                }
                else if(gen.Value=="P"){
                    Invoke("initAz",gen.Key + delay - retrasoM);
                }
            // generar en esta linea usando invoke de elementos con movimiento hacia abajo
        } 
        } 

        

        if(iniciarGame == true){
            Time.timeScale= 1;
            tiempoDelFrameConTimeScale = Time.deltaTime * escadaDelTiempo;
            tiempoAMostrarEnSegundos += tiempoDelFrameConTimeScale; // cambiar esto a += hace que avance
        } 

        if(tiempoAMostrarEnSegundos>= 102f){
            DesactivarTeclas = true;
            DesactivarScriptsPlayer(true);
        }

        if(tiempoAMostrarEnSegundos>= 104f){ // no dejar que se tome la mision denuevo
            gameObject.SetActive(false);
        }

        if( !DesactivarTeclas){

            if(Input.GetKeyDown(KeyCode.Q)){
                brillante1.SetActive(true);
                Invoke("desactivar1", tiempo);
                //Debug.Log("Tiempo Rojo: "+tiempoAMostrarEnSegundos.ToString());
                bool R1 =TocarNota("Q");
                /* if(R1){
                    Debug.Log("correcto");
                }else{
                    Debug.Log("Incorrecto");
                } */
            }

            if(Input.GetKeyDown(KeyCode.W)){
                brillante2.SetActive(true);
                Invoke("desactivar2", tiempo);
                //Debug.Log("Tiempo Amari: "+tiempoAMostrarEnSegundos.ToString());
                bool R2 =TocarNota("W");
                /* if(R2){
                    Debug.Log("correcto1");
                }else{
                    Debug.Log("Incorrecto1");
                } */
            }

            if(Input.GetKeyDown(KeyCode.O)){
                brillante3.SetActive(true);
                Invoke("desactivar3", tiempo);
                //Debug.Log("Tiempo verde: "+tiempoAMostrarEnSegundos.ToString());
                bool R3 =TocarNota("O");
                /* if(R3){
                    Debug.Log("correcto2");
                }else{
                    Debug.Log("Incorrecto2");
                } */
            }

            if(Input.GetKeyDown(KeyCode.P)){
                brillante4.SetActive(true);
                Invoke("desactivar4", tiempo);
                //Debug.Log("Tiempo Azul: "+tiempoAMostrarEnSegundos.ToString());
                bool R4 =TocarNota("P");
                /* if(R4){
                    Debug.Log("correcto3");
                }else{
                    Debug.Log("Incorrecto3");
                } */
            }

        }
        
        
        
        
    }

    void desactivar1(){
        brillante1.SetActive(false);
    }
    void desactivar2(){
        brillante2.SetActive(false);
    }
    void desactivar3(){
        brillante3.SetActive(false);
    }
    void desactivar4(){
        brillante4.SetActive(false);
    }

    void delayM(){
        musica.Play();
    }

    void initR(){
        GameObject a =Instantiate(Rojo, PRojo.transform.position, Quaternion.identity, transform.parent);
        //a.transform.SetParent(canvas.transform);
        a.GetComponent<TilesMov>().vel = (Vector3.Distance(actual1.transform.position , PRojo.transform.position))/tiempoCaidaTeclas;

        Destroy(a, 2f);
    }

    void initA(){
        GameObject a =Instantiate(Amari, PAmari.transform.position, Quaternion.identity, transform.parent);
        a.GetComponent<TilesMov>().vel = (Vector3.Distance(actual2.transform.position , PAmari.transform.position))/tiempoCaidaTeclas;
        //a.transform.SetParent(canvas.transform);
        Destroy(a, 2f);
    }

    void initV(){
        GameObject a =Instantiate(Verde, PVerde.transform.position, Quaternion.identity, transform.parent);
        a.GetComponent<TilesMov>().vel = (Vector3.Distance(actual3.transform.position , PVerde.transform.position))/tiempoCaidaTeclas;
        //a.transform.SetParent(canvas.transform);
        Destroy(a, 2f);
    }

    void initAz(){
        GameObject a =Instantiate(Azul, PAzul.transform.position, Quaternion.identity, transform.parent);
        a.GetComponent<TilesMov>().vel = (Vector3.Distance(actual4.transform.position , PAzul.transform.position))/tiempoCaidaTeclas;
        //a.transform.SetParent(canvas.transform);
        Destroy(a, 2f);
    }

    bool TocarNota(string letra){
       
        foreach(KeyValuePair<float, string> gen in generarT){
            // el tiempo a mostrar en segundos hay que restarle -1.4f aprox
            if(gen.Value==letra && (  (gen.Key-retrasoM>=(tiempoAMostrarEnSegundos-margen-tiempoCaidaTeclas)) && (gen.Key-retrasoM<= (tiempoAMostrarEnSegundos+margen-tiempoCaidaTeclas))   )){ // se presiono la tecla "letra" y esta dentro de lo permitido
                //Debug.Log(gen.Key);
                generarT.Remove(gen.Key);
                if( (gen.Key-retrasoM>=(tiempoAMostrarEnSegundos-(margen/2)-tiempoCaidaTeclas)) && (gen.Key-retrasoM<=(tiempoAMostrarEnSegundos+(margen/2)-tiempoCaidaTeclas))){
                    Instantiate(increible, posGenerarMensajes, Quaternion.identity, transform);
                }else{
                    Instantiate(bien, posGenerarMensajes, Quaternion.identity, transform);
                }


                return true;
            } 
            
        }

        Instantiate(mal, posGenerarMensajes, Quaternion.identity, transform);
        Error.Play();
        return false;
    }

    void DesactivarScriptsPlayer(bool des){
        Player.GetComponent<FpsCamera>().enabled = des;
        Player.GetComponent<CharacterMovement>().enabled = des;
        Player.GetComponent<CameraInteraction>().enabled = des;
        Player.GetComponent<AbrirMapa>().enabled = des;
        Player.GetComponent<Mascota>().enabled = des;
    }

}
