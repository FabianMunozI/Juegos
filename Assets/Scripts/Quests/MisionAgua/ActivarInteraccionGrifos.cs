using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarInteraccionGrifos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivarInteraccion(){
        for(int i = 1; i < gameObject.transform.childCount; i++){
            gameObject.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
            gameObject.transform.GetChild(i).GetChild(3).gameObject.SetActive(true);
        }
    }
}
