using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] bool isPlayer;
    [SerializeField] GameObject[] players;
    [Header("Health")]
    [SerializeField] GameObject healingEffect;
    [SerializeField] [Tooltip("Maximum health of entity.")] public int maxHealth;
    [SerializeField] [Tooltip("Current health of entity.")] public int currentHealth;
    int accumulatedDamage;
    [SerializeField] [Tooltip("The time it takes to update the health bar relative to the amount of health restored or lost.")] [Range(0.01f, 2f)] float fillTime = 0.4f;
    [SerializeField] [Tooltip("Graphical representation of the current health for entity.")] Image healthBar;
    [SerializeField] [Tooltip("Numerical representation of the health for entity.")] TMP_Text currentHealthText;

    [Header("GodMode")] 
    [SerializeField] [Tooltip("Checking this box disallows subtraction of health for entity")] bool isInvulnerable;
    [HideInInspector] public bool canBeHealed;
    [HideInInspector] public bool isDead;
    Coroutine updateHealthBar;

    void Awake()
    {
        currentHealth = maxHealth;
        canBeHealed = false;
        currentHealthText.text = $"{currentHealth} / {maxHealth}";
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    /// <summary>
    /// Heals the entity by a certain amount.
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(int amount)
    {
        if (isDead) return;
        if (!canBeHealed) return;
        accumulatedDamage -= amount;
        StartHealthUpdate();
        if (isPlayer)
        { 
            foreach(var player in players)
            {
                var effect = Instantiate(healingEffect, player.transform.position, Quaternion.identity);
                effect.transform.SetParent(player.transform);
            }
        }
    }

    /// <summary>
    /// Increases the maximum health of the entity by a certain amount.
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseMaxHealth(int amount)
    {
        if(isDead) return;
        maxHealth += amount;
    }

    /// <summary>
    /// Decreases the current health of the entity by a certain amount.
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;
        accumulatedDamage += amount;
        StartHealthUpdate();
        if (isPlayer)
            CameraShake.Instance.ShakeCamera(0.1f, .5f, 0.1f);
    }

    /// <summary>
    /// Kills the entity.
    /// </summary>
    void Die() 
    {
        if(isDead) return;
        canBeHealed = false;
        isInvulnerable = true;
        currentHealth = 0;
        print($"{name} DIED LOL");
        if (isPlayer)
        {
            //player dead event maybe?
        }
        isDead = true;
    }

    /// <summary>
    /// Starts the health update coroutine.
    /// </summary>
    public void StartHealthUpdate()
    {
        if (updateHealthBar != null)
            StopCoroutine(updateHealthBar);
        updateHealthBar = StartCoroutine(UpdateHealthBar());
    }

    /// <summary>
    /// Smoothly updates the health bar over time based on the amount of health restored or lost.
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateHealthBar()
    {
        var t = 0f;
        var startHealth = currentHealth;
        var absAccumulatedDamage = MathF.Abs(accumulatedDamage);
        var duration = fillTime;
        while (absAccumulatedDamage > 0)
        {
            for (int i = 0; i < absAccumulatedDamage; i++)
            {
                UpdateHealthStatus();
                if (accumulatedDamage > 0)
                {
                    currentHealth--;
                    accumulatedDamage--;
                }
                else if (accumulatedDamage < 0)
                {
                    currentHealth++;
                    accumulatedDamage++;
                }
                UpdateHealthUI();
                yield return new WaitForSeconds(duration / absAccumulatedDamage);
            }
            duration -= Time.deltaTime;
            yield return null;
        }
        UpdateHealthStatus();
        UpdateHealthUI();
        accumulatedDamage = 0;
    }

    /// <summary>
    /// Updates the health status of the entity based on the current health.
    /// </summary>
    void UpdateHealthStatus()
    {
        if (currentHealth - accumulatedDamage >= maxHealth)
            canBeHealed = false;
        else if (currentHealth - accumulatedDamage <= 0)
            Die();
        else if (currentHealth < maxHealth && !isDead)
            canBeHealed = true;
    }

    /// <summary>
    /// Updates the health UI with the current health and maximum health values.
    /// </summary>
    void UpdateHealthUI()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentHealthText.text = $"{currentHealth} / {maxHealth}";
        healthBar.fillAmount = currentHealth / (float)maxHealth;
    }
}
