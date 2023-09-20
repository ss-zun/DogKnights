using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{   [SerializeField]
    public Animator anim;
    public Rigidbody playerRigidbody;
    [SerializeField]
    public float jumpForce = 30;
    [SerializeField]
    public float attackPower = 10f;
    [SerializeField]
    public GameObject swordObject;
    public float gravity;  //중력 가속도
    float yVelocity;  //y 이동값
    Vector3 moveDir;
    [SerializeField]
    private float runSpeed = 0.1f;
    [SerializeField]
    private float dashSpeed = 0.5f;
    [SerializeField]
    private GameObject root;
    //private float defense = 100f;

    SkinnedMeshRenderer[] meshs;
    bool isDamage = false;


    Color red = new Color(1f, 0f, 0f, 0.5f);
    Color blue = new Color(0f, 0f, 1f, 0.5f);

    // 에너지 충전에 관한 변수
    public float curEnergy = 0f;     //에너지 량
    public float maxEnergy = 100f;
    private float chargeTime = 0f;    //충전 시간
    //private float maxTime = 2f;
    float attackTime = 0f;
    public float maxJumpTime = 0.4f;
    float curJumpTIme = 0f;

    public int heart = 5;     //현재 하트 수
    public int maxHeart = 5; //최대 하트 수

    [SerializeField]
    Slider hpSlider;

    private bool isInvincible = false;  //현재 무적상태인지
    private bool isDead = false;  //현재 사망상태인지
    public bool isAttacking = false; //공격상태인지
    private bool isCharging = false; //에너지를 모으는 중 인지 
    private bool isDefense = false; //방어상태인지
    private bool isPower = false; //필살기 사용 중인지
    private bool isDash = false; //대시 중인지
    private bool isDashable = true; //대시 할 수 있는지
    public bool isJumping = true;       //공중에 떠 있는지
    // 플레이어 상태에 관한 boolean 변수들은 주로 이펙트를 적용하기 위해 정의

    private float invincibleTime = 2.0f; //무적 상태 2초동안 유지
    float curTime = 0f; //무적상태를 유지한 시간
    [SerializeField]
    private float dashCoolTime = 2.0f; // 대시 스킬을 쓸 수 있는 쿨타임
    private float curDashTime = 0f;

    Renderer playerColor;//플레이어 material 색상이 붉게 깜빡거리도록 함

    [SerializeField]
    private GameObject[] effectPrefabs; // 캐릭터의 이펙트 : 피격 시 이펙트, 공격 시 이펙트, 에너지 모을 때의 이펙트 등
    // 0:slash, 1:hit, 2:restart, 3:heart get, 4:charging, 5:get hitted, 6:defense, 7:explosion, 8:slash, 9:hit

    GameObject chargeEffect = null;
    GameObject defenseEffect = null;
    GameObject attackEffect = null;
    GameObject[] effectsGO;
    public Vector3 savePoint = Vector3.zero;   //사망한 후 재시작 할 때 스폰할 지점
    [SerializeField]
    private float groundPos = 0.0f;

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
        if (hpSlider)
        {
            hpSlider.value = heart;
            hpSlider.maxValue = maxHeart;
        }

        //isJumping = false;
        //Move();
        Attack();
        Charge();
        Restart();
        Invincible();
        Defense();
        PowerAttack();
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("PowerAttack")){
            isInvincible = true;

        }
    }

   void FixedUpdate(){   // 점프 후 착지에서 땅에 닿을 때 raycast로 땅을 뚫지 않도록 조정
                         //if(yVelocity < 0){
                         //    Debug.DrawRay(playerRigidbody.position, Vector3.down, new Color(0, 1, 0));
                         //    RaycastHit2D rayHit = Physics2D.Raycast(playerRigidbody.position, Vector3.down, 1, LayerMask.GetMask("Floor"));
                         //    Debug.Log(rayHit);
                         //    if(rayHit.collider != null){
                         //        if (rayHit.distance < 0.5f){
                         //            isJumping = false;
                         //        }

        //    if(rayHit.collider.transform.position.y - transform.position.y < yVelocity){
        //        yVelocity = rayHit.collider.transform.position.y - transform.position.y;  
        //        // 떨어지는 이동 변위가 지면까지의 거리보다 크다면 지면을 뚫고 들어갈 수 있으므로 이보다 작게 설정해준다.
        //    }
        //    }
        //    else{
        //        Debug.Log("rayHit is null");
        //    }

        //}
        Move();
        Jump();
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

        //yVelocity = 0f;
        curDashTime += Time.deltaTime;
        float moveSpeed = runSpeed;
        float xInput = Input.GetAxis("Horizontal");
        if(isDead || isCharging || isDefense || isAttacking){  // 사망했거나 방어 자세이거나 에너지 충전중이라면 움직이지 않도록 함
            return;
        }

        if (curDashTime >= dashCoolTime)  // 쿨타임 다 차면 
        {
            isDashable = true;
            curDashTime = 0f;
        }
        else if(curDashTime >= 1.0f)
        {
            isDash = false;
        }


        if (Input.GetKeyDown(KeyCode.V) && isDash == false && isDashable)  // 대시 쓸수있으면
        {
            isDash = true;
            isDashable = false;
            curDashTime = 0; // 스킬 사용시 쿨타임 초기화
            anim.SetTrigger("Dash");
        }
        if (isDash)
        {
            yVelocity = 0;
            moveSpeed = dashSpeed;

        }

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
        if(Input.GetKeyDown(KeyCode.LeftControl) && isJumping == false && isDash == false){  // 최대 1초동안만 점프할 수 있도록 함
            // playerRigidbody.AddForce(Vector3.up*jumpForce, ForceMode.Impulse );
            // isJumping = true;
            //if(curJumpTIme <= maxJumpTime){

            //    yVelocity = jumpForce*Time.deltaTime;
            //    isJumping = true;
            //    Debug.Log(curJumpTIme);
            //}else{
            //    yVelocity = -1f*jumpForce*Time.deltaTime;  
            //    isJumping = true;
            //}
            yVelocity = jumpForce * Time.deltaTime;
            isJumping = true; 
        }

        if(isDash == false && isJumping )
        {
            //curJumpTIme += Time.deltaTime;
            yVelocity += gravity * Time.deltaTime;

        }
        else
        {
            yVelocity = 0f;
            //curJumpTIme = 0f;
        }


        moveDir.Normalize();
        
        moveDir = transform.right*xInput*-1f;
        moveDir.y = yVelocity;

        transform.Translate(moveDir*moveSpeed*Time.deltaTime);
        
        // float zInput = Input.GetAxis("Vertical");
        if(anim.GetBool("Attack")){
            return;
        }

    }

    protected void Attack(){
        //int effectCount = 0;
        if(Input.GetKeyDown(KeyCode.Z)){
            if (isAttacking == false){
                anim.SetTrigger("AttackTrigger");  // 공격모션

                
            }
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01")){
            Vector3 effectPosition = new Vector3(transform.position.x+0.8f, transform.position.y+0.8f, 0f);
            swordObject.GetComponent<BoxCollider>().enabled = true;    // 무기 콜라이더 활성화 
            attackEffect.SetActive(true); //이펙트 활성화
            isAttacking = true; //공격 플래그 true

        }//공격 모션이 진행되는 동안 위 상태가 지속되다가 공격모션이 끝나면 아래 상태 변경
        else{ 
            swordObject.GetComponent<BoxCollider>().enabled = false;
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

    protected void Jump()
    {
        // yVelocity += gravity*Time.deltaTime;
        //if (Input.GetKey(KeyCode.LeftControl) && isJumping == false)
        //{
        //    playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //    isJumping = true;
        //    // transform.forward*


        //}
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "MonsterAttack" && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01")) //적과 충돌했을 때의 상태가 공격중인 경우(적한테 맞은게 아니라 플레이어가 때린 것)
        {   
            Debug.Log(other.gameObject.name);
        }
        else if (other.tag == "MonsterAttack" && isInvincible == false && heart > 0) //적에게 맞았을 때 
        {
            if (!isDamage)
            {
                MonsterAttack attack = other.GetComponent<MonsterAttack>();
                //연경부분-start
                if (isDefense & curEnergy >= 70f)
                {  // 방어 성공 -> 데미지 무효, 에너지 감소
                    curEnergy -= 70f;
                }
                else
                {
                    isInvincible = true;
                    //int damage = 1;
                    TakeDamage(attack.Damage);
                }
                //연경부분-end

                //Rock만 if문 안에 들어감
                if (other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject, 4); //플레이어와 닿으면 Rock은 Destroy

                //연경부분-start
                GameObject hitted = GenerateEffect(5, (other.transform.position + transform.position) / 2f); // 피격 이펙트 생성
                Destroy(hitted, 0.5f);
                Debug.Log("Player : get Attacked");
                //연경부분-end

                Debug.Log("플레이어 현재 하트: " + heart);
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
    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Floor")){       //바닥과 충돌 : 착지
            isJumping = false;
            yVelocity = 0f;
            moveDir.y =yVelocity;
            Debug.Log("Player : Floor collision");
        }
        //else
        //{
        //    isJumping = true;
        //    yVelocity += gravity * Time.deltaTime;
        //    moveDir.y = yVelocity;
        //}
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Floor"))
        {       //바닥과 충돌 : 착지
            isJumping = true;
            Debug.Log("Player : floor exit");
        }
        //else
        //{
        //    isJumping = true;
        //    yVelocity += gravity * Time.deltaTime;
        //    moveDir.y = yVelocity;
        //}
    }



    private void TakeDamage(int damage){
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

    public void Dash() //단시간에 앞으로 튀어나가는 기술
    {

    }
}
