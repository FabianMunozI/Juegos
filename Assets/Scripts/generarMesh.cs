using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.AI;
//using UnityEngine.AIModule;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class generarMesh : MonoBehaviour
{
    // Start is called before the first frame update
    public List<NavMeshSurface> surfaces;

    public GameObject sePuede;

    GameObject plano;

    //GameObject edificios;
    void Start()
    {
        //edificios = GameObject.Find("Edificios");
        //sePuede = GameObject.Find("AgenteController");
        surfaces = new List<NavMeshSurface>();


        plano= GameObject.Find("RuntimeMesh");
        surfaces.Add(plano.GetComponent<NavMeshSurface>());
        surfaces[0].BuildNavMesh(); 

        /* for(int i =0; i< edificios.transform.childCount; i++){
            surfaces.Add(edificios.transform.GetChild(i).GetComponent<NavMeshSurface>());
            surfaces[i].BuildNavMesh();
        }
        surfaces.Add(plano.GetComponent<NavMeshSurface>());
        surfaces[edificios.transform.childCount].BuildNavMesh();  */
        
        //NavMeshBuilder.BuildNavMeshData();
        //

        Invoke("iniciarAgente",0.1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void iniciarAgente(){
        sePuede.SetActive(true);
        
        Invoke("delayPlano",0.1f);
        /* sePuede.GetComponent<NavMeshController>().enabled=true;
        sePuede.GetComponent<NavMeshAgent>().enabled=true;
        NavMeshAgent agent = sePuede.GetComponent<NavMeshAgent>();
        if(!agent.isOnNavMesh) {
            transform.position = this.transform.position;
            agent.enabled = false;
            agent.enabled = true;
        } */
    }

    void delayPlano(){
        plano.GetComponent<MeshRenderer>().enabled=false;
        sePuede.transform.position = new Vector3(sePuede.transform.position.x, sePuede.transform.position.y,sePuede.transform.position.z);
    }

}
