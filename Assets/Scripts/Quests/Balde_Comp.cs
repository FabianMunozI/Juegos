using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balde_Comp : Interactable
{
    public bool tiene_agua = false;
    private Mesh bal_con_agua;
    private Mesh bal_sin_agua;
    private GameObject water_mesh;

    void Start()
    {
        tiene_agua = false;
        water_mesh = GameObject.Find("water 2");
    }

    void Update()
    {
        if (tiene_agua)
        {
            Debug.Log("aa");
        }else{
            water_mesh.active = false;  
        }
    }
}
