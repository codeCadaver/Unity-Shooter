using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer, _powerupContainer;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private float _spawnEnemyDelay;
    [SerializeField] private float _minPowerupDelay, _maxPowerupDelay;
    [SerializeField] private float _xPosition, _yPosition;

    private bool _canSpawn = true;
    private WaitForSeconds _wait;
    
    // Start is called before the first frame update
    void Start()
    {
        _wait = new WaitForSeconds(_spawnEnemyDelay);
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
                Vector3 spawnPosition =
                    new Vector3(Random.Range(-_xPosition, _xPosition), _yPosition, transform.position.z);
                GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity, _enemyContainer.transform);
            }
            // wait 5 seconds
            yield return _wait;
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_canSpawn)
        {
            if (_powerups != null)
            {
                float randomDelay = Random.Range(_minPowerupDelay, _maxPowerupDelay);
                yield return new WaitForSeconds(randomDelay);
                int randomPowerup = Random.Range(0, _powerups.Length);
                Vector3 spawnPosition = new Vector3(Random.Range(-_xPosition, _xPosition), _yPosition, transform.position.z);
                GameObject powerup =
                    Instantiate(_powerups[randomPowerup], spawnPosition, Quaternion.identity, _powerupContainer.transform);
            }
        }
    }

    public void StopSpawning()
    {
        _canSpawn = false;
    }
}
