using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardObject : LevelObject {

    public bool damageOverTime;
    public float damageRate = 1f;
    public int damage;

    PlayerEntity player;

    void Start(){
        player = FindObjectOfType<PlayerEntity>();
    }

    public virtual void OnCollisionEnter2D(Collision2D obj){
        if(!transform.GetComponent<Collider2D>().isTrigger)
            Attack(obj.collider.GetComponent<IDamageable>(), damage);
    }

    public virtual void OnTriggerEnter2D(Collider2D obj){
        Attack(obj.GetComponent<IDamageable>(), damage);
    }

    public virtual void Attack(IDamageable target, int damage){
        target.TakeDamage(damage);
    }

}
