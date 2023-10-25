using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTutorial : Interactable
{
    GameObject player;
    public int utilizar;

    public GameObject Informativo;

    public GameObject Buscar;

    void Start()
    {
        if(utilizar==0){
            Informativo=GameObject.Find("Hielo").transform.GetChild(0).gameObject;
        }else if(utilizar==1){
            Informativo=GameObject.Find("InfoBosque").transform.GetChild(0).gameObject;
        }else if(utilizar==2){
            Informativo=GameObject.Find("InfoCiudad").transform.GetChild(0).gameObject;
        }
        Buscar = GameObject.Find("Mirarhorizonte");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void teleporthielo0 ()
    {
        Vector3 newposition = new Vector3 (-25.26f, 5.19f, 10.5f);

        player.transform.position = newposition;
    }

    public void teleportbosque1 ()
    {
        Vector3 newposition = new Vector3 (-25.26f, 5.19f, 130f);

        player.transform.position = newposition;
    }

    public void teleportcity2 ()
    {
        Vector3 newposition = new Vector3 (-25.26f, 5.19f, 247f);

        player.transform.position = newposition;
    }

    public void prenderplayer (){
        player.GetComponent<CharacterMovement>().enabled = true;
        player.GetComponent<FpsCamera>().enabled = true;
        player.GetComponent<AbrirMapa>().mapaChico.SetActive(true);
        player.GetComponent<AbrirMapa>().enabled=true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public override void Interact()
    {
        base.Interact();
        
        if(utilizar==0){
            teleporthielo0();
        }
        else if(utilizar==1){
            teleportbosque1();
        }
        else if(utilizar==2){
            teleportcity2();
        }
        Informativo.SetActive(true);
        player.transform.LookAt(Buscar.transform);
        //menuCambioDeMapa.SetActive(true);
        
        player.GetComponent<CharacterMovement>().enabled = false;
        player.GetComponent<FpsCamera>().enabled = false;
        if(player.GetComponent<AbrirMapa>().abrirMapaGrande == false){
            player.GetComponent<AbrirMapa>().mapaChico.SetActive(false);
        }
        player.GetComponent<AbrirMapa>().enabled=false;
        Cursor.lockState = CursorLockMode.Confined;
        
        //SceneManager.LoadScene(nombreScenaCambiar);
    }


}
