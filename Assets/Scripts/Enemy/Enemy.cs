using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public GameObject currentTarget;
    EnemyBasicAttack enemyBasicAttack;
    Health health;
    NavMeshAgent navMeshAgent;
    [SerializeField] bool isRanged;
    [SerializeField] Animator animator;
    [Tooltip("The distance between enemy and its target before it will perform its attack.")] public float attackRange = 1.5f;
    [Tooltip("The distance between enemy and its target before it will perform a dash.")] public float dashRange = 3f;
    [Tooltip("The distance between enemy and its target before it attempt to flee.")] public float fleeRange = 1f;
    [SerializeField] AudioClip attackAudioClip;
    public int damage = 5;
    public int maxHealth = 100;
    public float speed = 3.5f;
    public int level = 1;
    public float levelMultiplier = 1.2f;

    void Awake()
    {
        enemyBasicAttack = GetComponent<EnemyBasicAttack>();
        if (enemyBasicAttack == null)
        {
            Debug.LogError("EnemyBasicAttack component is missing on the enemy.");
            return;
        }
        health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("Health component is missing on the enemy.");
            return;
        }
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the enemy.");
            return;
        }
        navMeshAgent.speed = speed;
    }

    public void Attack(GameObject target)
    {
        if (enemyBasicAttack.CurrentCooldown == 0 && attackAudioClip != null)
        {
            if(isRanged)
                animator.Play("RangedAttack");
            else
                animator.Play("MeleeAttack");
            target.GetComponentInParent<AudioSource>().PlayOneShot(attackAudioClip);
        }
        enemyBasicAttack.ActivateAbility();
    }

    public void SetLevel(int level)
    {
        if (level > 0)
        {
            damage = Mathf.RoundToInt(damage * Mathf.Pow(levelMultiplier, level));
            maxHealth = Mathf.RoundToInt(maxHealth * Mathf.Pow(levelMultiplier, level));
        }
        if (enemyBasicAttack != null)
        {
            enemyBasicAttack.Damage = damage;
        }
        if (health != null)
        {
            health.maxHealth = maxHealth;
            health.currentHealth = maxHealth;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
