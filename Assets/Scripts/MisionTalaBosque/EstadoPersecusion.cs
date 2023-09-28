using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoPersecusion : MonoBehaviour
{
    public Color ColorEstado = Color.red;

    private MaquinaDeEstados maquinaDeEstados;
    private ControlladorNavMesh controladorNavMesh;
    private ControladorVision controladorVision;
    private MisionTala misionTala;
    private float tiempoBuscando;
    // Start is called before the first frame update
    void Awake(){
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<ControlladorNavMesh>();
        controladorVision = GetComponent<ControladorVision>();
        misionTala = transform.parent.parent.GetComponent<MisionTala>();
    }

    void OnEnable(){
        maquinaDeEstados.MeshRendererIndicador.material.color = ColorEstado;
        tiempoBuscando = 3f;
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(!controladorVision.PuedeVerAlJugador(out hit, true)){
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoAlerta);

            transform.GetChild(21).GetChild(2).gameObject.SetActive(false);
            transform.GetChild(21).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(21).GetChild(0).gameObject.SetActive(false);
            return;
        }
        controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent();

        tiempoBuscando -= Time.deltaTime;
        if(tiempoBuscando>2.5f){ // prender el numero 3
            transform.GetChild(21).GetChild(2).gameObject.SetActive(true);
        }
        else if(tiempoBuscando>1.5f) // prender el numero 2
        {
            transform.GetChild(21).GetChild(2).gameObject.SetActive(false);
            transform.GetChild(21).GetChild(1).gameObject.SetActive(true);
        }else if(tiempoBuscando>0.5f){// prender el numero 1
            transform.GetChild(21).GetChild(2).gameObject.SetActive(false);
            transform.GetChild(21).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(21).GetChild(0).gameObject.SetActive(true);
        }
        else if (tiempoBuscando<= 0f){
            transform.GetChild(21).GetChild(0).gameObject.SetActive(false);

            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPatrulla);

            misionTala.VolverCheckPoint();

        }

    }
}
