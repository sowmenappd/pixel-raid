using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveEnemy : EnemyEntity, ISimpleEnemy {

    public float moveSpeed = 2f;
    public float minPatrolWaitTime, maxPatrolWaitTime;
    public float minimumPatrolDistance = .6f;
    public float maxScanRadius = 10f, maxAttackRadius = .75f;
    public float colliderTopOffset;

    Animator animator;

    public IEnumerator AttackPlayer(IDamageable target, int damage) {
        yield return null;
    }

    public IEnumerator PatrolArea(Transform startPoint, Transform endPoint) {
        yield return null;
    }

    public bool ScanForPlayer() {
        Collider2D[] availableColliders = Physics2D.OverlapCircleAll(transform.position + Vector3.up * colliderTopOffset, maxScanRadius);
        if(availableColliders != null){
            foreach(Collider2D c in availableColliders){
                if(c.tag == "Player"){
                    return true;
                }
            }
        }
        return false;
    }
	
	public override void Start () {
        base.Start();
        animator = GetComponent<Animator>();
        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer, true);
        OnDeath += TriggerDeath;
    }

    void SetState(State state){
        currentState = state;
    }

    void TriggerDeath(){
        animator.SetTrigger("Dead");
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * colliderTopOffset, maxAttackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * colliderTopOffset, maxScanRadius);
    }
}
