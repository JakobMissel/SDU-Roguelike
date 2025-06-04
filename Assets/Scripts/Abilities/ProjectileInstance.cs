using UnityEngine;

public class ProjectileInstance : AbilityInstance {
    [SerializeField] float Distance;
    void Start() {
        StartPosition = SourceAbility.transform.position;
    }

    void Update() {
        transform.Translate(Vector3.forward * SourceAbility.ProjectileSpeed * Time.deltaTime);
        Distance = Vector3.Distance(StartPosition,transform.position);
        if (Distance >= SourceAbility.Range){
            Destroy(gameObject);
        }
    }
    internal override void OnHit(Collider other) {
        Destroy(gameObject);
    }
}
