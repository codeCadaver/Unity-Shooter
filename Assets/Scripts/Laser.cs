using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private bool _isHeatSeeking = true;
    [SerializeField] private bool _isEnemyLaser = false;
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _maxY = 9f;
    [SerializeField] private LayerMask _ownerLayer, _friendLayer;

    private GameObject[] _enemies = new GameObject[2];
    private Rigidbody2D _rigidbody2D;
    private float _laserPositionY;

    
    // Start is called before the first frame update
    void Start()
    {
        _laserPositionY = transform.position.y;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        EnemyCheck();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Destroy();
    }

    private void CheckForEnemies()
    {
        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_enemies[i] == null)
            {
                GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
                if (enemy != null)
                {
                    _enemies[i] = enemy;
                    Debug.Log("Enemy Added to array");
                }
            }
        }
    }

    private void Movement()
    {
        if (_isHeatSeeking)
        {
            CheckForEnemies();
            Debug.Log("Heatseeking active");
            var target = _enemies[0];

            if (target == null)
            {
                target = _enemies[1];
            }
            if (target != null)
            {
                Debug.Log("Enemies Detected");
                // var q = Quaternion.LookRotation(target.transform.position - transform.position);
                // transform.rotation = Quaternion.RotateTowards(transform.rotation, q, _speed * Time.deltaTime);
            }
        }
        transform.Translate(Vector3.up * (Time.deltaTime * _speed), Space.Self);
    }

    private void Destroy()
    {
        if(ScreenCheck())
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void EnemyCheck()
    {
        if (_isEnemyLaser)
        {
            _speed = -_speed;
        }
    }

    private bool ScreenCheck()
    {
        if (_isEnemyLaser)
        {
            return  transform.position.y < -_maxY ;
        }
        else
        {
            return transform.position.y > _maxY;
        }
    }
    
}
