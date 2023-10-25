using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasuraScriptInteraction : Interactable
{
    GameObject Jugador;
    GameObject basuraRecogidaCont;
    // Start is called before the first frame update
    void Start()
    {
        Jugador = GameObject.Find("Player");
        basuraRecogidaCont = GameObject.Find("BasuraRecogida");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (Jugador.GetComponent<PickUpObjects>().PickedObject != null)
        {
            if(Jugador.GetComponent<PickUpObjects>().PickedObject.name == "cosaparaagarra(Clone)"
            || Jugador.GetComponent<PickUpObjects>().PickedObject.name == "cosaparaagarra")
            {  
                base.Interact();
                this.transform.parent = basuraRecogidaCont.transform;
                this.gameObject.SetActive(false);
            }
        }
    }
}
