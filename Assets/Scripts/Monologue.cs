using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monologue : MonoBehaviour {

    public int storyOrder = 0;
    [System.Serializable]
    public class Line{
        [TextArea(2, 4)][Tooltip("All text strings are to be played in array sequence")]
        public string name = null;
        public string text = null;
    }
    public Line[] lines;
    public bool replayable = false;


    void Awake(){
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.isTrigger = true;
        box.size = Vector2.one * 3;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Player" && !MonologueManager.Instance.Locked){
            MonologueManager.Instance.GetNextMonologue();
            //if(!this.replayable) gameObject.SetActive(false);
        }
    }
    void OnTriggerStay2D(Collider2D col){
        if(col.tag == "Player" && !MonologueManager.Instance.Locked){
            MonologueManager.Instance.GetNextMonologue();
            //if(!this.replayable) gameObject.SetActive(false);
        }
    }

}
