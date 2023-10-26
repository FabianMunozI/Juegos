using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionTrigger : Interactable
{   
    public GameObject objectObjetivo; 
     [SerializeField] UnityEvent onTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact(){
        base.Interact();
        onTrigger.Invoke();
    }
}
