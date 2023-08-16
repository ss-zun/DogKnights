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
    public float attackPower = 10f;
    [SerializeField]
    public GameObject swordObject;
    float gravity = -9.8f;  //중력 가속도
    float yVelocity;  //y 이동값
    Vector3 moveDir;
    [SerializeField]
    private float moveSpeed = 0.1f;
    [SerializeField]
    private GameObject root;
    //private float defense = 100f;

    SkinnedMeshRenderer[] meshs;
    bool isDamage = false;
    int health;


    Color red = new Color(1f, 0f, 0f, 0.5f);
    Color blue = new Color(0f, 0f, 1f, 0.5f);

    // 에너지 충전에 관한 변수
    private float curEnergy = 0f;     //에너지 량
    private float maxEnergy = 100f;
    private float chargeTime = 0f;    //충전 시간
    //private float maxTime = 2f;
    float attackTime = 0f;

    public int heart = 5;     //현재 하트 수
    private int maxHeart = 5; //최대 하트 수

    private bool isInvincible = false;  //현재 무적상태인지
    private bool isDead = false;  //현재 사망상태인지
    public bool isAttacking = false; //공격상태인지
    private bool isCharging = false; //에너지를 모으는 중 인지 
    private bool isDefense = false; //방어상태인지
    private bool isPower = false; //필살기 사용 중인지
    // 플레이어 상태에 관한 boolean 변수들은 주로 이펙트를 적용하기 위해 정의

    private float invincibleTime = 2.0f; //무적 상태 2초동안 유지
    float curTime = 0f; //무적상태를 유지한 시간

    Renderer playerColor;//플레이어 material 색상이 붉게 깜빡거리도록 함

    [SerializeField]
    private GameObject[] effectPrefabs; // 캐릭터의 이펙트 : 피격 시 이펙트, 공격 시 이펙트, 에너지 모을 때의 이펙트 등
    // 0:slash, 1:hit, 2:restart, 3:heart get, 4:charging, 5:get hitted, 6:defense, 7:explosion, 8:slash, 9:hit

    GameObject chargeEffect = null;
    GameObject defenseEffect = null;
    GameObject attackEffect = null;
    GameObject[] effectsGO;
    public Vector3 savePoint = Vector3.zero;   //사망한 후 재시작 할 때 스폰할 지점

    // Start is called before the first frame update
    void Start()
    {
        swordObject.GetComponent<BoxCollider>().enabled = false;
        InitEffect();
        playerRigidbody = GetComponent<Rigidbody>();
        playerColor = transform.Find("polySurface1").gameObject.GetComponent<Renderer>();
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
        Restart();
        Invincible();
        Defense();
        PowerAttack();
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("PowerAttack")){
            isInvincible = true;

        }
    }

    void InitEffect(){

        chargeEffect = GenerateEffect(4, transform.position);
        chargeEffect.SetActive(false);
        defenseEffect = GenerateEffect(6, transform.position);
        defenseEffect.SetActive(false);
        attackEffect = GenerateEffect(0, transform.position);
        attackEffect.SetActive(false);
        attackEffect.transform.SetParent(root.transform);
        attackEffect.transform.position = Vector3.zero;
        attackEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);

    }


    // 키보드 입력에 따라 움직이기
    protected void Move(){
        float xInput = Input.GetAxis("Horizontal");
        if(isDead || isCharging || isDefense || isAttacking){  // 사망했거나 방어 자세이거나 에너지 충전중이라면 움직이지 않도록 함
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
        //int effectCount = 0;
        if(Input.GetKey(KeyCode.Z)){
            anim.SetBool("Attack", true);
            if (isAttacking == false){
                swordObject.GetComponent<BoxCollider>().enabled = true;
                //effectCount = 1;
                Vector3 effectPosition = new Vector3(transform.position.x+0.8f, transform.position.y+0.8f, 0f);
                //GameObject go = GenerateEffect(0, effectPosition);
                //attackEffect.transform.position = effectPosition;
                attackEffect.SetActive(true);
                //attackEffect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                //attackEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);
                //Destroy(go, 0.5f);
                
            }
            isAttacking = true;
        }
        else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
            swordObject.GetComponent<BoxCollider>().enabled = false;
            anim.SetBool("Attack", false);
            isAttacking = false;
            attackEffect.SetActive(false);
        }
    }

    protected void Defense(){  
        if(Input.GetKey(KeyCode.X)){
            defenseEffect.transform.position = new Vector3(transform.position.x, transform.position.y+0.6f, transform.position.z);
            anim.SetBool("Defense", true);
            defenseEffect.SetActive(true);
            isDefense = true;
        }else{
            isDefense = false;
            anim.SetBool("Defense", false);
            defenseEffect.SetActive(false);
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

    void OnTriggerEnter(Collider other){
       if(other.tag == "monsterAttack")
       {
          if(!isDamage)
          {
                MonsterAttack attack = other.GetComponent<MonsterAttack>();
                health -= attack.Damage;

                StartCoroutine(OnDamage());
          }
       }
    }

    IEnumerator OnDamage()
    {
    
       isDamage = true;
      

       yield return new WaitForSeconds(1f);

       isDamage = false;


    }



    // 오브젝트와 충돌한경우
    private void OnCollisionStay(Collision collision){
        if(collision.collider.CompareTag("Ground")){       //바닥과 충돌 : 착지
            isJumping = false;
            yVelocity = 0f;
            moveDir.y =yVelocity;
        }
        if(collision.collider.CompareTag("Monster")&&anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")){   //적과 충돌했을 때의 상태가 공격중인 경우(적한테 맞은게 아니라 플레이어가 때린 것)
            Debug.Log(collision.gameObject.name);
            
        }
        else if(collision.collider.CompareTag("Monster") && isInvincible==false&&heart>0){        //적에게 맞았을 때 
            GameObject hitted = GenerateEffect(5, (collision.transform.position+transform.position)/2f);
            Destroy(hitted, 0.5f);
            Debug.Log("Player : get Attacked");

            if (isDefense & curEnergy >= 70f){  // 방어 성공 -> 데미지 무효, 에너지 감소
                curEnergy -= 70f;
            }
            else{
                isInvincible = true; 
                //int damage = 1;
                TakeDamage(1);
            }

        }
        
    }



    public void TakeDamage(int damage){
        anim.SetTrigger("GetHit");
        heart -= damage;
        //Debug.Log(damage);
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
            chargeEffect.transform.localPosition =  new Vector3(transform.position.x, transform.position.y-0.35f, transform.position.z);
            
            if (isCharging == false){
                isCharging = true;
                chargeEffect.SetActive(true);
            }
            if (curEnergy>=maxEnergy){
                curEnergy=0;
                Debug.Log("Player : 하트 획득 : "+addHeart(1));
                GameObject healEffect = GenerateEffect(3, transform.position);
                Destroy(healEffect, 0.2f);
                return;
            }
            curEnergy+=Time.deltaTime*(maxEnergy/chargeTime);    // 2초동안 100만큼 채우도록 한다.
            Debug.Log("에너지 충전... "+ curEnergy);
        }
        else if(chargeEffect!=null){
            isCharging = false;
            chargeEffect.SetActive(false);
        }
    }



    public void Invincible(){//피격 시 일정 시간 동안 무적상태 유지
        
        if(isInvincible==true){
            curTime += Time.deltaTime;
            //if((int)(curTime*10)%10==0){
            //    playerColor.material.color = red;
            //}
            //else{
            //    playerColor.material.color = blue;
            //}
            
            playerColor.material.color = Color.red;

        }
        //2초동안 true 상태를 유지하고 isInvincible을 false로 바꾸는 코드
        if(curTime >= invincibleTime){
            playerColor.material.color = Color.white;
            curTime=0f;
            isInvincible = false;
        }
    }

    // 플레이어 사망시 쓰러지는 모션, 특정 키 입력 시 부활하도록 함
    public void Die(){
        anim.SetTrigger("Die");
        Debug.Log("Player : Die...");
        isDead = true;
    }

    // 부활하는 모션과 함께 생명 수 초기화
    public void Restart(){
        if(Input.GetKey(KeyCode.R)&&isDead){ 
            GameObject restartEffect = GenerateEffect(2, savePoint);
            Destroy(restartEffect, 1f);
            anim.SetTrigger("DieRecover");
            heart = maxHeart;
            isDead = false;
            transform.position = savePoint;
        }
    }
    
    public GameObject GenerateEffect(int index, Vector3 position){
        GameObject go = Instantiate<GameObject>(effectPrefabs[index], position, Quaternion.identity);
        return go;
    }

    public void PowerAttack(){
        if (Input.GetKey(KeyCode.C) && isPower == false){
            isPower = true;
            anim.SetTrigger("PowerAttack");

        }
        if (isPower){
            attackTime += Time.deltaTime;
            if(attackTime>=0.8f && attackTime<=0.8f+Time.deltaTime){
                GameObject hit = GenerateEffect(9, transform.position);
                hit.transform.localScale = new Vector3(2f, 2f, 2f);
                Destroy(hit, 1.5f);
            }
            else if(attackTime>=1.3f && attackTime<=1.3f+Time.deltaTime){
                GameObject slash = GenerateEffect(8, transform.position);
                slash.transform.localScale = new Vector3(2f, 2f, 2f);
                Destroy(slash, 2.1f);
            }
            else if (attackTime>=2.5f && attackTime<=2.5f+Time.deltaTime){
                GameObject explosion = GenerateEffect(7, transform.position);
                explosion.transform.localScale = new Vector3(2f, 2f, 2f);
                Destroy(explosion, 1f);
                isPower = false;
                attackTime = 0f;
            }

        }
    }
}
