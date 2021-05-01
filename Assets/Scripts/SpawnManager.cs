using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] private GameObject[] _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer, _powerupContainer;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private float _spawnEnemyDelay;
    [SerializeField] private float _minPowerupDelay, _maxPowerupDelay;
    [SerializeField] private float _xPosition, _yPosition;
    [SerializeField] private float _initialSpawnDelay = 2f;
    [SerializeField] private WaveManager _waveManager;

    private bool _canSpawn = false;
    private int _currentWave = 0;
    private int _enemiesToSpawn;
    private int _enemiesKilled = 0;
    private WaitForSeconds _wait;

    private void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _wait = new WaitForSeconds(_spawnEnemyDelay);
        
        SetWaveEnemies();
    }

    private void SetWaveEnemies()
    {
        if (_waveManager != null)
        {
            _enemiesToSpawn = _waveManager.GetWave(_currentWave).enemies;
        }
        for (int i = 0; i < _enemyPrefab.Length; i++)
        {
            _enemyPrefab[i] = _waveManager.GetWave(_currentWave).enemyTypes[i];
        }
    }


    private void StartSpawning(bool canSpawn)
    {
        _canSpawn = canSpawn;
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_canSpawn)
        {
            // spawn enemy
            
            // Grab random enemy from WaveManager
            {
                GameObject randomEnemy = _enemyPrefab[Random.Range(0, _waveManager.GetWave(_currentWave).enemyTypes.Length)];
                if (_enemyPrefab != null)
                {
                    yield return new WaitForSeconds(_initialSpawnDelay);
                    Vector3 spawnPosition =
                        SpawnLocation(-_xPosition, _xPosition, _yPosition, transform.position.z);
                    // GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity, _enemyContainer.transform);
                    GameObject enemy = Instantiate(randomEnemy, spawnPosition, Quaternion.identity, _enemyContainer.transform);
                    _enemiesToSpawn--;
                }
                yield return _wait;
            }
            
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_canSpawn)
        {
            if (_powerups != null)
            {
                yield return new WaitForSeconds(_initialSpawnDelay);
                float randomDelay = Random.Range(_minPowerupDelay, _maxPowerupDelay);
                yield return new WaitForSeconds(randomDelay);
                int randomPowerup = Random.Range(0, _powerups.Length);
                Vector3 spawnPosition = SpawnLocation(-_xPosition, _xPosition, _yPosition, transform.position.z);
                GameObject powerup =
                    Instantiate(_powerups[randomPowerup], spawnPosition, Quaternion.identity, _powerupContainer.transform);
            }
        }
    }

    private void GetRemainingEnemies(int enemyScore)
    {
        _enemiesKilled++;
        Debug.Log($"Enemies Killed: {_enemiesKilled}");
        if (_enemiesKilled >= _waveManager.GetWave(_currentWave).enemies)
        {
            StopSpawning();
            
            StartCoroutine(NextWave());
        }
    }

    IEnumerator NextWave()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().SelfDestruct();
        }

        yield return new WaitForSeconds(2f);
        _currentWave++;
        _enemiesKilled = 0;
        _canSpawn = true;
    }

    public void StopSpawning()
    {
        _canSpawn = false;
    }

    private void OnEnable()
    {
        UIManager.OnStartGame += StartSpawning;
        Enemy.OnEnemyDestroyed += GetRemainingEnemies;
    }

    private void OnDisable()
    {
        UIManager.OnStartGame -= StartSpawning;
        Enemy.OnEnemyDestroyed -= GetRemainingEnemies;
    }

    private Vector3 SpawnLocation(float minX, float maxX, float startPositionY, float positionZ)
    {
        return new Vector3(Random.Range(minX, maxX), startPositionY, positionZ);
    }
}
