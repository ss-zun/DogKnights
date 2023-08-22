using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private int BossHP = 300;
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField]
    private int FlameAttackDamage = 2;
    [SerializeField]
    private int FlyFlameAttackDamge = 2;

    [SerializeField]
    private Vector3 attackOffset; // 캐릭터를 중심으로 떨어진 거리
    [SerializeField]
    private float attackRange = 5f; // 공격 사정거리
    [SerializeField]
    private LayerMask attackMask;

    private GameObject deathEffect;
    public bool isInvulnerable = false; // 무적

    public int _BossHP
    {
        get { return BossHP; }
    }

    public void Attack()
    {
        Debug.Log("Attack");
        Vector3 pos = transform.position;
        // 공격 범위
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        BossHP -= damage;
        Debug.Log("MaxHP : 300, CurrentHP : " + BossHP);
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
