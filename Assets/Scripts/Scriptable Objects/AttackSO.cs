using UnityEngine;

namespace Attacks
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObjects/Attack", order = 1)]
    public class AttackSO : ScriptableObject
    {
        [Header("Prefabs")] 
        public GameObject prefab;
        
        [Header("Info")] 
        public string attackName;
        public string description;
        
        [Header("Stats")]
        public int damage;
        public float range;
        public float attackSpeedMultiplier;
        public float attackDelay;
        public float cooldown;
        
        [Header("Ranged Attack Settings")] 
        public bool isRanged;
        public int maxProjectile;
        public float projectileSpeed;
        public int piercing;
    }
}
