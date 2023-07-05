using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   [SerializeField]
    public Animator anim;
    private Rigidbody playerRigidbody;
    bool isJumping = false;       //공중에 떠 있는지
    [SerializeField]
    public float jumpForce = 30;
    [SerializeField]
    public GameObject swordObject;
    float gravity = -9.8f;  //중력 가속도
    float yVelocity;  //y 이동값
    Vector3 moveDir;
    [SerializeField]
    private float moveSpeed = 0.1f;

    // 에너지 충전에 관한 변수
    private float curEnergy = 0f;     //에너지 량
    private float maxEnergy = 100f;
    private float chargeTime = 0f;    //충전 시간
    private float maxTime = 2f;

    public int heart = 5;
    private int maxHeart = 5;
    private bool isInvincible = false;
    private bool isDead = false;



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
        isJumping = false;
        Move();
        Attack();
        Jump();
        Charge();
    }


    // 키보드 입력에 따라 움직이기
    protected void Move(){
        float xInput = Input.GetAxis("Horizontal");
        if(isDead){
            return;
        }
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

    protected void Attack(){
        
        if(Input.GetKey(KeyCode.Z)){
            anim.SetBool("Attack", true);
        }
        else{
            anim.SetBool("Attack", false);
        }
    }

    protected void Jump(){
        // yVelocity += gravity*Time.deltaTime;
        if(Input.GetKey(KeyCode.LeftControl) && isJumping == false){
            playerRigidbody.AddForce(Vector3.up*jumpForce, ForceMode.Impulse );
            isJumping = true;
            // transform.forward*


        }
    }


    // 바닥과 닿아있을 경우
    private void OnCollisionStay(Collision collision){
        if(collision.gameObject.CompareTag("Ground")){       //바닥과 충돌 : 착지
            isJumping = false;
            yVelocity = 0f;
            moveDir.y =yVelocity;
        }
        if(collision.gameObject.CompareTag("Enemy")&&anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")){   //적과 충돌했을 때의 상태가 공격중인 경우(적한테 맞은게 아니라 플레이어가 때린 것)
            Debug.Log(collision.gameObject.name);
            
        }
        else if(collision.gameObject.CompareTag("Enemy") && isInvincible==false){        //적에게 맞았을 때 
            Debug.Log("get Attacked");
            isInvincible = true; 
            TakeDamage(1);
            Invincible();

        }
        
    }

    public void TakeDamage(int damage){
        
        anim.SetTrigger("GetHit");
        heart -= damage;
        if (heart <= 0){
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



    public void Invincible(){
        isInvincible = false;
    }


    // 플레이어 사망시 쓰러지는 모션, 특정 키 입력 시 부활하도록 함
    public void Die(){
        anim.SetTrigger("Die");
        Debug.Log("Die...");
        isDead = true;
        if(Input.GetKey(KeyCode.R)){
            Restart();
        }
    }

    // 부활하는 모션과 함께 생명 수 초기화
    public void Restart(){
        anim.SetTrigger("RecoverDie");
        heart = maxHeart;
        isDead = false;
    }
    
}
