using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class postMisionRitmo : MonoBehaviour
{
    // Start is called before the first frame update


    GameObject MisionRitmo;

    GameObject PanelPostMision;

    activarMisionRitmo referenciaOtroScript;
    GameObject player;

    bool panelActivoPostMision;
    void Start()
    {
        player = GameObject.Find("Player");
        referenciaOtroScript = gameObject.GetComponent<activarMisionRitmo>();
        panelActivoPostMision = false;
        MisionRitmo = GameObject.Find("Critmo");
        PanelPostMision = MisionRitmo.transform.GetChild(5).gameObject;

        //referenciaOtroScript.MisionRitmo.transform.GetChild(0).GetChild(0).GetComponent<RitmoTeclas>().DesactivarScriptsPlayer(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && panelActivoPostMision){
            panelActivoPostMision = false;
            PanelPostMision.SetActive(false);
            referenciaOtroScript.MisionRitmo.transform.GetChild(0).GetChild(0).GetComponent<RitmoTeclas>().DesactivarScriptsPlayer(true);
        }
        
    }

    public void infoPostMision(){ // preguntar mision
        PanelPostMision.SetActive(true);

        panelActivoPostMision = true;
        SetActivePlayerScripts(false);
        /* MisionRitmo.transform.GetChild(1).gameObject.SetActive(true); // aceptar o rechazar mision
        Cursor.lockState = CursorLockMode.Confined;
        SetActivePlayerScripts(false); */

    }

    void SetActivePlayerScripts(bool valor){
        player.GetComponent<AbrirMapa>().enabled = valor;
        player.GetComponent<Mascota>().enabled = valor;
        player.GetComponent<CharacterMovement>().enabled = valor;
        player.GetComponent<FpsCamera>().enabled = valor;
        player.GetComponent<CameraInteraction>().enabled = valor;
    }
}
