using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    new Rigidbody2D rigidbody;

    public float moveSpeed;
    public float jumpForce;
     public bool grounded;
     public bool climbing;

    [HideInInspector] public bool attacking1 = false;
    [HideInInspector] public bool attacking2 = false;

    public KeyCode attackButton;

    Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

	void Update () {
        Move();
        Attack();
        Climb();
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
            print("trigger enter");
            rigidbody.isKinematic = true;
            grounded = false;
            climbing = true;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Ladder"){
            print("trigger exit");
            rigidbody.isKinematic = false;
            grounded = true;
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
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            grounded = false;
        }
        else if(climbing){
            transform.Translate(Vector2.up * Time.deltaTime * (Input.GetKey(KeyCode.W) ? 1:0));
        }

        Vector2 dir = new Vector2(h, 0);
        if(!attacking1 && !attacking2)
            transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    void Attack(){
        if (Input.GetKeyDown(attackButton) && !attacking1 && !attacking2){
            //StopCoroutine(StartAttack());
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        attacking1 = true;
        float timeCheckForSecondAttack = .6f;
        float counter = 0;
        yield return null;
        while(counter <= timeCheckForSecondAttack){
            counter += Time.fixedDeltaTime;
            AnimatorStateInfo attackState = animator.GetCurrentAnimatorStateInfo(0);
            if(attackState.fullPathHash == Animator.StringToHash("Base Layer.Player_MeleeAttack1") &&
                Input.GetKeyDown(attackButton)){
                animator.SetTrigger("Attack_2");
            }
            yield return null;
        }

        attacking1 = false;
        yield return new WaitForSeconds(2f);
        attacking2 = false;
    }

    void Climb(){

    }
}
