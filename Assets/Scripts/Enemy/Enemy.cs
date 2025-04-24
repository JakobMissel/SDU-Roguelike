using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public GameObject currentTarget;
    EnemyBasicAttack enemyBasicAttack;
    [Tooltip("The distance between enemy and its target before it will perform its attack.")] public float attackRange = 1.5f;
    [Tooltip("The distance between enemy and its target before it will perform a dash.")] public float dashRange = 3f;
    [SerializeField] AudioClip attackAudioClip;
    public int damage = 5;

    void Awake()
    {
        enemyBasicAttack = GetComponent<EnemyBasicAttack>();
        if (enemyBasicAttack == null)
        {
            Debug.LogError("EnemyBasicAttack component is missing on the enemy.");
            return;
        }
    }

    public void Attack(GameObject target)
    {
        if (enemyBasicAttack.CurrentCooldown == 0 && attackAudioClip != null)
        {
            target.GetComponentInParent<AudioSource>().PlayOneShot(attackAudioClip);
        }
        enemyBasicAttack.ActivateAbility();
        //target.GetComponentInParent<Health>().TakeDamage(damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

    }
}
