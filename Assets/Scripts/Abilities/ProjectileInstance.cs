using Unity.VisualScripting;
using UnityEngine;

public class ProjectileInstance : AbilityInstance {
    // Vector3 StartPosition;
    [SerializeField] float Distance;
    void Start() {
        StartPosition = SourceAbility.transform.position;
    }

    void Update() {
        transform.Translate(Vector3.forward * SourceAbility.ProjectTileSpeed * Time.deltaTime);
        Distance = Vector3.Distance(StartPosition,transform.position);
        if (Distance >= SourceAbility.Range){
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision other) {
        if ((other.gameObject.CompareTag("Enemy") && SourceAbility.PlayerAbility) || 
        (other.gameObject.CompareTag("Player") && !SourceAbility.PlayerAbility)){
            other.gameObject.GetComponent<Health>().TakeDamage(SourceAbility.CalculateDamage());
            Destroy(gameObject);
        }
        // if (other.gameObject.CompareTag("Player") && !SourceAbility.PlayerAbility){
        //     other.gameObject.GetComponent<Health>().TakeDamage(SourceAbility.CalculateDamage());
        //     Destroy(this);
        // }
        // switch(other.gameObject.tag){
        //     case "Enemy":
        //     break;
        //     case "Player":
        //     break;
        // }
    }
}
