using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public static Action<int> OnEnemyDestroyed;

    [SerializeField] private bool _canDodge = true, _canShootBackwards = true;
    [SerializeField] private int _enemyTypes = 4;
    [SerializeField] private int _enemyValue = 10;
    [SerializeField] private float _fireDelay = 2f, _speed = 5f;
    [SerializeField] private float _screenTopY, _screenBottomY;
    [SerializeField] private float _randomX;
    [SerializeField] private AudioClip _explosion;
    [SerializeField] private GameObject _enemyLaser;
    [SerializeField] private Transform _laserOffset;

    private Animator _animator;
    private AudioSource _audioSource;
    private bool _canRam = true, _canFire = true;
    private bool _isDead = false;
    private Collider2D _collider2D;
    private int _deathHash = Animator.StringToHash("ExplosionTrigger");
    private GameObject player;
    private PlayerPrototype _player;
    private Rigidbody2D _rigidbody2D;
    private int _movementType = 0;
    private float _lastFired = 0f;

    public bool wasTarget = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        player = GameObject.FindWithTag("Player");

        // StartCoroutine(FireRoutine());
        
        SetMovementType(_enemyTypes);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void SetMovementType(int movements)
    {
        _movementType = Random.Range(0, movements);
    }

    private void Movement()
    {
        if (transform.position.y < player.transform.position.y)
        {
            _canRam = false;
            _speed = 5f;
        }

        // move down
        switch (_movementType)
        {
            case 0:
                RamPlayer();
                break;
            case 1: 
                MoveDown();
                break;
            case 2:
                MoveTowardsPlayer();
                break;
            case 3:
                RamPlayer();
                break;
                
            default:
                // MoveDown();
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

    private void RamPlayer()
    {
        float mainSpeed = _speed;
        MoveDown();
        Debug.Log("Enemy::RamPlayer Called!");
        RaycastHit2D hit = Physics2D.Raycast(_laserOffset.position, Vector3.down);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                _speed = 10;
                Fire();
            }
            // StartCoroutine(RamRoutine());
            else
            {
                _speed = 5f;
            }
        }
    }

    private void RacastTarget()
    {
        Debug.Log("Enemy::RacastTarget Called");
        RaycastHit2D hit2D = Physics2D.Raycast(_laserOffset.position, Vector2.down);

        // if (hit2D != null)
        if (hit2D.collider)
        {
            if (hit2D.collider.CompareTag("Player"))
            {
                // StartCoroutine(FireRoutine());
                Fire();
            }

            if (hit2D.collider.CompareTag("PowerUp"))
            {
                Fire();
            }
        }
    }

    private IEnumerator RamRoutine()
    {
        if (_canRam)
        {
            float mainSpeed = _speed;
            _speed = 2f;
            yield return new WaitForSeconds(1f);
            _speed = 10;
        }

        if (transform.position.y < player.transform.position.y)
        {
            Debug.Break();
                Debug.Log("Missed Player");
                _speed = 5;
                _canRam = false;
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
                OnEnemyDestroyed?.Invoke(_enemyValue);
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

    public void SelfDestruct()
    {
        EnemyDestructionAnimation();
    }

    private void Fire()
    {
        if (Time.time >= _lastFired)
        {
            Instantiate(_enemyLaser, _laserOffset.position, Quaternion.identity);
            _lastFired = Time.time + _fireDelay;
        }
    }

    private IEnumerator FireRoutine()
    {
        while (!_isDead)
        {
            if (_canFire)
            {
                _canFire = false;
                Fire();
                yield return new WaitForSeconds(_fireDelay);
                _canFire = true;
            }
        }
    }

    private void Dodge(float amount)
    {
        if (_canDodge)
        {
            Vector2 dodgePosition = transform.position;
            
            dodgePosition.x += amount;
            transform.position = dodgePosition;
        }
    }

    private void FixedUpdate()
    {
        RadarDetection();
        RacastTarget();
    }

    private void RadarDetection()
    {
        float xOffset = 0.5f;
        Vector2 radarPosition = _laserOffset.position;
        
        RaycastHit2D hit = Physics2D.Linecast(new Vector2(radarPosition.x - xOffset, radarPosition.y),
                                              new Vector2(radarPosition.x + xOffset, radarPosition.y));

        if (hit.collider != null)
        {
            var dodgeAmount = 1f;
            if (hit.collider.CompareTag("Laser"))
            {
                if (hit.collider.transform.position.x > this.transform.position.x)
                {
                    dodgeAmount = -dodgeAmount;
                    _canDodge = true;
                }
                Dodge(dodgeAmount);
            }
        }
    }
}
