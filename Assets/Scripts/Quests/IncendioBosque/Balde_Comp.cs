using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balde_Comp : Interactable
{
    public bool tiene_agua = false;
    private Mesh bal_con_agua;
    private Mesh bal_sin_agua;
    private GameObject water_mesh;
    private GameObject water_particles;

    void Start()
    {
        tiene_agua = false;
        water_mesh = GameObject.Find("water 2");
        water_particles = GameObject.Find("Water_particles");
    }

    void Update()
    {
        if (tiene_agua)
        {
            water_mesh.SetActive(true);  
            water_particles.SetActive(true);
        }else{
            water_mesh.SetActive(false);  
            water_particles.SetActive(false);
        }
    }
}
