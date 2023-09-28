using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scriptCantArboles : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI quest_text;
    public float cantidad;
    float tiempoDesaparecer=6f;
    void Start()
    {
        quest_text = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        quest_text.text = "Arboles cortados : " + (cantidad).ToString("000");
    }

    // Update is called once per frame
    void Update()
    {
        tiempoDesaparecer -= Time.deltaTime;
        if(tiempoDesaparecer<=0){
            gameObject.SetActive(false);
        }
    }
}
