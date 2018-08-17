using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : LevelObject {

    public float interactionRadius = 4f;
    public bool canInteract, done;

    public KeyCode interactionButton;
    Transform playerT;

    public GameObject OnInteractDrop;
	// Use this for initialization
	void Start () {
        interactionButton = FindObjectOfType<PlayerController>().interactButton;
        print(interactionButton);
        done = false;
        playerT = FindObjectOfType<PlayerEntity>().transform;
    }
	
	// Update is called once per frame
	void Update () {
        if(!done){

            if(Mathf.Abs(transform.position.x - playerT.position.x) < interactionRadius) {
                canInteract = true;
            }
            else {
                canInteract = false;
            }

            if(canInteract && Input.GetKeyDown(interactionButton)){
                done = true;
                Interact();
            }
        }
	}

    void Interact(){
        GetComponent<Animator>().SetTrigger("Opened");
        if(OnInteractDrop != null) {
            GameObject temp = Instantiate(OnInteractDrop, transform.position, Quaternion.identity);
            temp.GetComponent<SpriteRenderer>().sortingOrder = 10;
            temp.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            temp.GetComponent<BoxCollider2D>().isTrigger = false;
            temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-.4f, .4f), 1) * 2000f);
            temp.GetComponent<Rigidbody2D>().gravityScale = 7f;
        }
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, Vector3.one * interactionRadius);
    }
}
