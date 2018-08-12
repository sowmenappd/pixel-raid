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
		if(Input.GetAxisRaw("Horizontal") != 0 && player.grounded){
            animator.SetBool("Idle", false);
        }
        else{
            animator.SetBool("Idle", true);
        }

        animator.SetBool("Grounded", player.grounded);
	}
}
