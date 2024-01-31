using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField]
    protected enum Type { Melee, Charge, Ranged, MiddleBoss, Boss };
    [SerializeField]
    protected Type monsterType;
    [SerializeField]
    protected float maxHealth;
    [SerializeField]
    protected float curHealth;
    [SerializeField]
    protected BoxCollider attackArea; //공격범위
    [SerializeField]
    private GameObject rock;

    protected Transform target; //추적타겟

    protected bool isChase;  //추적중인가
    protected bool isAttack; //공격중인가
    protected bool isDead;   //죽었는가
    protected bool isInvincible;   //무적(방어했던지, 날았다던지)
    protected bool isZeroFloor; // 0층인지


    protected Rigidbody rigid;
    protected BoxCollider boxCollider;
    protected SkinnedMeshRenderer mat;
    protected NavMeshAgent nav;
    protected Animator anim;

    protected float curHitDis;
    protected float targetRadius = 0f; //폭
    protected float targetRange = 0f; //공격범위

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); // 플레이어의 위치 받아오기
        Debug.Log(target.name);

        if (monsterType != Type.Boss && monsterType != Type.MiddleBoss)
            Invoke("ChaseStart", 0);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if (monsterType != Type.Boss && monsterType != Type.MiddleBoss)
        {
            //네비게이션 활성화되어 있을때만 추적
            if (nav.enabled)
            {
                nav.SetDestination(target.position);
                nav.isStopped = !isChase; //멈추기
            }
        }       
    }

    //플레이어랑 물리적인 충돌일어날 때
    //리지드바디에 velocity 물리력이 추가되어있기 때문에
    //충돌하면 물리속도에 의해 움직임에 변화가 생김
    //velocity가 계속 유지되어 있기 때문에 추적못하는 상태가 되어 방지하고자함
    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targeting()
    {
        if (!isDead && monsterType != Type.Boss && monsterType != Type.MiddleBoss)
        {
            switch (monsterType)
            {
                case Type.Melee:
                    targetRadius = 0.6f;
                    targetRange = 0.4f;
                    break;
                case Type.Charge:
                    targetRadius = 0.6f;
                    targetRange = 2.0f;
                    break;
                case Type.Ranged:
                    targetRadius = 0.6f;
                    targetRange = 6.0f;
                    break;
            }

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position,
                                      targetRadius,
                                      transform.forward,
                                      targetRange, //공격 범위(거리)
                                      LayerMask.GetMask("Player"));
            curHitDis = 0;
            foreach (RaycastHit hit in rayHits)
            {
                curHitDis = hit.distance;
            }

            //하나라도 걸리면 && 공격중일땐 그거마저끝내고 공격해야함
            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        
        switch (monsterType)
        {
            case Type.Melee:
                yield return new WaitForSeconds(0.1f);
                attackArea.enabled = true; //공격범위 활성화
                anim.SetBool("isAttack", true);

                yield return new WaitForSeconds(GetAnimationLength("Attack01"));
                attackArea.enabled = false;
                anim.SetBool("isAttack", false);

                yield return new WaitForSeconds(1f);
                break;
            case Type.Charge:
                yield return new WaitForSeconds(0.2f);
                rigid.AddForce(transform.forward * 10, ForceMode.Impulse);
                attackArea.enabled = true;
                anim.SetBool("isAttack", true);

                yield return new WaitForSeconds(GetAnimationLength("Attack02"));
                rigid.velocity = Vector3.zero;
                attackArea.enabled = false;
                anim.SetBool("isAttack", false);

                yield return new WaitForSeconds(2f);
                break;
            case Type.Ranged:
                anim.SetBool("isAttack", true);
                yield return new WaitForSeconds(GetAnimationLength("Attack01") - 1.2f);
                Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.4f, transform.position.z);
                GameObject instantRock = Instantiate(rock, pos, transform.rotation);
                Rigidbody rigidRock = instantRock.GetComponent<Rigidbody>();
                rigidRock.velocity = transform.forward * 20;
                yield return new WaitForSeconds(1.2f);

                anim.SetBool("isAttack", false);
                Destroy(instantRock, 2);
                yield return new WaitForSeconds(2f);
                break;
        }
        isChase = true;
        isAttack = false;
    }

    // 고정시간마다 동작하기 때문에 물리처리를 할 때 사용
    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword" && isInvincible == false && gameObject.layer == 8)
        {
            Sword sword = other.GetComponent<Sword>();
            curHealth -= sword.damage;

            Vector3 reactVec = transform.position - other.transform.position; //넛백(반작용) : 현재 위치 - 피격 위치

            Debug.Log("Sword : " + curHealth);
            StartCoroutine(OnDamage(reactVec));
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.material.color = Color.red;

        //넛백
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rigid.AddForce(reactVec * 5, ForceMode.Impulse);

        anim.SetTrigger("getHit");

        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            mat.material.color = Color.white;
        }
        else //사망
        {
            mat.material.color = Color.gray;
            gameObject.layer = LayerMask.NameToLayer("MonsterDead");
            anim.SetTrigger("doDie");
            MonsterManager.Instance.MonsterKilled();
            StopAllCoroutines();

            isDead = true;
            isChase = false; //사망했으니 추적중단
            if (monsterType != Type.Boss && monsterType != Type.MiddleBoss)
            {
                nav.enabled = false; //NavAgent 비활성화
            }
            
            Destroy(gameObject, 2); //2초 뒤 파괴

            GameManager.Instance.Player.curEnergy += 50; //몬스터가 죽으면 플레이어 에너지 충전

            if(monsterType == Type.Boss)
            {
                GameManager.Instance.EndGame();
            }
        }
    }

    // 애니메이션 이름을 받아 해당 애니메이션의 길이를 반환하는 함수
    protected float GetAnimationLength(string animationName)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f; // 애니메이션을 찾지 못한 경우 0을 반환하거나 예외 처리
    }
}
