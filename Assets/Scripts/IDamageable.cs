using UnityEngine;

public interface IDamageable {
    T RetrieveComponent<T>();
    void TakeDamage(int damage);
    void Die();
}
