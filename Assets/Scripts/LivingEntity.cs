using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable {
    public int startingHealth;
    protected bool isAlive;
    [Tooltip("Place this slightly in front of the character")]
    public float attackForce;

    int health;

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
    }
}
