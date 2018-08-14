using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : LivingEntity {
	public enum State { Idle, Patrolling, Attacking, Stunned, Dead};
	State state;
    public int damage;
}
