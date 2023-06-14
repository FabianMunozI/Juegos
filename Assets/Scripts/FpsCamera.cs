using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCamera : MonoBehaviour
{

    public Vector2 sensibility;
    public new Transform camera;
    // Start is called before the first frame update
    void Start()
    {

        //camera = transform.Find("Camera");
        Cursor.lockState = CursorLockMode.Locked;
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float hor = Input.GetAxis("Mouse X");
        float ver = Input.GetAxis("Mouse Y");
        if(hor != 0){
            transform.Rotate(Vector3.up * hor * sensibility.x);
        }
        if(ver != 0){
            //camera.Rotate(Vector3.left * ver * sensibility.y);
            float angle = (camera.localEulerAngles.x - ver * sensibility.y + 360)% 360;
            if (angle > 180){angle -= 360;};
            angle = Mathf.Clamp(angle, -80, 80);
            camera.localEulerAngles = Vector3.right * angle;
        }
        
    }
}

