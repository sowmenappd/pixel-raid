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
        if(!transform.GetComponent<Collider>().isTrigger)
            Attack(obj.collider.GetComponent<IDamageable>(), damage);
    }

    public virtual void OnTriggerEnter2D(Collider2D obj){
        Attack(obj.GetComponent<IDamageable>(), damage);
    }

    public virtual void Attack(IDamageable target, int damage){
        target.TakeDamage(damage);
        StartCoroutine(StartImmunityPeriod(3f));
    }

    IEnumerator StartImmunityPeriod(float duration){
        if(!player.immune){
            SpriteRenderer renderer = player.GetComponent<SpriteRenderer>();
            player.immune = true;
            float timer = 0;
            int i = 0;
            while(timer < (duration - .1f)){
                timer += Time.deltaTime;
                renderer.enabled = ( i % 4 == 0 || i % 7 == 0 ? false : true);
                i++;
                yield return null;
            }
            player.immune = false;
            renderer.enabled = true;
        }
    }
}
