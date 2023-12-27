using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    [SerializeField]
    private GameObject Fire;
    [SerializeField]
    private Transform FirePort;
    [SerializeField]
    private Transform FlyFirePort;

    int currentPhase = 1; // 보스 초기 페이즈

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponentInChildren<Animator>();
        isInvincible = true;
    }


    void Update()
    {
        if (isDead)
        {
            // 적의 상태가 사망일 때 모든 코루틴 중지
            StopAllCoroutines();
        }
    }

    public void StartBossPattern()
    {
        isInvincible = false;
        StartCoroutine(BossPattern());
    }

    IEnumerator BossPattern()
    {
        yield return new WaitForSeconds(0.1f);

        int randAction = Random.Range(0, 7);
        switch(randAction)
        {
            case 0:
                // 방어
                StartCoroutine(Defend());
                break;
            case 1:
            case 2:
                // 1타 근접 공격
                StartCoroutine(BasicAttack());
                break;
            case 3:
            case 4:
                // 2타 근접 공격
                StartCoroutine(ClawAttack());
                break;
            case 5:
                // 지면에서 불뿜기
                StartCoroutine(FlameAttack());
                break;
            case 6:
                // 날아서 불뿜기
                StartCoroutine(FlyFlameAttack());
                break;
        }
    }

    void EnabledArea()
    {
        targetRadius = 5.0f;
        targetRange = 5.0f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
                                  targetRadius,
                                  transform.forward,
                                  targetRange, //공격 범위(거리)
                                  LayerMask.GetMask("Player"));

        //하나라도 걸리면 && 공격중일땐 그거마저끝내고 공격해야함
        if (rayHits.Length > 0 && !isAttack)
        {
            attackArea.enabled = true; //공격범위 활성화
        }
    }

    IEnumerator Defend()
    {
        isInvincible = true;
        anim.SetTrigger("doDefend");
        yield return new WaitForSeconds(GetAnimationLength("Defend"));
        isInvincible = false;
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(BossPattern());
    }
    IEnumerator BasicAttack()
    {
        anim.SetTrigger("doBasicAttack");
        EnabledArea();
        yield return new WaitForSeconds(GetAnimationLength("Basic Attack"));
        attackArea.enabled = false;
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(BossPattern());
    }
    IEnumerator ClawAttack()
    {
        anim.SetTrigger("doClawAttack");
        EnabledArea();
        yield return new WaitForSeconds(GetAnimationLength("Claw Attack"));
        attackArea.enabled = false;
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(BossPattern());
    }
    IEnumerator FlameAttack()
    {
        anim.SetTrigger("doFlameAttack");
        yield return new WaitForSeconds(0.5f);
        GameObject instantFire = Instantiate(Fire, FirePort.position, FirePort.rotation);
        yield return new WaitForSeconds(GetAnimationLength("Flame Attack"));
        Destroy(instantFire);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(BossPattern());
    }
    IEnumerator FlyFlameAttack()
    {
        isInvincible = true;
        anim.SetTrigger("doFlyFlameAttack");
        yield return new WaitForSeconds(GetAnimationLength("Take Off"));
        yield return new WaitForSeconds(GetAnimationLength("Fly Float"));
        GameObject instantFire = Instantiate(Fire, FlyFirePort.position, FlyFirePort.rotation);
        yield return new WaitForSeconds(GetAnimationLength("Fly Flame Attack"));
        Destroy(instantFire);
        yield return new WaitForSeconds(GetAnimationLength("Land"));
        isInvincible = false;
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(BossPattern());
    }

    // 애니메이션 이름을 받아 해당 애니메이션의 길이를 반환하는 함수
    float GetAnimationLength(string animationName)
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
