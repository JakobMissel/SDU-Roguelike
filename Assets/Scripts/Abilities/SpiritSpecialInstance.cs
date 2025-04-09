using UnityEngine;
using UnityEngine.UIElements;

public class SpiritSpecialInstance : AbilityInstance {
    [SerializeField] GameObject GameObjectToDestroy;
    internal override void OnHit(){
        Destroy(GameObjectToDestroy);
    }
}
