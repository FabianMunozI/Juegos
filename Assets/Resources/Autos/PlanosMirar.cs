using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanosMirar : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, transform.up);
        //Vector3 v3 = player.transform.position - transform.position;
		//v3.x = 90.0f;
		//transform.rotation = Quaternion.LookRotation(-v3);
    }
}
