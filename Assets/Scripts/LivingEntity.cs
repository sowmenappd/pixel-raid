using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable {
    public int startingHealth;
    [HideInInspector] public bool isAlive;
    [Tooltip("Place this slightly in front of the character")]
    public float attackForce;

    int health;

    public event System.Action OnDeath;

    public Component[] componentsToTurnOffOnDeath;

    void Awake(){
        OnDeath += TurnOffComponentsOnDeath;
    }

	public virtual void Start () {
        isAlive = true;
        health = startingHealth;
	}
	
    public virtual void TakeDamage(int damage){
        health = Mathf.Clamp(health - damage, 0, startingHealth);
        print(gameObject.name + " health remaining: " + health);
        if(health == 0)
            Die();
        return;
    }

    public virtual void Die(){
        isAlive = false;
        if(OnDeath != null) OnDeath();
        GetComponent<Rigidbody2D>().simulated = false;
        //GetComponent<BoxCollider2D>().enabled = false;
    }

    public T RetrieveComponent<T>(){
        return GetComponent<T>();
    }

    void TurnOffComponentsOnDeath(){
        foreach(Component component in componentsToTurnOffOnDeath){
            if(component is Renderer){
                ( component as Renderer ).enabled = false;
            } else if(component is Collider){
                ( component as Collider ).enabled = false;
            } else if(component is Behaviour){
                ( component as Behaviour ).enabled = false;
            }
        }
    }
}
