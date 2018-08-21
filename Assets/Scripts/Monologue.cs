using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(BoxCollider2D))]
public class Monologue : MonoBehaviour {
    [TextArea(2, 4)][Tooltip("All text strings are to be played in array sequence")]
    public string[] text;
    public int storyOrder = 0;
    public bool replayable = false;


    void Awake(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.isTrigger = true;
        box.size = Vector2.one * 3;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Player"){
            MonologueManager.Instance.GetNextMonologue();
            if(!this.replayable) gameObject.SetActive(false);
        }
    }

}
