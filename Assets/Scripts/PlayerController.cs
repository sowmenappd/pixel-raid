using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public bool grounded;

	void Update () {
        Move();

	}

    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "Ground"){
            grounded = true;
        }
    }

    void Move(){
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetKeyDown(KeyCode.W) ? 1 : 0;
        if (h < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (h > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (grounded && v > 0)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            grounded = false;
        }

        Vector2 dir = new Vector2(h, 0);
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
}
