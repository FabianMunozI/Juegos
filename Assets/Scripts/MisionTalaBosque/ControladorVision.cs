using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorVision : MonoBehaviour
{
    public Transform Ojos;
    public float rangoVision = 20f;

    public Vector3 offset = new Vector3(0f, 0.5f, 0f);

    private ControlladorNavMesh controladorNavMesh;

    void Awake(){
        controladorNavMesh = GetComponent<ControlladorNavMesh>();
    }

    public bool PuedeVerAlJugador(out RaycastHit hit, bool mirarHaciaElJugador = false){
        Vector3 vectorDireccion;

        if(mirarHaciaElJugador){
            vectorDireccion = (controladorNavMesh.perseguirObjetivo.position + offset) - Ojos.position;

        }else
        {
            vectorDireccion = Ojos.forward;
        }

        return Physics.Raycast(Ojos.position, vectorDireccion, out hit, rangoVision) && hit.collider.CompareTag("Player");
    }
    
}
