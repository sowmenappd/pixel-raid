using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    new Rigidbody2D rigidbody;
    PlayerEntity player;

    public float moveSpeed;
    public float jumpForce;
    [HideInInspector] public bool grounded;
    [HideInInspector] public bool climbing;
    [HideInInspector] public bool crouched;

    [HideInInspector] public bool attacking1 = false;
    [HideInInspector] public bool attacking2 = false;

    [HideInInspector] public bool moving;

    public float attackRadius;
    float currentMoveTimer;
    float nextMoveCheckTime;
    float moveDistanceCheckThreshold = .25f;
    Vector2 playerOldPosition;

    public KeyCode attackButton;

    Animator animator;

    void Start(){
        player = GetComponent<PlayerEntity>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        currentMoveTimer = Time.time;
        nextMoveCheckTime = Time.time + 0.5f;
    }

	void Update () {
        Move();
        CheckForMovement();
        Attack();
	}

    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "Ground"){
            grounded = true;
            climbing = false;
            rigidbody.isKinematic = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Ladder"){
            rigidbody.isKinematic = true;
            grounded = false;
            climbing = true;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Ladder"){
            rigidbody.isKinematic = false;
            climbing = false;
        }
    }

    void Move(){
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetKeyDown(KeyCode.W) ? 1 : 0;

        if (h < 0){
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (h > 0){
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (grounded && v > 0){
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Force + 5);
            grounded = false;
        }
        else if(climbing && !grounded){
            transform.Translate(Vector2.up * Time.deltaTime * (Input.GetAxis("Vertical")));
            grounded = false;
        }

        if(Input.GetKey(KeyCode.S)){
            crouched = true;
        } else{
            crouched = false;
        }

        Vector2 dir = new Vector2(h, 0);
        if((!attacking1  || (!attacking1  && !attacking2)) && !crouched)
            transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    void CheckForMovement(){
        currentMoveTimer += Time.deltaTime;
        if(currentMoveTimer > nextMoveCheckTime){
            nextMoveCheckTime = Time.time + 0.15f;
            if(Mathf.Abs(playerOldPosition.x - transform.position.x) > moveDistanceCheckThreshold){
                moving = true;
            } else{
                moving = false;
            }
            playerOldPosition = transform.position;
        }
    }

    void Attack(){
        if (Input.GetKeyDown(attackButton) && !attacking1 && !attacking2){
            //StopAllCoroutines();
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack(){
        attacking1 = true;
        player.Attack(attackRadius, 15, "Enemy");
        float timeCheckForSecondAttack = 0.8f;
        float counter = 0;
        yield return null;
        attacking1 = false;
        while(counter <= timeCheckForSecondAttack){
            counter += Time.fixedDeltaTime;
            AnimatorStateInfo attackState = animator.GetCurrentAnimatorStateInfo(0);
            if(attackState.fullPathHash == Animator.StringToHash("Base Layer.Player_MeleeAttack1") &&
                Input.GetKeyDown(attackButton)){
                player.Attack(attackRadius, 15, "Enemy");
            }
            yield return null;
        }
        yield return new WaitForSeconds(.1f);
        attacking2 = false;
    }

}
