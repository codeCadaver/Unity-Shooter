using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public static Action<int> OnEnemyDestroyed;

    [SerializeField] private int _enemyValue = 10;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _screenTopY, _screenBottomY;
    [SerializeField] private float _randomX;

    private Animator _animator;
    private bool _isDead = false;
    private Collider2D _collider2D;
    private int _deathHash = Animator.StringToHash("ExplosionTrigger");
    private PlayerPrototype _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 newPosition = transform.position;
        // move down
        transform.Translate(Vector3.down *(Time.deltaTime * _speed));
        // if bottom of screen
        if (transform.position.y < _screenBottomY)
        {
            if (!_isDead)
            {
                // move to top
                newPosition.y = _screenTopY;
                newPosition.x = Random.Range(-_randomX, _randomX);
                transform.position = newPosition;
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            _player = other.GetComponent<PlayerPrototype>();
            if (_player != null)
            {
                _player.DamagePlayer();
            }
            else
            {
                Debug.LogError("PlayerPrototype = null");
            }
            
            Destroy(this.gameObject);
        }
    
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (OnEnemyDestroyed != null)
            {
                OnEnemyDestroyed(_enemyValue);
            }
            Destroy(this.gameObject);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.GetComponent<PlayerPrototype>();
            if (_player != null)
            {
                _player.DamagePlayer();
            }
            else
            {
                Debug.LogError("PlayerPrototype = null");
            }
            // Destroy(this.gameObject);
            EnemyDestructionAnimation();
            
        }
    
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            OnEnemyDestroyed?.Invoke(_enemyValue);
            // Destroy(this.gameObject);
            EnemyDestructionAnimation();
        }
    }

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    private void EnemyDestructionAnimation()
    {
        _animator.SetTrigger(_deathHash);
        _isDead = true;
        _collider2D.enabled = false;
    }
}
