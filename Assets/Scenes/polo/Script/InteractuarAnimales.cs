using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractuarAnimales : Interactable
{
    private GameObject dialogo;

    private Vector3 movementDirection;

    private GameObject player;

    private Button op1Button, op2Button, op3Button;

    public bool animalAyudado = false;

    int respuesta_correcta = 0;

    void Start()
    {
        if (transform.name == "pinguino(Clone)")
        {
            respuesta_correcta = 0;
            dialogo = GameObject.Find("Canvas").transform.GetChild(14).gameObject;
            op1Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
            op2Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
            op3Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        }
        else if (transform.name == "orca(Clone)")
        {
            respuesta_correcta = 0;
            dialogo = GameObject.Find("Canvas").transform.GetChild(15).gameObject;
            op1Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
            op2Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
            op3Button = dialogo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        }
        else if (transform.name == "pluma(Clone)")
        {
            respuesta_correcta = 1;
            dialogo = GameObject.Find("Canvas").transform.GetChild(16).gameObject;
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

    }

    public void Op1()
    {
        
        op1Button.GetComponent<Image>().color = Color.red;
        if(respuesta_correcta == 0)
        {
            op1Button.GetComponent<Image>().color = Color.green;
        } else if(respuesta_correcta == 1)
        {
            op2Button.GetComponent<Image>().color = Color.green;
        } else if (respuesta_correcta == 2)
        {
            op3Button.GetComponent<Image>().color = Color.green;
        }
        
        Invoke("retake_control", 1f);
    }

    public void Op2()
    {
        
        op2Button.GetComponent<Image>().color = Color.red;
        if(respuesta_correcta == 0)
        {
            
            op1Button.GetComponent<Image>().color = Color.green;
        } else if(respuesta_correcta == 1)
        {
            op2Button.GetComponent<Image>().color = Color.green;
        } else if (respuesta_correcta == 2)
        {
            op3Button.GetComponent<Image>().color = Color.green;
        }
        
        Invoke("retake_control", 1f);
    }
    public void Op3()
    {
        
        op3Button.GetComponent<Image>().color = Color.red;
        if(respuesta_correcta == 0)
        {
            op1Button.GetComponent<Image>().color = Color.green;
        } else if(respuesta_correcta == 1)
        {
            op2Button.GetComponent<Image>().color = Color.green;
            
        } else if (respuesta_correcta == 2)
        {
            op3Button.GetComponent<Image>().color = Color.green;
        }
        
        Invoke("retake_control", 2f);
    }

    void retake_control()
    {
        CharacterMovement.movementDialogue = false;
        CameraInteraction.interactionDialogue = false;
        FpsCamera.cameraDialogue = false;
        dialogo.SetActive(false);

        animalAyudado = true;

        Cursor.lockState = CursorLockMode.Locked;

        op1Button.GetComponent<Image>().color = Color.white;
        op2Button.GetComponent<Image>().color = Color.white;
        op3Button.GetComponent<Image>().color = Color.white;
    }

}