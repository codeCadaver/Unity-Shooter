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
    private GameObject player;
    private PlayerPrototype _player;
    private Rigidbody2D _rigidbody2D;
    private int _movementType = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        player = GameObject.FindWithTag("Player");

        StartCoroutine(FireRoutine());
        
        SetMovementType(3);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void SetMovementType(int movements)
    {
        _movementType = Random.Range(0, movements+1);
    }

    private void Movement()
    {
        

        // move down
        switch (_movementType)
        {
            case 0: 
                MoveDown();
                break;
            case 1:
                MoveTowardsPlayer();
                break;
                
            default:
                MoveDown();
                break;
        }
        
    }

    private void MoveDown()
    {
        Vector3 newPosition = transform.position;
        transform.Translate(Vector2.down * (Time.deltaTime * _speed));
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

    private void MoveTowardsPlayer()
    {
        GameObject player = GameObject.Find("Player2D");
        MoveDown();
        // move along x axis towards player
        if (player != null)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime);
            newPosition.y = transform.position.y;
            transform.position = newPosition;

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
