using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuarGrifo : Interactable
{
    // Start is called before the first frame update

    public AguaSurtidor aguaSurtidor;
    public static bool misionActiva;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivarInteraccion(){
        gameObject.SetActive(true);
    }
    public override void Interact()
    {
        base.Interact();
        aguaSurtidor.interactuado = true;
    }
}
