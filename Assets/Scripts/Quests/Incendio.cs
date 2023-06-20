using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Incendio : MonoBehaviour
{
    private bool Quest_started;
    public Material color1;
    public Material color2;
    private GameObject Llamas;
    public TextMeshProUGUI quest_text;
    private int n_llamas;
    public GameObject Balde_Agua;
    public GameObject Pozo;

    void Start()
    {
        Quest_started = false;
        Llamas = GameObject.Find("Llamas");
        Llamas.SetActive(false);
        quest_text.enabled = false;
        n_llamas = Llamas.transform.childCount;

        Balde_Agua = GameObject.Find("Wooden_Bucket");
        Balde_Agua.GetComponent<Outline>().enabled = false;

        Pozo = GameObject.Find("SM_Bld_Well_01");
        Pozo.GetComponent<Outline>().enabled = false;
    }

    void Update()
    {

        int llamas_restantes = counter_actives(Llamas);
        if (Quest_started)
        {
            quest_text.text = "¡Apaga los Arboles!\n- "+(n_llamas - llamas_restantes )+"/"+n_llamas+" Arboles Apagados";
        }


        if (llamas_restantes == 0)
        {
            quest_text.text = "¡Mision Completada!";
            Quest_started = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            this.gameObject.GetComponent<MeshRenderer>().material.color = color1.color;

            if (!Quest_started){
                Quest_started = true;
                Llamas.SetActive(true);
                quest_text.enabled = true;
                Balde_Agua.GetComponent<Outline>().enabled = true;
                Pozo.GetComponent<Outline>().enabled = true;
            }
            
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            this.gameObject.GetComponent<MeshRenderer>().material.color = color2.color;
        }
    }

    private int counter_actives(GameObject currentObj)
    {
        int count = 0;
        for (int i = 0; i < currentObj.transform.childCount; i++)
        {
            if(currentObj.transform.GetChild(i).gameObject.activeSelf)
            count++;
        }
        return count;
    }
}
