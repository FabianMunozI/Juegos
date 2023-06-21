using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsablePickedObject : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(this.GetComponent<PickUpObjects>().PickedObject != null){
            if(Input.GetKeyDown(KeyCode.T)){ // llamar codigo de usar objeto
                if(this.GetComponent<PickUpObjects>().PickedObject.CompareTag("pelotaRecogible")){ // si tengo una pelota, primero la suelto
                    // y luego uso el objeto
                    GameObject pelotaRecogida=GetComponent<PickUpObjects>().PickedObject;
                    pelotaRecogida.GetComponent<Interactable>().Interact();

                    //ahora que solto el objeto (en caso de que sea una pelota) llama a la funcion usar del objeto
                    pelotaRecogida.GetComponent<Usable>().usable();

                }else if(this.GetComponent<PickUpObjects>().PickedObject.CompareTag("basura") && GetComponent<CameraInteraction>().ultimoReconocidoBasura){ // 1ro se suelta y luego ...
                    GameObject basura=GetComponent<PickUpObjects>().PickedObject;
                    //Debug.Log(GetComponent<CameraInteraction>().ultimoReconocidoBasura); // basurero
                    basura.GetComponent<Interactable>().Interact();
                    
                    GameObject basureroSeleccionado = GetComponent<CameraInteraction>().ultimoReconocidoBasura;
                    basureroSeleccionado.GetComponent<BasureroReciclaje>().objetoPickeadoReferencia=basura;
                   
                    basureroSeleccionado.GetComponent<Usable>().usable(); // falta aki
                }

                



            }
        }


    }
}
