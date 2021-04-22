using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] private GameObject _enemyPrefab, _asteroid;
    [SerializeField] private GameObject _enemyContainer, _powerupContainer;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private float _spawnEnemyDelay;
    [SerializeField] private float _minPowerupDelay, _maxPowerupDelay;
    [SerializeField] private float _xPosition, _yPosition;
    [SerializeField] private float _initialSpawnDelay = 2f;

    private bool _canSpawn = false;
    private WaitForSeconds _wait;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _wait = new WaitForSeconds(_spawnEnemyDelay);
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
            if (_enemyPrefab != null)
            {
                yield return new WaitForSeconds(_initialSpawnDelay);
                Vector3 spawnPosition =
                    SpawnLocation(-_xPosition, _xPosition, _yPosition, transform.position.z);
                GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity, _enemyContainer.transform);
            }
            yield return _wait;
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

    public void StopSpawning()
    {
        _canSpawn = false;
    }

    private void OnEnable()
    {
        UIManager.OnStartGame += StartSpawning;
    }

    private Vector3 SpawnLocation(float minX, float maxX, float startPositionY, float positionZ)
    {
        return new Vector3(Random.Range(minX, maxX), startPositionY, positionZ);
    }
}
