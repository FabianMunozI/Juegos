using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshController : MonoBehaviour
{
    public LineRenderer MyLineRenderer;


    public GameObject Player;

    //public Transform objetivo;
    public NavMeshAgent agente;

    public Camera minimapCamera;
    public Camera PlayerCamera; // la principal

    public AbrirMapa referenciaPlayerScript;


    public Transform minimapTopLeftCorner;

    public Transform minimapBottomRightCorner;



    Vector3 GuardadoPuntoFinal;

    // Start is called before the first frame update
    void Start()
    {
        //agente = GetComponent<NavMeshAgent>();

        
        minimapBottomRightCorner = GameObject.Find("ContMinimap").transform.GetChild(3);
        minimapTopLeftCorner = GameObject.Find("ContMinimap").transform.GetChild(2);


        MyLineRenderer.positionCount=0;
        Player= GameObject.Find("Player");
        minimapCamera = Player.transform.GetChild(2).GetComponent<Camera>();
        PlayerCamera = Player.transform.GetChild(0).gameObject.GetComponent<Camera>();
        referenciaPlayerScript = Player.GetComponent<AbrirMapa>();
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetMouseButtonDown(0) && referenciaPlayerScript.abrirMapaGrande==true){
            Vector2 minimapTopLeft = minimapTopLeftCorner.position;
            Vector2 minimapBottomRight = minimapBottomRightCorner.position;
            Vector3 clicEnPantalla = Input.mousePosition;


            if ( (clicEnPantalla.x >= minimapTopLeft.x) && (clicEnPantalla.x <= minimapBottomRight.x) && (clicEnPantalla.y <= minimapTopLeft.y) && (clicEnPantalla.y >= minimapBottomRight.y) )
            {
            // El clic está dentro de los límites del minimapa
            // Convertir las coordenadas del clic a coordenadas de mundo

                Vector3 NuevoClicPantalla = new Vector3(clicEnPantalla.x - minimapTopLeft.x, clicEnPantalla.y - minimapBottomRight.y, 0);
                
                float largoMainCameraX = minimapBottomRight.x - minimapTopLeft.x;
                float largoMainCameraY = minimapTopLeft.y - minimapBottomRight.y;

                float XRatio = minimapCamera.pixelWidth/largoMainCameraX;
                float YRatio = minimapCamera.pixelHeight/largoMainCameraY;

                Vector3 clicEnCamaraMinimapaConversion = new Vector3(NuevoClicPantalla.x * XRatio, NuevoClicPantalla.y * YRatio, 0); // el 51 era un 0

                Vector3 clicEnMundo = minimapCamera.ScreenToWorldPoint(new Vector3(clicEnCamaraMinimapaConversion.x, clicEnCamaraMinimapaConversion.y, minimapCamera.nearClipPlane));

                Vector3 final = new Vector3(clicEnMundo.x, 5f, clicEnMundo.z); //el 5 era 100f
                GuardadoPuntoFinal = final;
                
                this.transform.position = new Vector3(Player.transform.position.x, 5f, Player.transform.position.z);
                //GetComponent<TrailRenderer>().Clear();
                agente.destination = final; 
                
            }
        } 
        //agente.destination = objetivo.position;
        if(agente.hasPath){
            this.transform.position = new Vector3(Player.transform.position.x, 5f, Player.transform.position.z);
            DrawPath();
        }

        if(Vector3.Distance(new Vector3(agente.destination.x, 0, agente.destination.z), Player.transform.position) <= agente.stoppingDistance){
            MyLineRenderer.positionCount=0;
            agente.ResetPath();
        }

        if(Input.GetMouseButtonDown(1)){
            agente.ResetPath();
        }
    }

    void DrawPath(){
        MyLineRenderer.positionCount = agente.path.corners.Length;
        MyLineRenderer.SetPosition(0, Player.transform.position);

        if(agente.path.corners.Length < 2){
            return;
        }

        for(int i = 1; i < agente.path.corners.Length; i++){
            Vector3 pointPosition = new Vector3(agente.path.corners[i].x, agente.path.corners[i].y, agente.path.corners[i].z);
            MyLineRenderer.SetPosition(i, pointPosition);
        }
    }




}
