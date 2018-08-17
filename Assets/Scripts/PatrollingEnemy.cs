using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : EnemyEntity, ISimpleEnemy {

    public float moveSpeed = 2f, attackSpeed = 0.5f;
    public float minPatrolWaitTime, maxPatrolWaitTime;
    public float minimumPatrolDistance = .6f;
    public float maxScanRadius = 10f, maxAttackRadius = .75f;
    public float colliderTopOffset;

    public Transform patrolStartPoint, patrolEndPoint;
    Animator animator;
    IDamageable player;

    bool boundaryFlag = false;
    bool _enabled = true;

    public override void Start(){
        base.Start();
        animator = GetComponent<Animator>();
        var _player = FindObjectOfType<PlayerEntity>().GetComponent<IDamageable>();
        if(_player != null) {
            player = _player;
            StartCoroutine(PatrolArea(patrolStartPoint, patrolEndPoint));
        } else {
            SetState(State.Idle);
        }

        OnDeath += SetToDeathState;
    }

    //NOTE: startPoint should be at left, endPoint at right
    public IEnumerator PatrolArea(Transform startPoint, Transform endPoint){
        if(isAlive){
            SetState(State.Patrolling);
            Queue<Vector3> stopPositions = new Queue<Vector3>();
            float newX = Random.Range(startPoint.position.x, endPoint.position.x);
            while(Mathf.Abs(transform.position.x - newX) < minimumPatrolDistance){
                newX = Random.Range(startPoint.position.x, endPoint.position.x);
            }
            stopPositions.Enqueue(new Vector3(newX, transform.position.y, transform.position.z));

		    while(stopPositions.Count > 0){
                Vector3 targetPosition = stopPositions.Dequeue();

                while(Mathf.Abs(transform.position.x - targetPosition.x) > 0.1f && currentState == State.Patrolling && isAlive){
                    if(_enabled){
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    }
                    ConfigureAnimation(targetPosition);
                    if(ScanForPlayer()){
                        SetState(State.Attacking);
                        break;
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
                    ConfigureAnimation(targetPosition);
                    float timeToWait = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                    yield return new WaitForSeconds(timeToWait);
                }
                yield return null;
		    }
            StartCoroutine(AttackPlayer(player, damage));
        }
    }

    public bool ScanForPlayer(){
        if(isAlive){
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
        return false;
    }

    IEnumerator DisableMovement(float time){
        _enabled = false;
        yield return new WaitForSeconds(time);
        _enabled = true;
    }

    public IEnumerator AttackPlayer(IDamageable target, int damage){
        if(!boundaryFlag){
            Transform playerT = target.RetrieveComponent<Transform>();
            PlayerEntity playerE = target.RetrieveComponent<PlayerEntity>();
            if(playerT != null)
                SetState(State.Attacking);
            while(Mathf.Abs(transform.position.x - playerT.position.x) < maxScanRadius && currentState == State.Attacking && isAlive){
                ConfigureAnimation(playerT.position);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerT.position.x, transform.position.y, transform.position.z), 2 * moveSpeed * Time.deltaTime);
            
                CheckForBounds();
                if(boundaryFlag) break;
                if(Mathf.Abs(transform.position.x - playerT.position.x) < maxAttackRadius
                    && Mathf.Abs(transform.position.y - playerT.position.y) < maxAttackRadius) {
                    if(!playerE.immune){
                        playerE.TakeDamage(damage);
                        Vector2 forceDir = (transform.position - playerT.position).normalized;
                        playerT.GetComponent<Rigidbody2D>().AddForce(forceDir * attackForce);
                    }
                } 
                yield return null;
            }
            StartCoroutine(PatrolArea(patrolStartPoint, patrolEndPoint));
        }
    }

    void CheckForBounds(){
        if(Mathf.Abs(transform.position.x - patrolStartPoint.position.x) < .1f ||
            Mathf.Abs(transform.position.x - patrolEndPoint.position.x) < .1f){
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, patrolStartPoint.position.x, patrolEndPoint.position.x), transform.position.y, transform.position.z);
            boundaryFlag = true;
        }
        else
            boundaryFlag = false;
    }

   

    //for the scorpion
    void ConfigureAnimation(Vector3 position){
        if(isAlive){
            if(transform.position.x < position.x){
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
            }
            else{
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
            }
        }
    }

    void SetState(State state){
		currentState = state;
    }

    void OnDrawGizmos(){
        if(isAlive){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * colliderTopOffset, maxAttackRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * colliderTopOffset, maxScanRadius);
        }
    }

    public void SetToDeathState() {
        animator.SetTrigger("Dead");
    }
}
