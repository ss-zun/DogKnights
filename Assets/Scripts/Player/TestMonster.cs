using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMonster : MonoBehaviour
{
    [SerializeField]
    private enum Type { Melee, Charge, Ranged };
    [SerializeField]
    private Type monsterType;
    [SerializeField]
    private int damage;

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float curHealth;
    [SerializeField]
    private BoxCollider monsterAttack; //공격범위
    [SerializeField]
    private Transform target; //추적타겟
    [SerializeField]
    private bool isChase; //추적중인가
    [SerializeField]
    private bool isAttack; //공격중인가


    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;

    private float curHitDis;
    private float targetRadius = 0f; //폭
    private float targetRange = 0f; //공격범위

    public int Damage
    {
        get { return damage; }
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2); //2초 뒤 실행
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        //네비게이션 활성화되어 있을때만 추적
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase; //멈추기
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
        switch (monsterType)
        {
            case Type.Melee:
                targetRadius = 0.6f;
                targetRange = 0.6f;
                break;
            case Type.Charge:
                targetRadius = 0.4f;
                targetRange = 12f;
                break;
            case Type.Ranged:
                targetRadius = 0.5f;
                targetRange = 25f;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, transform.position + transform.forward * curHitDis);
        Gizmos.DrawWireSphere(transform.position + transform.position * curHitDis, targetRadius);
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        
        switch (monsterType)
        {
            case Type.Melee:
                yield return new WaitForSeconds(0.2f);
                monsterAttack.enabled = true; //공격범위 활성화
                anim.SetBool("isAttack", true);

                yield return new WaitForSeconds(1f);
                monsterAttack.enabled = false;
                anim.SetBool("isAttack", false);

                yield return new WaitForSeconds(1f);
                break;
            case Type.Charge:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                monsterAttack.enabled = true;

                yield return new WaitForSeconds(1f);
                rigid.velocity = Vector3.zero;
                monsterAttack.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.Ranged:

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
        if(other.tag == "Sword")
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
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            mat.color = Color.white;
        }
        else //사망
        {
            mat.color = Color.gray;
            gameObject.layer = 9; //MonsterDead
            isChase = false; //사망했으니 추적중단
            nav.enabled = false; //NavAgent 비활성화(넛백 리액션을 살리기위해서)

            anim.SetTrigger("doDie");

            //넛백
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);

            Destroy(gameObject, 4); //4초 뒤 죽음
        }
    }
}
