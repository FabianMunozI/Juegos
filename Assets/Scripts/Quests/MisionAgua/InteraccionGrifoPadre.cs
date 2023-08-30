using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteraccionGrifoPadre : Interactable
{
    public AguaSurtidor aguaSurtidor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Interact()
    {
        base.Interact();
        aguaSurtidor.interactuado = true;
        gameObject.transform.parent.GetChild(1).gameObject.SetActive(true);
        Destroy(gameObject.GetComponent<InteraccionGrifoPadre>());
    }
}
