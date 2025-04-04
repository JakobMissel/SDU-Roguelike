using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] [Tooltip("Maximum health of entity.")] public int maxHealth;
    [SerializeField] [Tooltip("Current health of entity.")] public int currentHealth;
    int newHealth;
    [SerializeField] [Tooltip("The time it takes to update the health bar relative to the amount of health restored or lost.")] float fillTime = 0.4f;
    [SerializeField] [Tooltip("Graphical representation of the current health for entity.")] Image healthBar;
    [SerializeField] [Tooltip("Numerical representation of the health for entity.")] TMP_Text currentHealthText;

    [Header("GodMode")] 
    [SerializeField] [Tooltip("Checking this box disallows subtraction of health for entity")] bool isInvulnerable;
    [HideInInspector] public bool canBeHealed;
    [HideInInspector] public bool isDead;

    void Awake()
    {
        currentHealth = maxHealth;
        canBeHealed = false;
        currentHealthText.text = $"{currentHealth} / {maxHealth}";
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        if (!canBeHealed) return;
        newHealth = currentHealth;
        newHealth += amount;
        StopCoroutine(UpdateHealthBar());
        StartCoroutine(UpdateHealthBar());
    }

    public void IncreaseMaxHealth(int amount)
    {
        if(isDead) return;
        maxHealth += amount;
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;
        newHealth = currentHealth;
        newHealth -= amount;
        StopCoroutine(UpdateHealthBar());
        StartCoroutine(UpdateHealthBar());
        if(isPlayer)
            CameraShake.Instance.ShakeCamera(0.1f, .5f, 0.1f);
    }

    void Dead() 
    {
        //code to be executed upon death
        print($"{name} DIED LOL");
        isDead = true;
        isInvulnerable = true;
        canBeHealed = false;
        if (isPlayer)
        {
            //player dead event maybe?
        }

    }
    IEnumerator UpdateHealthBar()
    {
        var t = 0f;
        var fillTimeWait = fillTime / Mathf.Abs(newHealth - currentHealth);
        while (t < 1)
        {
            t += Time.deltaTime / fillTime;
            yield return new WaitForSeconds(fillTimeWait);
            UpdateHealthStatus();
        }
        currentHealth = newHealth;
        UpdateHealthStatus();
    }
    void UpdateHealthStatus()
    {
        if (newHealth < currentHealth)
            currentHealth -= 1;
        else if (newHealth > currentHealth)
            currentHealth += 1;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            canBeHealed = false;
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            Dead();
        }
        else if (currentHealth < maxHealth)
        {
            canBeHealed = true;
        }
        currentHealthText.text = $"{currentHealth} / {maxHealth}";
        healthBar.fillAmount = currentHealth / (float)maxHealth;
    }
}
