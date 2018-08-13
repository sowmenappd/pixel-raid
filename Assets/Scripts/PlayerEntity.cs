using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : LivingEntity {

    Animator animator;
    PlayerController controller;

	public override void Start () {
        base.Start();
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();

    }

    public override void TakeDamage(int dmg){
        animator.SetTrigger("Attacked");
        base.TakeDamage(dmg);
        StartCoroutine(HaltMovement(0.45f));
    }

    public override void Attack(float radius, int attackDamage, string tag){
        if(isAlive){
            //StartCoroutine(HaltMovement(.2f));
            base.Attack(radius, attackDamage, tag);
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
        controller.enabled = false;
        yield return new WaitForSeconds(time);
        controller.enabled = true;
    }

}
