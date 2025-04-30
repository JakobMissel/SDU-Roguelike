using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform spawnArea;
    [SerializeField] float spawnRadius = 5f;
    [SerializeField] [TextArea] string spawnText = "CLEANS THE CORRUPTION!";
    TextMeshProUGUI spawnMessage;
    public GameObject[] enemyPrefabs;
    public int numberOfEnemiesToSpawn = 5;
    public int zoneLevel = 1;
    bool hasSpawned = false;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        spawnMessage = GameObject.Find("SpawnMessage").GetComponent<TextMeshProUGUI>();
        spawnMessage.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            var randomPosition = spawnArea.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0; // Keep the spawn position on the ground
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            var enemy = Instantiate(enemyPrefabs[randomIndex], randomPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetLevel(zoneLevel);
        }
        spawnMessage.gameObject.SetActive(true);
        spawnMessage.text = spawnText;
        hasSpawned = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnArea.position, spawnRadius);
    }
}
