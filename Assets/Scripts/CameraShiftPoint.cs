using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShiftPoint : MonoBehaviour {
    Camera cam;

    public Vector2 shiftOffsetOnForwardPass;
    public float resetDelay;

	void Start () {
        cam = Camera.main;
	}
	
    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Player"){
            cam.GetComponent<SimpleControl>().offset += (Vector3)shiftOffsetOnForwardPass;
            StartCoroutine(Reset(resetDelay));
        }
    }

    IEnumerator Reset(float resetDelay){
        yield return new WaitForSeconds(resetDelay);
        cam.GetComponent<SimpleControl>().offset -= (Vector3)shiftOffsetOnForwardPass;
    }

    IEnumerator StartShift(Vector2 targetPoint){
        yield return null;
        //TODO: implement
    }
}
