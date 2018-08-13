using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable {
    public int startingHealth;
    protected bool isAlive;
    [Tooltip("Place this slightly in front of the character")]
    public Transform attackPoint;
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

    public virtual void Attack(float radius, int attackDamage, string tag){
        print("Attacking");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, radius);

        foreach(Collider2D c in colliders){
            if(c.GetComponent<IDamageable>() != null && c.tag == tag){
                c.GetComponent<IDamageable>().TakeDamage(attackDamage);
                Vector2 dir = ( (Vector2)c.transform.position - (Vector2)transform.position ).normalized;
                c.GetComponent<Rigidbody2D>().AddForce(dir * attackForce);
            }
        }
    }

    public virtual void Die(){
        isAlive = false;
    }
}
