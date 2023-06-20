using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{


    // variables de movimiento
    public float speed;
    public float speedUsar;
    public float Runspeed;
    public float distance_to_ground;
    public float jumpforce;
    private new Rigidbody rigidbody;


    // Variables Control Agacharse
    public Vector3 InitialPos;
    Vector3 controlPos;
    public bool EstaParado=true;


    //Variables Animaciones
    public Animator PlayerAnimatorController;

    public bool AnimacionOn;


    // Start is called before the first frame update
    void Start()
    {
        AnimacionOn=true;

        InitialPos = transform.GetChild(0).position - transform.position;
        controlPos = transform.position - new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y-0.5f, transform.GetChild(0).position.z);

        rigidbody = GetComponent<Rigidbody>();
        //distance_to_ground = GetComponent<Collider>().bounds.extents.y;
        speedUsar=speed;
        PlayerAnimatorController = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    public void UpdateMovement()
    {
        

            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");
       
            Vector3 velocity;

            if (hor!=0 || ver!= 0){
                velocity = (transform.forward * ver + transform.right * hor).normalized * speedUsar;
            } else{
                velocity = Vector3.zero;
            }

        //PlayerAnimatorController.SetFloat("WalkVelocity", velocity.magnitude * speedUsar);

            velocity.y = rigidbody.velocity.y;
        //PlayerAnimatorController.SetFloat("PlayerVerticalVelocity", velocity.y);
            rigidbody.velocity = velocity;

        

        
  
    }

    private bool IsGrounded(){
        return Physics.BoxCast(transform.position, new Vector3(0.4f, 0f, 0.4f), Vector3.down, Quaternion.identity, distance_to_ground);
    }
    public void UpdateJump(){
        bool Grounded = IsGrounded();
        //PlayerAnimatorController.SetBool("IsGrounded", Grounded);
        if (Input.GetButtonDown("Jump") && Grounded){
            //PlayerAnimatorController.SetTrigger("PlayerJump");
            rigidbody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }
    }

    void Update(){
        if(AnimacionOn){

            if(Input.GetKeyDown(KeyCode.LeftShift)){
                speedUsar=Runspeed;
            //Debug.Log("aqui");
            }else if(Input.GetKeyUp(KeyCode.LeftShift)){
                speedUsar=speed;
            }
            UpdateMovement();
            UpdateJump();

            if(Input.GetButtonDown("Fire1")){
                EstaParado=!EstaParado; // parte en start como true
            }


            if(EstaParado==true){
                transform.GetChild(0).position = transform.position + InitialPos;
            }else if (EstaParado==false){
                transform.GetChild(0).position = transform.position - controlPos;
            }

        }


        
    }

    private void OnAnimatorMove() {
        
    }
}   

