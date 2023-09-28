using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanciaPlayer : MonoBehaviour
{
    ControlladorNavMesh controladorNavMesh;
    MisionTala misionTala;
    MaquinaDeEstados maquinaDeEstados;
    Animator controlador;
    void Awake(){
        controladorNavMesh = GetComponent<ControlladorNavMesh>();
        misionTala = transform.parent.parent.GetComponent<MisionTala>();
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controlador = GetComponent<Animator>();
        controlador.SetTrigger("Tala");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(controladorNavMesh.perseguirObjetivo != null){
            if(Vector3.Distance(transform.position, controladorNavMesh.perseguirObjetivo.position) < 3 && maquinaDeEstados.EstadoActual == maquinaDeEstados.EstadoPersecusion){
                misionTala.VolverCheckPoint();
            }
            if(Vector3.Distance(transform.position, controladorNavMesh.perseguirObjetivo.position) <1.5f){
                misionTala.VolverCheckPoint();
            }
        }
        
    }
}
