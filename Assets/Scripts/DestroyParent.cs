using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    [SerializeField] bool destroyGrandparent = false;
    public void DestroyMyParent()
    {
        if (destroyGrandparent)
        {
            if (transform.parent.parent != null)
            {
                Destroy(transform.parent.parent.gameObject);
            }
        }
        else
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
