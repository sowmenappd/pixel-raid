using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable {
    public int startingHealth;
    public bool isAlive;
    [Tooltip("Place this slightly in front of the character")]
    public float attackForce;
    public int damage;
    public float damageWaitTime = .5f;
    int health;

    public event System.Action OnAttacked;
    public event System.Action OnDeath;

    public Component[] componentsToTurnOffOnDeath;

    public bool destroyAfterDeath = false;
    public float destroyDelay = 2f;
    public bool fadeOnDeath = false;
    public float fadeDuration = 2f;

    void Awake(){
        OnDeath += TurnOffComponentsOnDeath;
        OnDeath += DestroyAfterDeath;
    }

    void DestroyAfterDeath(){
        if(fadeOnDeath){
            StartCoroutine(FadeToInvisible(fadeDuration));
        }

        if(!fadeOnDeath && destroyAfterDeath)
            Destroy(gameObject, destroyDelay);
    }

    public virtual void Start () {
        isAlive = true;
        health = startingHealth;
	}
	
    public virtual void TakeDamage(int _damage){
        if(OnAttacked != null)
            OnAttacked();
        StartCoroutine(WaitBeforeDamage(damageWaitTime, _damage));
    }

    public virtual void Die(){
        isAlive = false;
        print(gameObject.name + " dead!");
        if(OnDeath != null) OnDeath();
        GetComponent<Rigidbody2D>().simulated = false;
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

    IEnumerator FadeToInvisible(float fadeDuration){
        float speed = 1 / fadeDuration;
        float timer = 0;
        GetComponent<Animator>().enabled = false;
        SpriteRenderer _r = GetComponent<SpriteRenderer>();
        while(timer < fadeDuration){
            timer += Time.deltaTime;
            _r.color = Color.Lerp(_r.color, Color.clear, 2 * speed * Time.deltaTime);
            yield return null;
        }
        if(destroyAfterDeath)
            Destroy(gameObject);
    }

    IEnumerator WaitBeforeDamage(float duration, int _damage){
        yield return new WaitForSeconds(duration);
        health = Mathf.Clamp(health - _damage, 0, startingHealth);
        print(gameObject.name + " health remaining: " + health);
        if(health == 0)
            Die();
    }
}
