using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    PlayerController player;
    Animator animator;

	void Start () {
        player = GetComponent<PlayerController>();
        animator = transform.GetComponent<Animator>();
	}

	void Update () {

        if(player._moving && player.grounded && !player.attacking1 
            && !player.attacking2 && !player.halt){
            animator.SetBool("Idle", false);
        } else{
            animator.SetBool("Idle", true);
        }

        animator.SetBool("Grounded", player.grounded);
        animator.SetBool("Climbing", player.climbing);
        animator.SetBool("Crouched", player.crouched);

        animator.SetBool("Attack_1", player.attacking1);
        if(player.attacking2){
            animator.SetTrigger("Attack_2");
            player.attacking2 = false;
        }
    }
}
