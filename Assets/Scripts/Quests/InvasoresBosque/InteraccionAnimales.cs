using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteraccionAnimales : Interactable
{
    public GameObject cagePrefab;
    public GameObject cageWithAnimalPrefab;
    GameObject Jugador;
    GameObject Iv;
    // Start is called before the first frame update
    void Start()
    {
        Jugador = GameObject.Find("Player");
        Iv = GameObject.Find("Inventario");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (Jugador.GetComponent<PickUpObjects>().PickedObject != null)
        {
            if(Jugador.GetComponent<PickUpObjects>().PickedObject.name == string.Format("{0}(Clone)", cagePrefab.name))
            {  
                base.Interact();
                this.gameObject.SetActive(false);
                Iv.GetComponent<Inventory>().SoltarObjeto();
                DestroyImmediate(Jugador.GetComponent<PickUpObjects>().PickedObject);
                Iv.GetComponent<Inventory>().SpawnHotBarItem(cageWithAnimalPrefab.GetComponent<VincularObjetoInventario>().item_vinculado);
                Iv.GetComponent<Inventory>().SpawnHBIteminHand();

            }
        }
    }


}
