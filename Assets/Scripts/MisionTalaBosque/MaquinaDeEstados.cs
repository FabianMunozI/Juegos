using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class MaquinaDeEstados : MonoBehaviour
{
    public MonoBehaviour EstadoPatrulla;
    public MonoBehaviour EstadoAlerta;
    public MonoBehaviour EstadoPersecusion;
    public MonoBehaviour EstadoInicial;

    public MonoBehaviour EstadoActual;

    public MeshRenderer MeshRendererIndicador;
    // Start is called before the first frame update
    void Start()
    {
        ActivarEstado(EstadoInicial);
    }

    public void ActivarEstado(MonoBehaviour nuevoEstado){
        if(EstadoActual!=null){
            EstadoActual.enabled=false;
        }
        EstadoActual = nuevoEstado;
        EstadoActual.enabled = true;
    }

}
