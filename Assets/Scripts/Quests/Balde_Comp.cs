using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balde_Comp : Interactable
{
    public bool tiene_agua = false;
    private Mesh bal_con_agua;
    private Mesh bal_sin_agua;

    void Start()
    {
        tiene_agua = false;
    }
}
