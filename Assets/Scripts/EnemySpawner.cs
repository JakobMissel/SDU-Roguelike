using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner")]
    public GameObject[] enemyPrefabs;
    public int numberOfEnemiesToSpawn = 5;
    public int zoneLevel = 1;
    [SerializeField] Transform spawnArea;
    [SerializeField] float spawnRadius = 5f;
    [SerializeField] [TextArea] string spawnText1 = "ATTENTION!";
    [SerializeField] [TextArea] string spawnText2 = "CLEANS THE CORRUPTION!";
    [SerializeField] [TextArea] string waveCompletedText = "PROCEED FURTHER INTO THE FOREST";
    TextMeshProUGUI spawnMessage;

    [Header("Light")]
    [SerializeField] Color openColor;
    [SerializeField] Color closedColor;
    [SerializeField] Light[] pointLight;

    [Header("Doors")]
    [SerializeField] GameObject[] doors;

    bool hasActiveEnemies = false;
    bool isActive = true;
    int slainEnemies = 0;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        spawnMessage = GameObject.Find("SpawnMessage").GetComponent<TextMeshProUGUI>();
        spawnMessage.text = "";
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyDeath += EnemySlain;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyDeath -= EnemySlain;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasActiveEnemies && isActive)
        {
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        ChangeDoorsAndLight(false);
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            var randomPosition = spawnArea.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0; // Keep the spawn position on the ground
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            var enemy = Instantiate(enemyPrefabs[randomIndex], randomPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetLevel(zoneLevel);
        }
        spawnMessage.gameObject.SetActive(true);
        StartCoroutine(SetText(spawnText1, spawnText2));
        hasActiveEnemies = true;
    }

    void ChangeDoorsAndLight(bool isOpen)
    {
        for (int i = 0; i < pointLight.Length; i++)
        {
            pointLight[i].color = isOpen ? openColor : closedColor;
        }
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetActive(!isOpen);
        }
    }

    IEnumerator SetText(string text1, string text2)
    {
        spawnMessage.text = text1;
        yield return new WaitForSeconds(1f);
        spawnMessage.text = "";
        foreach (char letter in text2.ToCharArray())
        {
            spawnMessage.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        spawnMessage.gameObject.SetActive(false);
    }

    void EnemySlain()
    {
        if(!hasActiveEnemies || !isActive) return;
        slainEnemies++;
        Debug.Log("Enemy slain: " + slainEnemies);
        if (slainEnemies >= numberOfEnemiesToSpawn) 
        {
            WaveCompleted();
        }
    }

    private void WaveCompleted()
    {
        hasActiveEnemies = false;
        isActive = false;
        ChangeDoorsAndLight(true);
        spawnMessage.gameObject.SetActive(true);
        StartCoroutine(SetText("", waveCompletedText));
        slainEnemies = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnArea.position, spawnRadius);
    }
}
