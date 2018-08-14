using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour {

	public float scrollSpeed;
	SpriteRenderer _r;
    PlayerController player;
    PlayerEntity pEntity;
	void Start(){
		_r = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        pEntity = player.GetComponent<PlayerEntity>();
	}
	
	void Update () {
        if(pEntity.isAlive && player.moving){
            float dir;
            if(!player.Android){
                dir = Input.GetKey(player.leftButton) ? -1 : ( Input.GetKey(player.rightButton) ? 1 : 0 );
            } else
                dir = player.moveLeft ? -1 : (player.moveRight ? 1 : 0);
            _r.size += new Vector2(Time.deltaTime * scrollSpeed * dir, 0f);
        }
	}
}
