using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueTriggerEvent : StoryEvent {

	[Header("Set the target monologue id to trigger")]
	public int monologueOrder = 0;

	public float eventTriggerRadius = 3f;
	new BoxCollider2D collider2D;

	event System.Action<int> OnMonologueEventStart;

	// Use this for initialization
	void Start () {
		collider2D = gameObject.GetComponent<BoxCollider2D>();
		collider2D.isTrigger = true;
		collider2D.size = Vector2.one * eventTriggerRadius;

		OnMonologueEventStart += MonologueManager.Instance.GetMonologueByOrder; 
	}
	
	void OnDrawGizmos(){
		Gizmos.DrawWireSphere(transform.position, eventTriggerRadius);
	}

	void OnTriggerEnter2D(Collider2D col){
		foreach(string str in triggeringColliderTags){
			if(col.tag == str){
				print("triggered " + col.name);
				OnMonologueEventStart(monologueOrder);
				gameObject.SetActive(false);
				return;
			}
		}
	}
	
}
