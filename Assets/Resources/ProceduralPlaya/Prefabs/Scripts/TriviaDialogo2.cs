using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriviaDialogo2 : Interactable
{
    public Button AButton, BButton, CButton, DButton;
    public GameObject dialogoObjetivo;
    private AudioClip correctSound, wrongSound;
    public int respuesta_correcta;
    // Start is called before the first frame update
    void Start()
    {
        AButton.onClick.AddListener(() => ButtonAction(0));
        BButton.onClick.AddListener(() => ButtonAction(1));
        CButton.onClick.AddListener(() => ButtonAction(2));
        DButton.onClick.AddListener(() => ButtonAction(3));  
    }

    public void ButtonAction(int btnNo)
    {
        switch (btnNo)
        {
            case 0:
                AButton.GetComponent<Image>().color = Color.red;
                break;
            case 1:
                BButton.GetComponent<Image>().color = Color.red;
                break;
            case 2:
                CButton.GetComponent<Image>().color = Color.red;
                break;
            case 3:
                DButton.GetComponent<Image>().color = Color.red;
                break;
            default:
                break;
        }


        switch (respuesta_correcta)
        {
            case 0:
                AButton.GetComponent<Image>().color = Color.green;
                break;
            case 1:
                BButton.GetComponent<Image>().color = Color.green;
                break;
            case 2:
                CButton.GetComponent<Image>().color = Color.green;
                break;
            case 3:
                DButton.GetComponent<Image>().color = Color.green;
                break;
            default:
                break;
        }


        if (btnNo == respuesta_correcta)
        {
            dialogoObjetivo.GetComponent<AudioSource>().PlayOneShot(Resources.Load("positive_beeps") as AudioClip, 0.7F);
        } else {
            dialogoObjetivo.GetComponent<AudioSource>().PlayOneShot(Resources.Load("negative_beeps") as AudioClip, 0.7F);
        }
        Invoke("retake_control", 1f);
    }

    public override void Interact(){


        Cursor.lockState = CursorLockMode.None;
        
        base.Interact();

        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        dialogoObjetivo.SetActive(true);

    }

    void retake_control()
    {
        CharacterMovement.movementDialogue = false;
        CameraInteraction.interactionDialogue = false;
        FpsCamera.cameraDialogue = false;
        dialogoObjetivo.SetActive(false);

        AButton.GetComponent<Image>().color = Color.white;
        BButton.GetComponent<Image>().color = Color.white;
        CButton.GetComponent<Image>().color = Color.white;
        DButton.GetComponent<Image>().color = Color.white;

        Cursor.lockState = CursorLockMode.Locked;
    }

}
