using System.Collections;
using UnityEngine;

public interface ISimpleEnemy {
    IEnumerator PatrolArea(Transform startPoint, Transform endPoint);
    IEnumerator AttackPlayer(IDamageable target, int damage);
}
