using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoPatrulla : MonoBehaviour
{
    public Transform[] WayPoint;
    public Color colorEstado;

    private ControlladorNavMesh controladorNavMesh;
    private int siguienteWayPoint;

    private MaquinaDeEstados maquinaDeEstados;
    private ControladorVision controladorVision;

    void Awake(){
        controladorNavMesh = GetComponent<ControlladorNavMesh>();
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorVision = GetComponent<ControladorVision>();
    }

    // Update is called once per frame
    void Update()
    {
        // ve al jugador?
        RaycastHit hit;
        if(controladorVision.PuedeVerAlJugador(out hit)){
            controladorNavMesh.perseguirObjetivo = hit.transform;
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPersecusion);
            return;
        }

        if(controladorNavMesh.HemosLlegado()){
            siguienteWayPoint = (siguienteWayPoint +1) % WayPoint.Length;
            ActualizarWayPointDestino();
        }
    }

    void OnEnable() {
        //siguienteWayPoint = 0;
        maquinaDeEstados.MeshRendererIndicador.material.color = colorEstado;
        ActualizarWayPointDestino();
    }

    void ActualizarWayPointDestino(){
        controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent(WayPoint[siguienteWayPoint].position);
    }

    public void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoAlerta);
        }
    }
}
