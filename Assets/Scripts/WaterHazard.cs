using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHazard : HazardObject {
    public float pushForce;

    public override void OnCollisionEnter2D(Collision2D obj){
        if(obj.collider.tag == "Player"){
            base.OnCollisionEnter2D(obj);
            obj.collider.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            obj.collider.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), pushForce * 2f), ForceMode2D.Impulse);
        }
    }
}

