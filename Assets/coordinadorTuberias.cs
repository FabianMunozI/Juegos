using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coordinadorTuberias : MonoBehaviour
{
    public GameObject aguaLimpia;
    public GameObject cañeriadobladaDadaVuelta;
    public GameObject ocultarCañeriaLarga;
    public GameObject aguaSucia;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("PalancaPlantaTratamientoAgua")){
            //Debug.Log(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua"));
            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=2){
                gameObject.transform.Rotate(Vector3.up, 180f);
                aguaSucia.SetActive(false);
                /* ocultarCañeriaLarga.SetActive(false); */
            }

            if(PlayerPrefs.GetInt("PalancaPlantaTratamientoAgua")>=3){
                //aguaLimpia.transform.position += new Vector3(0,6f,0);
                aguaLimpia.SetActive(true);
                /* aguaLimpia.SetActive(true);
                aguaLimpia.GetComponent<ParticleSystem>().Play(); */
            }
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
