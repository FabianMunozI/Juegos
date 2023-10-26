using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractuarAnimales : Interactable
{
    private GameObject dialogo;
    private Vector3 movementDirection;
    private Vector3 playerPosition;
    private Vector3 npcPosition;
    private GameObject player;

    private Button op1Button, op2Button, op3Button;

    public bool animalAyudado = false;

    void Start()
    {
        if (transform.name == "pinguino(Clone)")
        {
            dialogo = GameObject.Find("Canvas").transform.GetChild(14).transform.GetChild(0).gameObject;
            op1Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
            op2Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
            op3Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        }
        else if (transform.name == "orca(Clone)")
        {
            dialogo = GameObject.Find("Canvas").transform.GetChild(15).transform.GetChild(1).gameObject;
            op1Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
            op2Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
            op3Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        }
        else
        {
            dialogo = GameObject.Find("Canvas").transform.GetChild(16).transform.GetChild(2).gameObject;
            op1Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
            op2Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
            op3Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        }

        op1Button.onClick.AddListener(Op1);
        op2Button.onClick.AddListener(Op2);
        op3Button.onClick.AddListener(Op3);
    }


    void Update()
    {
        movementDirection = transform.forward;
    }
   
    public override void Interact()
    {

        Cursor.lockState = CursorLockMode.None;

        base.Interact();

        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        dialogo.SetActive(true);

        Radar.targets.Remove(transform);
        animalAyudado = true;
    }

    public void Op1()
    {
        retake_control();
    }

    public void Op2()
    {
        retake_control();
    }
    public void Op3()
    {
        retake_control();
    }

    void retake_control()
    {
        CharacterMovement.movementDialogue = false;
        CameraInteraction.interactionDialogue = false;
        FpsCamera.cameraDialogue = false;
        dialogo.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

}
