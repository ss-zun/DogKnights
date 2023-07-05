using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private Transform player; // 플레이어의 위치
    private bool isFlipped = false; // 보스가 현재 뒤집혀 있는지

    [SerializeField]
    private int maxHP = 300;
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField]
    private int FlameAttackDamage = 2;
    [SerializeField]
    private int FlyFlameAttackDamge = 2;

    [SerializeField]
    private Vector3 attackOffset;
    [SerializeField]
    private float attackRange = 1f;
    [SerializeField]
    private LayerMask attackMask;

    private GameObject deathEffect;
    public bool isInvulnerable = false; // 무적

    public void LookAtPlayer() // 보스가 플레이어를 바라보게 하는 기능
    {
        Vector3 flipped = transform.localScale; 
        flipped.x *= -1f; // x축 반전

        // 보스가 플레이어보다 x축 방향으로 더 오른쪽에 있고,
        // 보스가 이미 뒤집혀 있다면(플레이어를 바라보고 있다면)
        // 보스를 다시 원래 방향으로 돌림
        if(transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        // 보스가 플레이어보다 x축 방향으로 더 왼쪽에 있고,
        // 보스가 아직 뒤집히지 않았다면(플레이어를 등지고 있다면)
        // 보스가 뒤집어서 플레이어를 바라보게 함
        else if(transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    // 만들다 말음
    public void Attack()
    {
        Debug.Log("Attack");
        Vector3 pos = transform.position;
        // 공격 범위
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        // 공격 범위 내 객체 탐지
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        // 공격 범위 내 객체가 감지됨
        if(colInfo != null)
        {         
            colInfo.GetComponent<Player>().TakeDamage(attackDamage);
            Debug.Log("attackDamage : 1");
        }
    }
    public void FlameAttack()
    {
        Debug.Log("FlameAttack");
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<Player>().TakeDamage(FlameAttackDamage);
            Debug.Log("FlameAttackDamage : 2");
        }
    }
    public void FlyFlameAttack()
    {
        Debug.Log("FlameAttack");
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<Player>().TakeDamage(FlyFlameAttackDamge);
            Debug.Log("FlyFlameAttackDamge : 2");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        maxHP -= damage;
        Debug.Log("MaxHP : 300, CurrentHP : " + maxHP);

        if (maxHP == 200)
            GetComponent<Animator>().SetBool("FlameAttack", true);

        if (maxHP == 100)
            GetComponent<Animator>().SetBool("FlyFlameAttack", true);

        if (maxHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
