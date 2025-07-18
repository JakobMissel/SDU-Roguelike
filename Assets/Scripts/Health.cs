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
    Animator[] animators = new Animator[2];
    [Header("Health")]
    [SerializeField] GameObject healingEffect;
    [SerializeField] [Tooltip("Maximum health of entity.")] public float maxHealth;
    [SerializeField] [Tooltip("Current health of entity.")] public float currentHealth;
    float accumulatedDamage;
    [SerializeField] [Tooltip("The time it takes to update the health bar relative to the amount of health restored or lost.")] [Range(0.01f, 2f)] float fillTime = 0.4f;
    [SerializeField] [Tooltip("Graphical representation of the current health for entity.")] Image[] healthBar;
    [SerializeField] [Tooltip("Numerical representation of the health for entity.")] TMP_Text[] currentHealthText;
    [SerializeField] [Tooltip("GameObject that visualizes a hit on the entity.")] GameObject[] hitVisualizer;
    [SerializeField] [Tooltip("How many seconds is the Hit Visualizer visible.")] [Range(0.01f, 0.5f)] float hitVisualizerDuration = 0.1f;
    [SerializeField] [Tooltip("Audio clip played when the entity takes damage.")] AudioClip damageTakenAudioClip;
    [SerializeField][Tooltip("Audio clip played when the entity is healed.")] AudioClip healAudioClip;
    AudioSource audioSource;
    Animator animator;
    [Header("GodMode")] 
    [SerializeField] [Tooltip("Checking this box disallows subtraction of health for entity")] bool isInvulnerable;

    [HideInInspector] public bool canBeHealed;
    [HideInInspector] public bool isDead;
    Coroutine updateHealthBar;

    void Awake()
    {
        if (isPlayer) {
            healthBar = new Image[] {
                GameObject.Find("CurrentPlayerHealth (1)").GetComponent<Image>(),
                GameObject.Find("CurrentPlayerHealth (2)").GetComponent<Image>()
            };
            currentHealthText = GameObject.Find("CurrentPlayerHealthText").GetComponentsInChildren<TMP_Text>();
        }
        currentHealth = maxHealth;
        canBeHealed = false;
        UpdateHealthUI();
        ShowHitVisualizer(false);
        audioSource = GetComponent<AudioSource>();
        if(!isPlayer)
            animator = GetComponentInChildren<Animator>();
        else
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null) continue;
                animators[i] = players[i].GetComponentInChildren<Animator>();
            }
        }
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
            if (audioSource != null && healAudioClip != null)
                audioSource.PlayOneShot(healAudioClip);
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
    public void TakeDamage(float amount, bool isPercentage = false)
    {
        if (isInvulnerable) return;
        if(isPercentage)
            amount = currentHealth * (amount / 100);
        accumulatedDamage += amount;
        accumulatedDamage = Mathf.CeilToInt(accumulatedDamage);
        StartHealthUpdate();
        if (isPlayer)
        {
            CameraShake.Instance.ShakeCamera(0.1f, 0.5f, 0.1f);
            if(audioSource != null && damageTakenAudioClip != null)
                audioSource.PlayOneShot(damageTakenAudioClip);
        }
        StartCoroutine(VisualHitIndicator());
    }

    /// <summary>
    /// Kills the entity.
    /// </summary>
    void Die() 
    {
        if(isDead) return;
        isDead = true;
        canBeHealed = false;
        isInvulnerable = true;
        currentHealth = 0;
        // DestrotUI();
        if (isPlayer)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null) continue;
                animators[i].Play("Die");
            }
            GameEvents.PlayerDeath();
        }
        else
        {
            animator.Play("Die");
            GameEvents.EnemyDeath();
        }
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

    void ShowHitVisualizer(bool toggle)
    {
        for (int i = 0; i < hitVisualizer.Length; i++)
        {
            if (hitVisualizer[i] == null) continue;
            hitVisualizer[i].SetActive(toggle);
        }
    }

    IEnumerator VisualHitIndicator()
    {
        ShowHitVisualizer(true);
        yield return new WaitForSeconds(hitVisualizerDuration);
        ShowHitVisualizer(false);
    }

    /// <summary>
    /// Smoothly updates the health bar over time based on the amount of health restored or lost.
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateHealthBar()
    {
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth - accumulatedDamage >= maxHealth)
            canBeHealed = false;
        else if (currentHealth <= 0)
            Die();
        else if (currentHealth < maxHealth && !isDead)
            canBeHealed = true;
    }

    /// <summary>
    /// Updates the health UI with the current health and maximum health values.
    /// </summary>
    void UpdateHealthUI()
    {
        currentHealth = Mathf.CeilToInt(currentHealth);
        for (int i = 0; i < healthBar.Length; i++)
        {
            if (healthBar[i] == null) continue;
            healthBar[i].fillAmount = currentHealth / (float)maxHealth;
        }
        for (int i = 0; i < currentHealthText.Length; i++)
        {
            if (currentHealthText[i] == null) continue;
            currentHealthText[i].text = $"{currentHealth} / {maxHealth}";
        }
    }

    void DestrotUI()
    {
        for (int i = 0; i < healthBar.Length; i++)
        {
            if (healthBar[i] == null) continue;
            Destroy(healthBar[i].gameObject);
        }
        for (int i = 0; i < currentHealthText.Length; i++)
        {
            if (currentHealthText[i] == null) continue;
            Destroy(currentHealthText[i].gameObject);
        }
    }
    
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
