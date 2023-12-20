using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] enemyPrefabs;

    public GameObject explosionWavePrefab;
    public GameObject extraBouncePrefab;
    public GameObject bulletsLaunchPrefab;
    public GameObject extraGripPrefab;
    public GameObject forceAuraPrefab;
    public GameObject openCenterPitPrefab;
    
    private GameManager gameManager;

    private const float spawnRange = 9f;

    private const float generalPowerupGenerationInterval = 8f;

    private const int bossAppearanceVawesInterval = 2;

    private static Vector3 specificSpawnPos1 = new Vector3(0.85f, 0f, -11.7f);
    private static Vector3 specificSpawnPos2 = new Vector3(-11.55f, 0f, 0.28f);
    private static Vector3 specificSpawnPos3 = new Vector3(12.95f, 0f, 0.28f);
    private static Vector3 specificSpawnPos4 = new Vector3(0.85f, 0f, 11.7f);

    private int enemyCount;
    

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        SpawnEnemyWave(gameManager.GetCurrentWaveNumber());

        Instantiate(extraBouncePrefab, GenerateRandomSpawnPosition(), extraBouncePrefab.transform.rotation);
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            gameManager.IncrementWaveNumber();

            SpawnEnemyWave(gameManager.GetCurrentWaveNumber());

            StartCoroutine(GeneratePowerupCountdownRoutine());
        }
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        if (IsBossSpawnConditionMet()) {
            InstantiateEnemyBySpecificType(EnemyType.Boss);
        }
        else
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if (i > 0 && i == enemiesToSpawn - 1)
                {
                    InstantiateEnemyBySpecificType(EnemyType.EliteEnemy);
                }
                else
                {
                    InstantiateEnemyBySpecificType(EnemyType.Enemy);
                }
            }
        }
    }

    private Vector3 GenerateRandomSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return spawnPos;
    }

    private Vector3 GenerateSpecificSpawnPosition()
    {
        Vector3 spawnPos;

        switch (Random.Range(1, 5))
        {
            case 1:
                spawnPos = specificSpawnPos1;
                break;
            case 2:
                spawnPos = specificSpawnPos2;
                break;
            case 3:
                spawnPos = specificSpawnPos3;
                break;
            default:
                spawnPos = specificSpawnPos4;
                break;
        }
        return spawnPos;
    }

    private void GenerateRandomPowerup()
    {
        int randomBuffIdx = Random.Range(0, 6);

        switch (randomBuffIdx)
        {
            case 0:
                if (GameObject.FindGameObjectsWithTag(nameof(PowerupType.EXPLOSION_WAVE)).Length == 0) {
                    Instantiate(explosionWavePrefab, GenerateRandomSpawnPosition(), explosionWavePrefab.transform.rotation);
                }
                break;
            case 1:
                if (GameObject.FindGameObjectsWithTag(nameof(PowerupType.EXTRA_BOUNCE)).Length == 0)
                {
                    Instantiate(extraBouncePrefab, GenerateRandomSpawnPosition(), extraBouncePrefab.transform.rotation);
                }
                break;
            case 2:
                if (GameObject.FindGameObjectsWithTag(nameof(PowerupType.BULLET_LAUNCH)).Length == 0)
                {
                    Instantiate(bulletsLaunchPrefab, GenerateRandomSpawnPosition(), bulletsLaunchPrefab.transform.rotation);
                }
                break;
            case 3:
                if (GameObject.FindGameObjectsWithTag(nameof(PowerupType.EXTRA_GRIP)).Length == 0)
                {
                    Instantiate(extraGripPrefab, GenerateRandomSpawnPosition(), extraGripPrefab.transform.rotation);
                }
                break;
            case 4:
                if (GameObject.FindGameObjectsWithTag(nameof(PowerupType.FORCE_AURA)).Length == 0)
                {
                    Instantiate(forceAuraPrefab, GenerateRandomSpawnPosition(), forceAuraPrefab.transform.rotation);
                }
                break;
            case 5:
                //Open center pit only for boss or elite, and only at specific positions of arena
                if (GameObject.FindGameObjectsWithTag(nameof(EnemyType.EliteEnemy)).Length >= 1
                    || GameObject.FindGameObjectsWithTag(nameof(EnemyType.Boss)).Length >= 1)
                {
                    Instantiate(openCenterPitPrefab, GenerateSpecificSpawnPosition(), openCenterPitPrefab.transform.rotation);
                }
                break;
            default:
                break;
        }
    }

    IEnumerator GeneratePowerupCountdownRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(generalPowerupGenerationInterval);
            GenerateRandomPowerup();
        }
    }

    private bool IsBossSpawnConditionMet()
    {
        return gameManager.GetCurrentWaveNumber() >= bossAppearanceVawesInterval
            && gameManager.GetCurrentWaveNumber() % bossAppearanceVawesInterval == 0;
    }

    private void InstantiateEnemyBySpecificType(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Enemy:
                Instantiate(enemyPrefabs[0], GenerateRandomSpawnPosition(), enemyPrefabs[0].transform.rotation);
                break;
            case EnemyType.EliteEnemy:
                Instantiate(enemyPrefabs[1], GenerateRandomSpawnPosition(), enemyPrefabs[0].transform.rotation);
                break;
            case EnemyType.Boss:
                Instantiate(enemyPrefabs[2], GenerateRandomSpawnPosition(), enemyPrefabs[0].transform.rotation);
                break;
            default:
                break;
        }
    }
}