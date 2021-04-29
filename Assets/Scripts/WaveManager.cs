using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int waveID;
    public GameObject[] enemyTypes;
    public int enemies;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] private int _maxWaves = 4;
    [SerializeField] private Wave[] _waves;
    // Start is called before the first frame update
    void Awake()
    {
        _waves = new Wave[_maxWaves];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Wave GetWave(int currentWave)
    {
        return _waves[currentWave];
    }
}
