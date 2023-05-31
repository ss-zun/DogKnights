using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Base
{   [SerializeField]
    public Animator anim;
    private Rigidbody playerRigidbody;
    bool isJumping = false;       //공중에 떠 있는지
    [SerializeField]
    public float jumpForce = 30;
    float gravity = -9.8f;  //중력 가속도
    float yVelocity;  //y 이동값
    Vector3 moveDir;

    // 에너지 충전에 관한 변수
    private float curEnergy = 0f;     //에너지 량
    private float maxEnergy = 100f;
    private float chargeTime = 0f;    //충전 시간
    private float maxTime = 2f;

    private int heart = 0;
    private int maxHeart = 5;



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
        Charge();
    }

    override protected void Move(){
        float xInput = Input.GetAxis("Horizontal");
        if (xInput == 0){
            anim.SetBool("Run", false);
        }else{
            anim.SetBool("Run", true);
            if (xInput <= 0.0f){
            transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            if (xInput > 0.0f){
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        if(Input.GetKey(KeyCode.LeftControl) && isJumping == false){
            // playerRigidbody.AddForce(Vector3.up*jumpForce, ForceMode.Impulse );
            // isJumping = true;
            yVelocity = jumpForce;
            isJumping = true;

        }
        if(isJumping){
            yVelocity += gravity*Time.deltaTime;

        }else{
            yVelocity = 0f;
        }

        moveDir.Normalize();
        
        moveDir = transform.right*xInput;
        moveDir.y = yVelocity;

        transform.Translate(moveDir*moveSpeed*Time.deltaTime);



        
        // float zInput = Input.GetAxis("Vertical");
        if(anim.GetBool("Attack")){
            return;
        }
        // float xSpeed = xInput*moveSpeed;

        // Vector3 newVelocity = new Vector3(xSpeed, 0f, 0f);
        // playerRigidbody.velocity = newVelocity;
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
        // yVelocity += gravity*Time.deltaTime;
        if(Input.GetKey(KeyCode.LeftControl) && isJumping == false){
            playerRigidbody.AddForce(Vector3.up*jumpForce, ForceMode.Impulse );
            isJumping = true;
            // transform.forward*


        }
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ground")){
            isJumping = false;
            yVelocity = 0f;
            moveDir.y =yVelocity;
        }
        
    }

    public void AttacktedByMonster(int damage){
        if (addHeart(damage) == 0){
            heart = 0;
            Die();
        }
    }


    // heart의 개수를 add만큼 변경하고 변경 후 heart 수 반환
    private int addHeart(int add){
        heart = heart+add;
        if (heart>=5) heart = 5;
        else if(heart<=0) heart = 0;
        return heart;
    }

    // 쉬프트누를 경우 에너지 충전
    public void Charge(){
        if(Input.GetKey(KeyCode.LeftShift)){
            if (curEnergy>=100f){
                curEnergy=0;
                Debug.Log("하트 획득 : "+addHeart(1));
                return;
            }
            curEnergy+=Time.deltaTime*50;
            Debug.Log("에너지 충전... "+ curEnergy);
        }
    }

    public void Die(){

    }
   


}
