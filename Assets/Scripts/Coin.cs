using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : LevelObject {

    public int Points = 1;
	
    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "Ground"){
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            OnPickup(other.GetComponent<PlayerEntity>());
            Destroy(gameObject);
        }
    }

    void OnPickup(PlayerEntity entity){
        if(entity != null){
            print("Coin picked up");
            //TODO: add coins to player's inventory
        }
    }

}
