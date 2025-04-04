using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("\"Animation\"")]
    [SerializeField][Tooltip("Should the pickup move vertically?")] protected bool verticalAnimation = true;
    [SerializeField][Tooltip("The vertical speed of the pickup. \nDefault: 2")] protected float verticalSpeed = 2;
    [SerializeField][Tooltip("The height of the vertical movement. \nDefault: 0.5")] protected float verticalHeight = 0.5f;
    [SerializeField][Tooltip("Should the pickup rotate around itself?")] protected bool rotate = true;
    [SerializeField][Tooltip("The rotation speed of the pickup. \nDefault: 100")] protected float rotationSpeed = 100;
    float initialY;
    [Header("Pickup")]
    [SerializeField] AudioClip audioClip;
    bool hasAudioClip = false;
    [SerializeField] bool canBepickedUp = true;

    private void Awake()
    {
        if (audioClip != null) 
            hasAudioClip = true;
        initialY = transform.position.y;
    }
    void OnTriggerEnter(Collider other)
    {
        if(!canBepickedUp) return;
        if (other.CompareTag("Player"))
        {
            ActivatePickup(other.gameObject);
        }
    }

    protected virtual void ActivatePickup(GameObject player) 
    {
        if (hasAudioClip)
        {
            player.GetComponentInParent<AudioSource>().PlayOneShot(audioClip);
        }
        Destroy(gameObject);
    } 

    void Update() 
    { 
        Animate(verticalAnimation, verticalSpeed, rotate, rotationSpeed); 
    }

    void Animate(bool verticalAnimation, float verticalSpeed, bool rotate, float rotationSpeed)
    {
        if(verticalAnimation) 
            MoveUpAndDown(verticalSpeed);
        if (rotate) 
            Rotate(rotationSpeed);
    }

    void Rotate(float speed)
    { 
        transform.Rotate(Vector3.up * speed * Time.deltaTime); 
    }

    void MoveUpAndDown(float speed)
    {
        float height = initialY;
        float y = height + (Mathf.Sin(speed * Time.time) * verticalHeight);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
