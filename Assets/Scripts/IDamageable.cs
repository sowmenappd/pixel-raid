using UnityEngine;

public interface IDamageable {
    void TakeDamage(int damage);
    //void TakeDamageOverTime(int dmg, int repeatRate);
    void Attack(float radius, int attackDamage, string tag);
    void Die();
}
