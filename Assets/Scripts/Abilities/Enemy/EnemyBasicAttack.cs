using UnityEngine;

public class EnemyBasicAttack : Ability
{
    private void Awake()
    {
        Enemy enemy = GetComponent<Enemy>();
        Range = enemy.attackRange;
        Damage = enemy.damage;
    }
    void Update()
    {
        RunCooldown();
    }
}
