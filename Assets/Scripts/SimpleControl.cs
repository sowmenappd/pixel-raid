using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControl : MonoBehaviour {

    Transform target;
    public float speed = .2f;
    public Vector3 offset;
    public Vector2 boundsX;

    void Start(){
        target = FindObjectOfType<PlayerController>().transform;
    }
    // Update is called once per frame
    void Update(){
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundsX.x, boundsX.y), transform.position.y, transform.position.z);
    }
}
