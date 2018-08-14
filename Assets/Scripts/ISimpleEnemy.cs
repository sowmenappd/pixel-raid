using System.Collections;
using UnityEngine;

public interface ISimpleEnemy {
    IEnumerator PatrolArea(Transform startPoint, Transform endPoint);
    bool ScanForPlayer();
    IEnumerator AttackPlayer(IDamageable target, int damage);
}
