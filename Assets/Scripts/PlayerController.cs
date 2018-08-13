﻿using System;
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
    [HideInInspector] public bool halt; // this is controlled by the entity class

    public float attackRadius;
    float currentMoveTimer;
    float nextMoveCheckTime;
    float moveDistanceCheckThreshold = .25f;
    Vector2 playerOldPosition;

    public KeyCode leftButton;
    public KeyCode jumpButton;
    public KeyCode rightButton;
    public KeyCode attackButton;
    public KeyCode crouchButton;

    Animator animator;

    void Start(){
        halt = false;
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
        float h = Input.GetKey(leftButton) ? -1 : (Input.GetKey(rightButton) ? 1 : 0);
        float v = Input.GetKeyDown(jumpButton) ? 1 : 0;

        if (h < 0){
            GetComponent<SpriteRenderer>().flipX = true;
            player.attackPoint.localPosition = new Vector2(-.153f, -.074f);
        }
        else if (h > 0){
            GetComponent<SpriteRenderer>().flipX = false;
            player.attackPoint.localPosition = new Vector2(.153f, -.074f);
        }

        if (grounded && v > 0){
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Force + 5);
            grounded = false;
        }
        else if(climbing && !grounded){
            transform.Translate(Vector2.up * Time.deltaTime * (Input.GetKey(jumpButton) ? 1 : 0));
            grounded = false;
        }

        if(Input.GetKey(crouchButton)){
            crouched = true;
        } else{
            crouched = false;
        }

        Vector2 dir = new Vector2(h, 0);
        if(((!attacking1  || (!attacking1  && !attacking2)) && !crouched) && !halt)
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
        if(Input.GetKeyDown(attackButton) && !attacking1){
            StartCoroutine(StartAttack());
            player.Attack(.75f, 1, "Enemy");
        }
    }

    IEnumerator StartAttack(){
        attacking1 = true;
        float c = 0, t = .6f;
        yield return null;
        attacking1 = false;
        while(c <= t){
            c += Time.deltaTime;
            if(Input.GetKeyDown(attackButton))
                attacking2 = true;
            yield return null;
        }
    }

    

}
