using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Script para objeto que dará la mision
public class QuestStarterPolo : Interactable
{

    private Button acceptButton, declineButton;
    private GameObject dialogoObjetivo;
    private GameObject canvas, player;
    public bool misionAceptada = false;


    void Awake()
    {
        //los nombres de los tres objetos uno en cada linea
        //PreservFaunaTomar
        //  Tomar
        //      Text (TMP)
        //          Si
        //          No

        canvas = GameObject.Find("Canvas");
        player = GameObject.FindGameObjectWithTag("Player");

        dialogoObjetivo = canvas.transform.GetChild(8).gameObject;
        acceptButton = canvas.transform.GetChild(8).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
        declineButton = canvas.transform.GetChild(8).transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();

        acceptButton.onClick.AddListener(acceptButton_Action);
        declineButton.onClick.AddListener(declineButton_Action);
    }

    private void Start()
    {
        

    }

    public override void Interact(){

        Cursor.lockState = CursorLockMode.None;
        
        base.Interact();

        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        dialogoObjetivo.SetActive(true);

    }

    public void acceptButton_Action()
    {
        misionAceptada = true;
        retake_control();
    }

    public void declineButton_Action()
    {
        retake_control();
    }

    void retake_control()
    {
        CharacterMovement.movementDialogue = false;
        CameraInteraction.interactionDialogue = false;
        FpsCamera.cameraDialogue = false;
        dialogoObjetivo.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }


    
}
