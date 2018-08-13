using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : LevelObject {

    public Vector3[] waypoints;
    public float transitionDelay = 0f;
    public float speed = 2f;

    Queue<Vector3> path = new Queue<Vector3>();
	// Use this for initialization
	void Start () {
        foreach(Vector3 v in waypoints)
            path.Enqueue(v);
        StartCoroutine(FollowPath());
    }

    void OnCollisionStay2D(Collision2D obj){
        if(obj.collider.tag == "Player"){
            obj.collider.GetComponent<Transform>().parent = this.transform;
        }
    }

    void OnCollisionExit2D(Collision2D obj){
        if(obj.collider.tag == "Player"){
            obj.collider.GetComponent<Transform>().parent = null;
        }
    }

    IEnumerator FollowPath(){
        while (path.Count > 0){
            Vector2 nextWayPoint = path.Dequeue();
            Vector3 oldPosition = transform.localPosition;
            Vector3 newPosition = (Vector3)nextWayPoint;
            while(Vector3.Distance(transform.localPosition, (Vector3)newPosition) > .01f){
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, speed);  //.1 for scaling down speed
                yield return null;
            }
            path.Enqueue(nextWayPoint);
            yield return new WaitForSeconds(transitionDelay);
        }
        yield return null;
    }


}
