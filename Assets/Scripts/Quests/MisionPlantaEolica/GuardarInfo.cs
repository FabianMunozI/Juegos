using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardarInfo : MonoBehaviour
{
    public int tipo;

    public GameObject planicie;
    public GameObject player;
    public Objects playerObjectScript;
    public GameObject npcMisionEolica;
    private float timer = 1.5f;

    private string tipoPrefName = "Tipo";
    private string alturaPrefName = "Altura";
    // Start is called before the first frame update
    
    void Start()
    {   
        LoadDataEolica();
        Invoke("GenerarPorTipoEolica", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTipo(){
        tipo = 1;
    }
    private void OnDestroy() {
        SaveDataEolica();
    }
    public void SaveDataEolica(){
        PlayerPrefs.SetInt(tipoPrefName, tipo);
    }

    public void LoadDataEolica(){
        tipo = PlayerPrefs.GetInt(tipoPrefName, 0);
    }

    public void GenerarPorTipoEolica(){
        if(tipo == 1){
            npcMisionEolica.transform.GetChild(19).gameObject.SetActive(false);
            npcMisionEolica.transform.GetChild(20).gameObject.SetActive(false);
            planicie.SetActive(true);
        }
    }
}
