using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : LivingEntity {
    Animator animator;
    PlayerController controller;
    PlayerAnimator pAnimator;

    public Transform attackPoint;

    public bool immune;

    public int attackDamage = 2;
    public float attackMovementInterruptDelay;
    public float damagedMovementInterruptDelay;
    public float attackRadius = .75f;

	public override void Start () {
        base.Start();
        animator = GetComponent<Animator>();
        pAnimator = GetComponent<PlayerAnimator>();
        controller = GetComponent<PlayerController>();
        immune = false;
    }

    public override void TakeDamage(int dmg){
        if(!immune){
            animator.SetTrigger("Attacked");
            base.TakeDamage(dmg);
            StartCoroutine(HaltMovement(damagedMovementInterruptDelay));
        }
    }

    public void Attack(float radius, int attackDamage, string tag){
        if(isAlive){
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, radius);
            foreach(Collider2D c in colliders){
                if(c.GetComponent<IDamageable>() != null && c.tag == tag){
                    c.GetComponent<IDamageable>().TakeDamage(attackDamage);
                    Vector2 dir = ( (Vector2)c.transform.position - (Vector2)transform.position ).normalized;
                    c.GetComponent<Rigidbody2D>().AddForce(dir * attackForce);
                }
            }
            //StopCoroutine(HaltMovement(attackMovementInterruptDelay / 2f));
            StartCoroutine(HaltMovement(attackMovementInterruptDelay / 2f));
        }
    }

    public override void Die(){
        if(isAlive && !immune){
            base.Die();
            animator.SetTrigger("Dead");
            controller.enabled = false;
        }
    }

    IEnumerator HaltMovement(float time){
        controller.halt = true;
        yield return new WaitForSeconds(time);
        controller.halt = false;
    }

}
