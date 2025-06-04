using TMPro;
using UnityEngine;
using System.Collections;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public TMP_Text text;
    [SerializeField] float fadeTime = 1f;
    [SerializeField] float minUpVelocity = 1f;
    [SerializeField] float maxUpVelocity = 5f;
    [SerializeField] float minSideVelocity = 0.5f;
    [SerializeField] float maxSideVelocity = 2f;

    void Awake()
    {
        float upVelocity = Random.Range(minUpVelocity, maxUpVelocity);
        float sideVelocity = Random.Range(minSideVelocity, maxSideVelocity);
        rb.linearVelocity = new Vector3(Random.Range(-sideVelocity, sideVelocity), upVelocity, 0);
        StartCoroutine(FadeOut());
    }

    public void SetDamageText(string damage)
    {
        text.text = damage;
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color initialColor = text.color;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            text.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }
        Destroy(gameObject);
    }
}
