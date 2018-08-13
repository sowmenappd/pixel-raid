using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : LivingEntity {
    Animator animator;
    PlayerController controller;
    PlayerAnimator pAnimator;

    public Transform attackPoint;

    public int attackDamage = 5;
    public float attackMovementInterruptDelay;
    public float radius = .75f;

	public override void Start () {
        base.Start();
        animator = GetComponent<Animator>();
        pAnimator = GetComponent<PlayerAnimator>();
        controller = GetComponent<PlayerController>();

    }

    public override void TakeDamage(int dmg){
        animator.SetTrigger("Attacked");
        base.TakeDamage(dmg);
        StartCoroutine(HaltMovement(0.45f));
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
            StartCoroutine(HaltMovement(attackMovementInterruptDelay));
        }
    }

    public override void Die(){
        if(!isAlive){
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

    void OnGUI(){
        if(controller.attacking1)
            Debug.DrawLine(transform.position, attackPoint.position, Color.red, 1.5f);

    }

}
