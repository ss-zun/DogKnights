using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Base
{   [SerializeField]
    public Animator anim;
    private Rigidbody playerRigidbody;
    bool isJumping = false;
    [SerializeField]
    public float jumpForce = 0;



    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
        Jump();
    }

    override protected void Move(){
        float xInput = Input.GetAxis("Horizontal");
        if (xInput == 0){
            anim.SetBool("Run", false);
        }else{
            anim.SetBool("Run", true);
            if (xInput <= 0.0f){
            transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            if (xInput > 0.0f){
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        
        // float zInput = Input.GetAxis("Vertical");
        if(anim.GetBool("Attack")){
            return;
        }
        float xSpeed = xInput*moveSpeed;

        Vector3 newVelocity = new Vector3(xSpeed, 0f, 0f);
        playerRigidbody.velocity = newVelocity;
    }

    override protected void Attack(){
        if(Input.GetKey(KeyCode.Z)){
            anim.SetBool("Attack", true);
        }
        else{
            anim.SetBool("Attack", false);
        }
    }

    override protected void Jump(){
        if(Input.GetKey(KeyCode.LeftControl) && isJumping == false){
            playerRigidbody.AddForce(Vector3.up*jumpForce, ForceMode.Impulse );
            isJumping = true;

        }
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ground")){
            isJumping = false;
        }
    }



}
