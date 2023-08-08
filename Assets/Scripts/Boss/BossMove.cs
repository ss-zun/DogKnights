using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : StateMachineBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float attackRange = 5f;

    Transform player;
    Rigidbody rb;
    Boss boss;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody>();
        boss = animator.GetComponent<Boss>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        Vector3 target = new Vector3(player.position.x, rb.position.y, rb.position.z);
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector3.Distance(player.position, rb.position) <= attackRange && animator.GetComponent<Boss>().isInvulnerable == false)
        {
            animator.SetBool("BasicAttack", true);
        }

        if (animator.GetComponent<Boss>().isInvulnerable == true)
        {
            animator.SetBool("BasicAttack", false);
        }
            
        if (animator.GetComponent<Boss>()._currentHP == 200) // old - cur �ؼ� �ذ��ϱ�////�ӽú��� bool �Ἥ �����߾����� üũ
        {
            animator.SetTrigger("FlameAttack");
        }

        //if (animator.GetComponent<Boss>().BossHP <= 100)
        //animator.SetBool("FlyFlameAttack", true);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("BasicAttack", false);

        //animator.SetBool("FlyFlameAttack", false);
    }
}