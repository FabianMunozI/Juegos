using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriviaDialogo : Interactable
{
    public Button AButton, BButton, CButton, DButton;
    public GameObject dialogoObjetivo;
    public AudioClip correctSound, wrongSound;
    // Start is called before the first frame update
    void Start()
    {
        dialogoObjetivo = FindInActiveObjectByName("TriviaList");

        AButton = dialogoObjetivo.transform.GetChild(1).GetComponent<Button>();
        BButton = dialogoObjetivo.transform.GetChild(2).GetComponent<Button>();
        CButton = dialogoObjetivo.transform.GetChild(3).GetComponent<Button>();
        DButton = dialogoObjetivo.transform.GetChild(4).GetComponent<Button>();

        if (MisionTrivia.listerners_botones == 0)
        {
            AButton.onClick.AddListener(() => ButtonAction(0));
            BButton.onClick.AddListener(() => ButtonAction(1));
            CButton.onClick.AddListener(() => ButtonAction(2));
            DButton.onClick.AddListener(() => ButtonAction(3));
            MisionTrivia.listerners_botones ++;
        }
        
    }

    GameObject FindInActiveObjectByName(string name)
{
    Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
    for (int i = 0; i < objs.Length; i++)
    {
        if (objs[i].hideFlags == HideFlags.None)
        {
            if (objs[i].name == name)
            {
                return objs[i].gameObject;
            }
        }
    }
    return null;
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


        switch (MisionTrivia.respuestas[MisionTrivia.intento_actual])
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


        if (btnNo == MisionTrivia.respuestas[MisionTrivia.intento_actual])
        {
            dialogoObjetivo.GetComponent<AudioSource>().PlayOneShot(Resources.Load("positive_beeps") as AudioClip, 0.7F);
            MisionTrivia.cont_correctas ++;
        } else {
            dialogoObjetivo.GetComponent<AudioSource>().PlayOneShot(Resources.Load("negative_beeps") as AudioClip, 0.7F);
        }

        MisionTrivia.intento_actual ++;
        Invoke("retake_control", 1f);
    }

    public override void Interact(){


        Cursor.lockState = CursorLockMode.None;
        
        base.Interact();

        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        dialogoObjetivo.SetActive(true);

        // Actualizar Trivia
        dialogoObjetivo.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = MisionTrivia.textos_sup[MisionTrivia.intento_actual];
        AButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = MisionTrivia.texto_botones[MisionTrivia.intento_actual,0];
        BButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = MisionTrivia.texto_botones[MisionTrivia.intento_actual,1];
        CButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = MisionTrivia.texto_botones[MisionTrivia.intento_actual,2];
        DButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = MisionTrivia.texto_botones[MisionTrivia.intento_actual,3];

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
