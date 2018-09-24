using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEvent : MonoBehaviour {

	public string[] triggeringColliderTags;

	public event System.Action OnEventStart;  
	public event System.Action OnEventEnd;
}
