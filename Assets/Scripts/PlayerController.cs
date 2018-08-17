using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public bool Android = false;

    new Rigidbody2D rigidbody;
    public PlayerEntity player;

    public float moveSpeed;
    public float jumpForce;

    public float dir = 1;
    [HideInInspector] public bool grounded;
    [HideInInspector] public bool climbing;
    [HideInInspector] public bool crouched;

    [HideInInspector] public bool attacking1 = false;
    [HideInInspector] public bool attacking2 = false;

    [HideInInspector] public bool moving, _moving;
    [HideInInspector] public bool halt; // this is controlled by the entity class
    [HideInInspector] public bool androidJumpFlag;

    float attackRadius;
    float currentMoveTimer;
    float nextMoveCheckTime;
    float moveDistanceCheckThreshold = 3f;
    Vector2 playerOldPosition;

    public KeyCode leftButton;
    public KeyCode jumpButton;
    public KeyCode rightButton;
    public KeyCode attackButton;
    public KeyCode crouchButton;

    Animator animator;
    float attackCounter = 0;

    void Start(){
        halt = false;
        player = GetComponent<PlayerEntity>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        currentMoveTimer = Time.time;
        nextMoveCheckTime = Time.time + 1f;
        playerOldPosition = transform.localPosition;
        androidJumpFlag = false;
        attackRadius = player.attackRadius;
    }

    void Update () {
        if(Android)
            MoveAndroid();
        else Move();
        CheckForMovement();
        Attack();
	}

    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "Ground"){
            grounded = true;
            climbing = false;
            rigidbody.isKinematic = false;
            jumpCounter = 0;
        }
        if(other.collider.tag == "Enemy"){
            grounded = true;
            climbing = false;
            jumpCounter = 0;
            GetComponent<IDamageable>().TakeDamage(other.collider.GetComponent<EnemyEntity>().damage);
        }
        if(other.collider.tag == "Interactive Enemy"){
            grounded = true;
            climbing = false;
            jumpCounter = 0;
            GetComponent<IDamageable>().TakeDamage(player.damage);
        }
    }
    void OnCollisionStay2D(Collision2D other) {
        if(other.collider.tag == "Ground") {
            grounded = true;
            climbing = false;
            rigidbody.isKinematic = false;
        }
        
    }

    void OnCollisionExit2D(Collision2D other){
        if(other.collider.tag == "Ground"){
            grounded = false;
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
        _moving = Input.GetKey(leftButton) ? true : ( Input.GetKey(rightButton) ? true : false);
        float h = Input.GetKey(leftButton) ? -1 : (Input.GetKey(rightButton) ? 1 : 0);
        float v = Input.GetKeyDown(jumpButton) ? 1 : 0;

        if (h < 0){
            this.dir = -1;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (h > 0){
            this.dir = 1;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (grounded && v > 0){
            grounded = false;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Force + 5);
            GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, 70);
        }
        else if(climbing && !grounded){
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            transform.Translate(Vector2.up * Time.deltaTime * 
                (Input.GetKey(jumpButton) ? 1 : Input.GetKey(crouchButton) ? -1 : 0));
        } else {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        if(Input.GetKey(crouchButton)){
            crouched = true;
        } else{
            crouched = false;
        }

        Vector2 dir = new Vector2(h, 0);
        if(((!attacking1  || (!attacking1  && !attacking2)) /*&& !crouched*/) && !halt)
            transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    #region AndroidControls

    [HideInInspector] public bool moveLeft, moveRight, jump, crouch;
    int jumpCounter = 0;

    public void MoveLeftAndroidIn(){
        moveLeft = true;
    }
    public void MoveRightAndroidIn(){
        moveRight = true;
    }
    public void JumpAndroidIn(){
        jumpCounter++;
        if(jumpCounter <= 2){
            jump = true;
        }
    }
    public void CrouchAndroidIn(){
        crouch = true;
    }
    public void MoveLeftAndroidOut(){
        moveLeft = false;
    }
    public void MoveRightAndroidOut(){
        moveRight = false;
    }
    public void JumpAndroidOut(){
        jump = false;
    }
    public void CrouchAndroidOut(){
        crouch = false;
    }

    void MoveAndroid(){
        _moving = moveLeft ? true : moveRight ? true : false;
        float h = moveLeft ? -1 : moveRight ? 1 : 0;
        float v = jump ? 1 : 0;


        if(h < 0) {
            GetComponent<SpriteRenderer>().flipX = true;
        } else if(h > 0){
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if(grounded && v > 0 && !androidJumpFlag) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Force + 5);
            GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, 70);
            androidJumpFlag = true;
            grounded = false;
        } else if(climbing && !grounded) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            transform.Translate(Vector2.up * Time.deltaTime * ( jump ? 1 : crouch ? -1 : 0 ));
            grounded = false;
        } else {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        if(v == 0) androidJumpFlag = false;

        if(crouch) {
            crouched = true;
        } else {
            crouched = false;
        }

        Vector2 dir = new Vector2(h, 0);
        if(( ( !attacking1 || ( !attacking1 && !attacking2 ) ) && !crouched ) && !halt)
            transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
    #endregion

    void CheckForMovement(){
        currentMoveTimer += Time.deltaTime;
        if(currentMoveTimer > nextMoveCheckTime){
            nextMoveCheckTime = Time.time + .5f;
            if(Mathf.Abs(playerOldPosition.x - transform.localPosition.x) > moveDistanceCheckThreshold){
                moving = true;
            } else{
                moving = false;
            }
            playerOldPosition = transform.localPosition;
        }
    }

    void Attack(){
        if(Input.GetKeyDown(attackButton) && !attacking1 && grounded && !halt){
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack(){
        StartCoroutine(player.HaltMovement(player.attackMovementInterruptDelay / 2f));
        float t = .4f;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(attackCounter == 0){
            animator.SetTrigger("Attack_1");
            attackCounter++;
            yield return new WaitForSeconds(t / 2);
            float c = t / 2;
            Attack(player.attackRadius, player.damage, "Enemy");
            while(c <= t){
                c += Time.deltaTime;
                if(Input.GetKeyDown(attackButton)){
                    animator.SetTrigger("Attack_2");
                    attackCounter++;
                    yield return new WaitForSeconds(t);
                    Attack(player.attackRadius, player.damage, "Enemy");
                }
                yield return null;
            }
            attackCounter = 0;
        }
    }

    public void Attack(float radius, int attackDamage, string tag){
        if(player.isAlive){
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.right * dir, player.attackRadius);
            if(hit != null){
                foreach(RaycastHit2D r in hit){
                    if(r.collider.GetComponent<IDamageable>() != null && r.collider.tag == tag){
                        r.collider.GetComponent<IDamageable>().TakeDamage(attackDamage);
                        r.collider.GetComponent<Rigidbody2D>().AddForce((Vector2.right + Vector2.up) * dir * player.attackForce);
                    }
                }
            }
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        attackRadius = player.attackRadius;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

}
