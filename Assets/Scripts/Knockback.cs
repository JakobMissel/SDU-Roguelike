using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Knockback : MonoBehaviour
{
    private float knockbackDuration;
    private NavMeshAgent agent;
    private bool isBeingKnockedBack = false;
    private float knockbackTimer;
    private Vector3 knockbackVelocity;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public void KnockbackEnemy(Vector3 direction, float force, float duration) {
        if (isBeingKnockedBack) return;

        isBeingKnockedBack = true;
        knockbackTimer = 0f;
        knockbackDuration = duration;

        agent.isStopped = true;

        knockbackVelocity = direction.normalized * (force / duration); // Units per second

    }

    void Update() {
        if(isBeingKnockedBack) { // Apply knockback force
            knockbackTimer += Time.deltaTime;

            // Calculate next position
            Vector3 nextPosition = transform.position + knockbackVelocity * Time.deltaTime;

            // Clamp to valid NavMesh point (or skip movement if invalid)
            if (NavMesh.SamplePosition(nextPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }

            if (knockbackTimer >= knockbackDuration)
            {
                isBeingKnockedBack = false;
                agent.isStopped = false;
                agent.nextPosition = transform.position; // Sync NavMeshAgent
            }
        }
    }
}
