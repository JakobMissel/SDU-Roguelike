using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner")]
    public GameObject[] enemyPrefabs;
    public List<GameObject> spawnedEnemies = new();
    public int totalWaves = 1;
    public int completedWaves = 0;
    public int enemiesPerWave = 5;
    public int zoneLevel = 1;
    [SerializeField] Transform[] spawnAreas;
    [SerializeField] float spawnRadius = 5f;
    [SerializeField] TextMeshProUGUI spawnMessage;
    [SerializeField] [TextArea] string spawnText1 = "ATTENTION!";
    [SerializeField] [TextArea] string spawnText2 = "CLEANS THE CORRUPTION!";
    [SerializeField] [TextArea] string waveCompletedText = "PROCEED FURTHER INTO THE FOREST";
    int playersInZone = 0;

    [Header("Light")]
    [SerializeField] Color openColor;
    [SerializeField] Color closedColor;
    [SerializeField] Light[] pointLight;

    [Header("Doors")]
    [SerializeField] GameObject[] doors;

    bool hasActiveEnemies = false;
    bool isActive = true;
    int waveEnemiesSlain = 0;

    void Awake()
    {
        completedWaves = 0;
        playersInZone = 0;
        GetComponent<Collider>().isTrigger = true;
        spawnMessage.text = "";
        zoneLevel = GameEvents.FloorLevel;
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
            playersInZone++;
            if (playersInZone > 2) // spirit has two colliders on it, so it counts as two players, therefore to check for both players it needs to be more  than 2 
            {
                BeginEncounter();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !hasActiveEnemies && isActive)
        {
            playersInZone--;
        }
    }

    public void BeginEncounter()
    {
        if (hasActiveEnemies || !isActive) return;
        ChangeDoorsAndLight(false);
        SpawnEnemies();
        spawnMessage?.gameObject.SetActive(true);
        StartCoroutine(SetText(spawnText1, spawnText2));
        hasActiveEnemies = true;
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            var randomPosition = spawnAreas[Random.Range(0, spawnAreas.Length)].position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0; // Keep the spawn position on the ground
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            var enemy = Instantiate(enemyPrefabs[randomIndex], randomPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy);
            enemy.GetComponent<Enemy>().SetLevel(zoneLevel);
        }
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
        waveEnemiesSlain++;
        if(waveEnemiesSlain >= spawnedEnemies.Count)
        {
            spawnedEnemies.Clear();
            waveEnemiesSlain = 0;
            completedWaves++;
            if (completedWaves >= totalWaves)
            {
                EncounterCompleted();
            }
            else
            {
                SpawnEnemies();
            }
        }
    }

    private void EncounterCompleted()
    {
        hasActiveEnemies = false;
        isActive = false;
        ChangeDoorsAndLight(true);
        spawnMessage.gameObject.SetActive(true);
        StartCoroutine(SetText("", waveCompletedText));
        waveEnemiesSlain = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < spawnAreas.Length; i++)
        {
            Gizmos.DrawWireSphere(spawnAreas[i].position, spawnRadius);
        }
    }
}
