using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _xPosition, _yPosition;

    private bool _canSpawn = true;
    private WaitForSeconds _wait;
    
    // Start is called before the first frame update
    void Start()
    {
        _wait = new WaitForSeconds(_spawnDelay);
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
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

    public void StopSpawning()
    {
        _canSpawn = false;
    }
}
