using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirMapa : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject mapaChico;

    public GameObject mapaGrande;

    public Camera referenciaCamaraMinimapa; //9.5
    public bool abrirMapaGrande = false;
    void Start()
    {   
        
        mapaChico = GameObject.Find("ContMinimap").transform.GetChild(0).gameObject;
        mapaGrande = GameObject.Find("ContMinimap").transform.GetChild(1).gameObject;
        mapaChico.SetActive(!abrirMapaGrande);
        mapaGrande.SetActive(abrirMapaGrande);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(referenciaCamaraMinimapa.orthographicSize<9.5f){
            referenciaCamaraMinimapa.orthographicSize=9.6f;
        }else if(referenciaCamaraMinimapa.orthographicSize>150f){
            referenciaCamaraMinimapa.orthographicSize=149f;
        }

        if(Input.GetKeyDown(KeyCode.M)){

            abrirMapaGrande = !abrirMapaGrande;
            mapaChico.SetActive(!abrirMapaGrande);
            mapaGrande.SetActive(abrirMapaGrande);

            Cursor.lockState = abrirMapaGrande ? CursorLockMode.Confined : CursorLockMode.Locked;
            
        }

        float v=Input.GetAxis("Mouse ScrollWheel");
        if(v!= 0f){
            referenciaCamaraMinimapa.orthographicSize+= 10*v;
        }

        if(abrirMapaGrande ==true && referenciaCamaraMinimapa.orthographicSize>65){
            mapaGrande.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            mapaGrande.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }else if(abrirMapaGrande ==true && referenciaCamaraMinimapa.orthographicSize<65){
            mapaGrande.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            mapaGrande.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
    }
}
