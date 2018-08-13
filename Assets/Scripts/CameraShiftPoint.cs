using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShiftPoint : MonoBehaviour {
    Camera cam;
    bool pointExitForward;

    public Vector2 shiftOffsetOnForwardPass;

	void Start () {
        cam = Camera.main;
	}
	
    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Player"){
            if(!pointExitForward){
                cam.GetComponent<SimpleControl>().offset -= (Vector3)shiftOffsetOnForwardPass;
            } else{
                cam.GetComponent<SimpleControl>().offset += (Vector3)shiftOffsetOnForwardPass;
            }

            pointExitForward = !pointExitForward;
        }
    }
}
