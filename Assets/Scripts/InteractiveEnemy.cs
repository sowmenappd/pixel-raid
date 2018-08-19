using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveEnemy : EnemyEntity, ISimpleEnemy {

    public Transform patrolStartPoint, patrolEndPoint;

    public float moveSpeed = 2f;
    public float minPatrolWaitTime, maxPatrolWaitTime;
    public float minimumPatrolDistance = .6f;
    public float maxAttackRadius = .75f;
    public float colliderTopOffset;

    Animator animator;
    IDamageable target;
    public BoxCollider2D attackCollider;
    public Vector2 attackColliderPos;

    bool _enabled = true, attacking = false;
    float dir = 1;
    Vector3 oldPosition;

    public IEnumerator PatrolArea(Transform startPoint, Transform endPoint) {
        if(currentState != State.Dead){
            SetState(State.Patrolling);
            animator.SetBool("Moving", true);
            Queue<Vector3> stopPositions = new Queue<Vector3>();
            float newX = Random.Range(startPoint.position.x, endPoint.position.x);
            while(Mathf.Abs(transform.position.x - newX) < minimumPatrolDistance){
                newX = Random.Range(startPoint.position.x, endPoint.position.x);
            }
            stopPositions.Enqueue(new Vector3(newX, transform.position.y, transform.position.z));

            while(stopPositions.Count > 0){
                Vector3 targetPosition = stopPositions.Dequeue();

                while(Mathf.Abs(transform.position.x - targetPosition.x) > 0.1f && currentState == State.Patrolling && isAlive){
                    if(_enabled && !attacking){
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    }
                    if(ScanForPlayer()){
                        SetState(State.Attacking);
                        animator.SetBool("Moving", false);
                    }
                    yield return null;
                }

                if(currentState == State.Patrolling){
                    newX = Random.Range(startPoint.position.x, endPoint.position.x);
                    while(Mathf.Abs(transform.position.x - newX) < minimumPatrolDistance){
                        newX = Random.Range(startPoint.position.x, endPoint.position.x);
                    }
                    Vector3 nextPatrolPosition = new Vector3(newX, transform.position.y, transform.position.z);
                    stopPositions.Enqueue(nextPatrolPosition);
                    float timeToWait = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
                    yield return new WaitForSeconds(timeToWait);
                }
                yield return null;
            }
            StartCoroutine(AttackPlayer(target, damage));
        }
    }

    public bool ScanForPlayer() {
        if(currentState == State.Idle || currentState == State.Patrolling || currentState == State.Attacking){
            Collider2D[] availableColliders = Physics2D.OverlapCircleAll(transform.position + Vector3.up * colliderTopOffset, maxAttackRadius);
            if(availableColliders != null){
                foreach(Collider2D c in availableColliders){
                    if(c.tag == "Player"){
                        if(target == null)
                            target = c.GetComponent<IDamageable>();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override void TakeDamage(int _damage){
        animator.SetTrigger("Attacked");
        base.TakeDamage(_damage);
    }

    void Update() {
        ConfigureSkeletonDirection();
        oldPosition = ( oldPosition != transform.position ) ? transform.position : oldPosition;
    }

    void ConfigureSkeletonDirection(){
        if(currentState != State.Attacking){
            if(transform.position.x < oldPosition.x){
                dir = -1;
                GetComponent<SpriteRenderer>().flipX = true;
                attackCollider.offset = -attackColliderPos;
            }
            else if(transform.position.x > oldPosition.x){
                dir = 1;
                GetComponent<SpriteRenderer>().flipX = false;
                attackCollider.offset = attackColliderPos;
            }
        }
        else{
            if(transform.position.x < target.RetrieveComponent<Transform>().position.x){
                dir = 1;
                GetComponent<SpriteRenderer>().flipX = false;
                attackCollider.offset = attackColliderPos;
            } else if(transform.position.x > target.RetrieveComponent<Transform>().position.x){
                dir = -1;
                GetComponent<SpriteRenderer>().flipX = true;
                attackCollider.offset = -attackColliderPos;
            }
        }
    }

    public IEnumerator AttackPlayer(IDamageable target, int damage){
        if(currentState != State.Dead){
            animator.SetTrigger("Attacking");
            attacking = true;
            ConfigureSkeletonDirection();
            yield return new WaitForSeconds(1.25f);
            attacking = false;
            StartCoroutine(PatrolArea(patrolStartPoint, patrolEndPoint));
        } 
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            other.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }


    public override void Start () {
        base.Start();
        animator = GetComponent<Animator>();
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Interactive Enemy"), LayerMask.NameToLayer("Interactive Enemy"), true);
        OnDeath += TriggerDeath;
        StartCoroutine(PatrolArea(patrolStartPoint, patrolEndPoint));
    }

    void SetState(State state){
        currentState = state;
    }

    void TriggerDeath(){
        currentState = State.Dead;
        animator.SetTrigger("Dead");
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * colliderTopOffset, maxAttackRadius);
    }
}
