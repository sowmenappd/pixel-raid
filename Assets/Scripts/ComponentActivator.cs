using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentActivator : MonoBehaviour {
    public Component[] componentsToDisable;
    public float activationDelay;

	// Use this for initialization
	void Start () {
        SetComponentState(componentsToDisable, false);
        StartCoroutine(ActivateAfter(activationDelay));
	}
	
	// Update is called once per frame
	IEnumerator ActivateAfter(float delay) {
        yield return new WaitForSeconds(delay);
        SetComponentState(componentsToDisable, true);
	}

    void SetComponentState(Component[] components, bool state){
        foreach(Component component in components){
            if(component is Renderer){
                ( component as Renderer ).enabled = state;
            } else if(component is Collider){
                ( component as Collider ).enabled = state;
            } else if(component is Behaviour){
                ( component as Behaviour ).enabled = state;
            }
        }
    }
}
