using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] [Tooltip("Maximum health of entity.")] public float maxHealth;
    [SerializeField] [Tooltip("Current health of entity.")] public float currentHealth;
    [SerializeField] [Tooltip("Graphical representation of the current health for entity.")] Image healthBar;
    [SerializeField] [Tooltip("Numerical representation of the health for entity.")] TMP_Text currentHealthText;
    //[SerializeField] TMP_Text floatingNumber;
    new Renderer renderer;
    Animator anim;

    [Header("GodMode")] 
    [SerializeField] [Tooltip("Checking this box disallows subtraction of health for entity")] bool isInvulnerable;

    void Awake()
    {
        currentHealth = maxHealth;
        currentHealthText.text = $"{currentHealth} / {maxHealth}";
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        currentHealthText.text = $"{currentHealth} / {maxHealth}";
        StartCoroutine(UpdateHealthBar());
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
    }

    public void TakeDamage(float amount)
    {
        if (isInvulnerable) return;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            //code to be executed upon death
            currentHealth = 0;
            isInvulnerable = true;
        }
        //update health bar and text.
        StartCoroutine(UpdateHealthBar());
        currentHealthText.text = $"{currentHealth} / {maxHealth}";
        if(isPlayer)
            CameraShake.Instance.ShakeCamera(0.1f, .1f, 0.2f);
    }
    
    IEnumerator UpdateHealthBar()
    {
        var start = healthBar.fillAmount;
        var end = currentHealth / maxHealth;
        var t = 0f;
        var time = .1f;
        while (t < 1)
        {
            yield return null;
            t += Time.deltaTime / time;
            healthBar.fillAmount = Mathf.Lerp(start, end, t);
        }
        start = end;
    }
}
