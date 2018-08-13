using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardObject : MonoBehaviour {

    public bool damageOverTime;
    public float damageRate = 1f;
    public int damage;

    PlayerEntity player;

    void Start(){
        player = FindObjectOfType<PlayerEntity>();
    }

    public virtual void OnCollisionEnter2D(Collision2D obj){
        Attack(obj.collider.GetComponent<IDamageable>(), damage);
    }

    public virtual void Attack(IDamageable target, int damage){
        target.TakeDamage(damage);
        StartCoroutine(StartImmunityPeriod(3f));
    }

    IEnumerator StartImmunityPeriod(float duration){
        if(!player.immune){
            player.immune = true;
            float timer = 0;
            int i = 0;
            while(timer < (duration - .1f)){
                timer += Time.deltaTime;
                player.GetComponent<SpriteRenderer>().enabled = ( i % 4 == 0 || i % 5 == 0 ? false : true);
                i++;
                yield return null;
            }
            player.immune = false;
            player.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
