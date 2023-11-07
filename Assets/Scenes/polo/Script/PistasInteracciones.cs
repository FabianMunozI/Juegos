using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistasInteracciones : Interactable
{
    private GameObject dialogo;
    private Vector3 movementDirection;
    private Vector3 playerPosition;
    private Vector3 npcPosition;
    private GameObject player;

    private Vector3 centroZona;
    [SerializeField] private GameObject zonaMiniMapa;

    void Start()
    {
        if (transform.name == "ninia(Clone)")
        {
            dialogo = GameObject.Find("Canvas").transform.GetChild(4).transform.GetChild(0).gameObject;
        }
        else if (transform.name == "huesoPescado(Clone)")
        {
            dialogo = GameObject.Find("Canvas").transform.GetChild(4).transform.GetChild(1).gameObject;
        }
        else if (transform.name == "pluma(Clone)")
        {
            dialogo = GameObject.Find("Canvas").transform.GetChild(4).transform.GetChild(2).gameObject;
        }
    }

    
    void Update()
    {
        movementDirection = transform.forward;
    }
    public override void Interact()
    {

        base.Interact();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
        playerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
        npcPosition = new Vector3(transform.position.x, 1, transform.position.z);

        Quaternion rotation = Quaternion.LookRotation(playerPosition - transform.position);
        
        ///////////Rotacion del jugador al NPC////////////////////
        rotation = Quaternion.LookRotation(npcPosition - player.transform.position);
        rotation.x = 0f;
        rotation.z = 0f;
        player.transform.rotation = rotation;
        //////////////////////////////////////////////////////////

        CharacterMovement.movementDialogue = true;
        CameraInteraction.interactionDialogue = true;
        FpsCamera.cameraDialogue = true;
        NpcNav.isInDialogue = true;
        Cursor.lockState = CursorLockMode.Confined;

        dialogo.SetActive(true);


        //mi cosecha

        GameObject aux;

        centroZona = new Vector3(Random.Range(-1100, 1100), 0, Random.Range(-1100, 1100));
        while (!CompareTwo(centroZona, transform.position,700))
        {
            centroZona = new Vector3(Random.Range(-1100, 1100), 0, Random.Range(-1100, 1100));
        }


        aux = Instantiate(zonaMiniMapa, new Vector3(centroZona.x,150f,centroZona.z), Quaternion.identity);
        aux.transform.localScale = new Vector3(200, 200, 200);
        //objetosMision.Add(aux);

        Radar.targets.Remove(transform);

        //Debug.Log( Los target del radar)

        if (transform.name == "ninia(Clone)")
        {
            aux.name = "zonaOrca";
        }
        else if (transform.name == "huesoPescado(Clone)")
        {
            aux.name = "zonaFoca";
        }
        else
        {
            aux.name = "zonaPengu";
        }

        // mirar hacia la orca pero un poco ladeado, como mirar hacia la zona
        rotation = Quaternion.LookRotation(centroZona);
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;
    }


    private static bool CompareTwo(Vector3 a, Vector3 b, float distanciaZonas)
    {
        float x = a.x - b.x;
        float z = a.z - b.z;

        if ((x * x + z * z) < (distanciaZonas * distanciaZonas))
            return false;

        return true;
    }
}
