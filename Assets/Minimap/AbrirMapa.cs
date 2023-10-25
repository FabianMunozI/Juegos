using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbrirMapa : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject mapaChico;

    public GameObject mapaGrande;

    public Camera referenciaCamaraMinimapa; //9.5
    public bool abrirMapaGrande = false;

    float a;
    float b;
    float c;
    float d;
    void Start()
    {   
        
        mapaChico = GameObject.Find("ContMinimap").transform.GetChild(0).gameObject;
        mapaGrande = GameObject.Find("ContMinimap").transform.GetChild(1).gameObject;
        mapaChico.SetActive(!abrirMapaGrande);
        mapaGrande.SetActive(abrirMapaGrande);
    
        a=9.5f;
        b=150f;
        c=9.6f;
        d=149f;

        if(Equals(SceneManager.GetActiveScene().name, "Polo")){
            b=350;
            d=349;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(referenciaCamaraMinimapa.orthographicSize<a){
            referenciaCamaraMinimapa.orthographicSize=c;
        }else if(referenciaCamaraMinimapa.orthographicSize>b){
            referenciaCamaraMinimapa.orthographicSize=d;
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
