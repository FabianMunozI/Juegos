using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generaBasura : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] basuraGenerar;
    int contador=0;

    float init=0.5f;
    bool sehizo=false;
    void Start()
    {
        /* generar();
        Invoke("generar", 5f);
        Invoke("generar", 7.5f);
        Invoke("generar", 10f);
        Invoke("generar", 12.5f);
        Invoke("generar", 15f);
        Invoke("generar", 17.5f);
        Invoke("generar", 20f); */
        
    }

    // Update is called once per frame
    void Update()
    {
        if(sehizo==false){
            sehizo=true;

            for(int i=0; i<62; i++){
                Invoke("generar", init);

                init+=0.5f;
            }
        }
    }

    void generar(){
        int mult=3;
        int positivo=1;
        if(contador%2 ==1){
            positivo=-1;
        }

        if(contador==basuraGenerar.Length){
            contador=0;
            GameObject generar = Instantiate(basuraGenerar[contador], transform.position, Quaternion.Euler(positivo*45,45,positivo*45) );
            generar.SetActive(true);
            generar.GetComponent<Rigidbody>().AddForce(new Vector3(45*positivo, 45*mult,45*positivo));

            Destroy(generar, 5f);
            // agregar a cada objeto en la lista de gameobjects a generar un script de auto eliminacion despues de x segundos
        }else{
            GameObject generar = Instantiate(basuraGenerar[contador], transform.position, Quaternion.Euler(positivo*45,45,positivo*45) );
            generar.SetActive(true);
            Destroy(generar, 5f);
            generar.GetComponent<Rigidbody>().AddForce(new Vector3(45*positivo, 45*mult,45*positivo));
            contador+=1;
        }

        


        
    }
}
