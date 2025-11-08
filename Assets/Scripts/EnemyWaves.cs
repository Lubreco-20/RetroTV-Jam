using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] private Waves waveList;
    private int currentWave = 0;

    [Header("SpawnArea")]
    public List<Transform> spawnPointList = new List<Transform>();

    [Header("Enemies")]
    private List<GameObject> enemyPool = new List<GameObject>();
    private float spawnTimer;

    [SerializeField] private float maxEnemiesAllowed = 100f;
    private int enemyToSpawn = 0; // Cantidad máxima de enemigos que va a spawnear
    private int enemieKilled = -1; // Cantidad de enemigos que tiene que matar para pasar a la siguiente ronda

    private bool bossAlreadySpawned = false;

    private void Start()
    {
        InitializeValues();
    }

    private void InitializeValues()
    {
        InstantiateAllEnemies();
        StartWave();
    }

    private void InstantiateAllEnemies()
    {
        List<EnemyData> allEnemies = new List<EnemyData>();

        for (int i = 0; i < waveList.waves.Count; i++)
        {
            foreach (EnemyData enemy in waveList.waves[i].enemyList)
            {
                allEnemies.Add(enemy);
            }
        }

        // Ordenamos la lista por cantidad de enemigos de mayor a menor
        List<EnemyData> everyEnemy = new List<EnemyData>();
        everyEnemy = allEnemies
            .GroupBy(e => e.enemyId) //Agrupamos por Id
            .Select(group => group.First()) //Seleccionamos el de mayor cantidad de enemigos posibles de spawnear de cada grupo
            .ToList();

        foreach (EnemyData enemy in everyEnemy)
        {
            GameObject newEnemy;

            for (int i = 0; i < 100; i++)
            {
                newEnemy = Instantiate(enemy.enemyPrefab, Vector2.zero, Quaternion.identity, transform);
                newEnemy.name = enemy.enemyPrefab.name;
                newEnemy.SetActive(false);
                enemyPool.Add(newEnemy);
            }
        }
    }

    private void Update()
    {
        if (currentWave < waveList.waves.Count - 1)
        {
            if (enemieKilled <= 0)
            {
                currentWave++;
                StartWave();
            }

            spawnTimer += Time.deltaTime;

            if (spawnTimer > waveList.waves[currentWave].enemySpawnInterval)
            {
                spawnTimer = 0f;
                EnemySpawner();
            }
        }
    }

    private void StartWave()
    {
        if (currentWave < waveList.waves.Count - 1)
        {
            enemyToSpawn = waveList.waves[currentWave].waveQuota;
            enemieKilled = enemyToSpawn;
            bossAlreadySpawned = false;
        }
    }

    private void EnemySpawner()
    {
        if (enemyToSpawn >= maxEnemiesAllowed)
            return;

        var wave = waveList.waves[currentWave];

        var bossEnemy = wave.enemyList.FirstOrDefault(e => e.isBoss);

        if(bossEnemy != null && !bossAlreadySpawned)
        {
            if (enemyPool.FirstOrDefault(e => e.name == bossEnemy.enemyPrefab.name && !e.activeInHierarchy))
            {
                bossAlreadySpawned = true;
                SpawnEnemy(bossEnemy);
                return;
            }
        }

        float totalWeight = wave.enemyList.Sum(e => e.spawnProbability);
        float rnd = Random.Range(0f, totalWeight);

        float cmltv = 0f;
        EnemyData enemySelected = wave.enemyList[0]; // Default

        foreach (var enemy in wave.enemyList)
        {
            cmltv += enemy.spawnProbability;
            if (rnd <= cmltv)
            {
                enemySelected = enemy;
                break;
            }
        }

        SpawnEnemy(enemySelected);
    }

    private void SpawnEnemy(EnemyData enemySelected)
    {
        GameObject enemyToActivate = enemyPool.FirstOrDefault(e => e.name == enemySelected.enemyPrefab.name && !e.activeInHierarchy);

        if (enemyToActivate != null)
        {
            EnemyHealth enemyHealth = enemyToActivate.GetComponent<EnemyHealth>();

            enemyToActivate.transform.position = SelectSpawnPoint();
            enemyToActivate.SetActive(true);

            enemyHealth.OnEnemyDeath += OnEnemyKilled;

            enemyToSpawn++;
        }
    }

    private Vector3 SelectSpawnPoint()
    {
        int rndPoint = Random.Range(0, spawnPointList.Count);
        return spawnPointList[rndPoint].position;
    }

    private void OnEnemyKilled(EnemyHealth enemy)
    {
        enemieKilled--;

        enemy.OnEnemyDeath -= OnEnemyKilled;
    }
}