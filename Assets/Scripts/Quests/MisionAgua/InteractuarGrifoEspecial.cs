using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuarGrifoEspecial : Interactable
{
    public AguaSurtidor aguaSurtidor;
    public static bool misionActiva;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Interact()
    {
        player = GameObject.Find("Player");
        if(player.GetComponent<PickUpObjects>().PickedObject.CompareTag("llaveAgua")){
            base.Interact();
            aguaSurtidor.interactuado = true;
        }
        
    }
}
