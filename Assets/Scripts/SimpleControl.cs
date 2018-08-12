using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControl : MonoBehaviour {

    Transform target;
    public float speed = .2f;
    public Vector3 offset;

    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }
    // Update is called once per frame
    void Update(){
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
