using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public static Action<int> OnEnemyDestroyed;

    [SerializeField] private int _enemyValue = 10;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _screenTopY, _screenBottomY;
    [SerializeField] private float _randomX;
    [SerializeField] private AudioClip _explosion;
    [SerializeField] private GameObject _enemyLaser;
    [SerializeField] private Transform _laserOffset;

    private Animator _animator;
    private AudioSource _audioSource;
    private bool _isDead = false;
    private Collider2D _collider2D;
    private int _deathHash = Animator.StringToHash("ExplosionTrigger");
    private PlayerPrototype _player;
    private Rigidbody2D _rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        StartCoroutine(FireRoutine());
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
                Debug.LogError("PlayerPrototype = NULL");
            }
            EnemyDestructionAnimation();
        }
    
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            OnEnemyDestroyed?.Invoke(_enemyValue);
            EnemyDestructionAnimation();
        }
    }

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    private void EnemyDestructionAnimation()
    {
        _audioSource.PlayOneShot(_explosion);
        _animator.SetTrigger(_deathHash);
        _isDead = true;
        _collider2D.enabled = false;
    }

    private void Fire()
    {
        // raycast
            // if hit.collider.compareTag(Player)
            Instantiate(_enemyLaser, _laserOffset.position, Quaternion.identity);
            // shoot laser

    }

    private IEnumerator FireRoutine()
    {
        while (!_isDead)
        {
            Fire();
            yield return new WaitForSeconds(1f);
        }
    }
}
