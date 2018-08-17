using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : LivingEntity {
    Animator animator;
    PlayerController controller;
    SpriteRenderer _renderer;

    public bool immune;

    public float attackMovementInterruptDelay;
    public float damagedMovementInterruptDelay;
    public float attackRadius;

	public override void Start () {
        base.Start();
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        _renderer = GetComponent<SpriteRenderer>();
        immune = false;
    }

    public override void TakeDamage(int dmg){
        if(!immune){
            animator.SetTrigger("Attacked");
            base.TakeDamage(dmg);
            StartCoroutine(HaltMovement(damagedMovementInterruptDelay));
            StartCoroutine(StartImmunityPeriod(3f));
        }
    }

    public override void Die(){
        if(isAlive){
            base.Die();
            animator.SetTrigger("Dead");
            controller.enabled = false;
        }
    }

    public IEnumerator HaltMovement(float time){
        controller.halt = true;
        yield return new WaitForSeconds(time);
        controller.halt = false;
    }

    public IEnumerator StartImmunityPeriod(float duration){
        if(!immune){
            immune = true;
            float timer = 0;
            int i = 0;
            while(timer < ( duration - .1f ))
            {
                timer += Time.deltaTime;
                _renderer.enabled = ( i % 4 == 0 || i % 7 == 0 ? false : true );
                i++;
                yield return null;
            }
            immune = false;
            _renderer.enabled = true;
        }
    }

}
