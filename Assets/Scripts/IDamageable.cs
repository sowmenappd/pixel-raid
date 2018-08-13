using UnityEngine;

public interface IDamageable {
    void TakeDamage(int damage);
    //void TakeDamageOverTime(int dmg, int repeatRate);
    void Die();
}
