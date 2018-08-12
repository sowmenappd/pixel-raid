using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour {

	public float scrollSpeed;
	SpriteRenderer _r;
    PlayerController player;
	void Start(){
		_r = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
	}
	
	void Update () {
        if(player.moving){
            float dir = Input.GetAxisRaw("Horizontal");
		    _r.size += new Vector2(Time.deltaTime * scrollSpeed * dir, 0f);
        }
	}
}
