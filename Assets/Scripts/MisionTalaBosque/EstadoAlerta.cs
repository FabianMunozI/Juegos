using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoAlerta : MonoBehaviour
{
    public float velocidadGiroBusqueda = 120f;
    public float duracionBusqueda = 4f;

    public Color ColorEstado = Color.yellow;

    private MaquinaDeEstados maquinaDeEstados;
    private ControlladorNavMesh controladorNavMesh;
    private float tiempoBuscando;
    private ControladorVision controladorVision;
    
    void Awake(){
        controladorNavMesh = GetComponent<ControlladorNavMesh>();
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorVision = GetComponent<ControladorVision>();
    }

    void OnEnable(){
        maquinaDeEstados.MeshRendererIndicador.material.color = ColorEstado;
        controladorNavMesh.DetenerNavMeshAgent();
        tiempoBuscando = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(controladorVision.PuedeVerAlJugador(out hit)){
            controladorNavMesh.perseguirObjetivo = hit.transform;
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPersecusion);
            return;
        }

        transform.Rotate(0f, velocidadGiroBusqueda* Time.deltaTime, 0f);
        tiempoBuscando += Time.deltaTime;
        if (tiempoBuscando>= duracionBusqueda){
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPatrulla);
            return;
        }
    }
}
