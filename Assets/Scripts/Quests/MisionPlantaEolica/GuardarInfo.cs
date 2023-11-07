using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardarInfo : MonoBehaviour
{
    public int tipo;

    public GameObject planicie;
    public GameObject player;
    public GameObject npcMisionEolica;

    private string tipoPrefName = "Tipo";
    // Start is called before the first frame update
    
    void Start()
    {   
        LoadDataEolica();
        //MoverPlayer();
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

    public void MoverPlayer(){
        if (tipo == 1){
            player.transform.position = new Vector3(252, 100, -212);
        }
    }

    public void GenerarPorTipoEolica(){
        if(tipo == 1){
            npcMisionEolica.transform.GetChild(19).gameObject.SetActive(false);
            npcMisionEolica.transform.GetChild(20).gameObject.SetActive(false);
            planicie.SetActive(true);
        }
    }
}
