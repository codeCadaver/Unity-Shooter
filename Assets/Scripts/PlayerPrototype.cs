using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;
using Random = UnityEngine.Random;

public class PlayerPrototype : MonoBehaviour
{
    public static Action<Vector2> OnPlayerMoved;

    public bool _is3D = false;

    [SerializeField] private Transform _playerTransform, _startTransform;
    [SerializeField] private bool _canWrap = false;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _tilt = 20f;
    [SerializeField] private float _rollSpeed = 20f;
    [SerializeField] private float _xWrap,_xClamp, _minY, _maxY;
    [SerializeField] private float _cooldownDelay = 0.5f;
    [SerializeField] private float _invincibleLength = 2f;
    [SerializeField] private GameObject _laser, _tripleShot, _shield, _explosion;
    [SerializeField] private int _lives = 3, _maxLives = 3;
    [SerializeField] private Transform _laserOffset;
    [SerializeField] private bool _tripleShotActive = false;
    [SerializeField] private float _powerUpTime = 5f;
    [SerializeField] private float _speedBoost = 8f;
    [SerializeField] private float _turboMultiplier = 1.5f, _turboAmount = 10f;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject[] _damageImages;
    [SerializeField] private AudioClip _laserSound, _explosionSound;

    private Animator _animator;
    private AudioSource _audioSource;
    private bool _canBarrelRoll = true;
    private bool _canFire = true;
    private bool _canTurbo = true;
    private bool _shieldActive = false;
    private Collider2D _collider2D;
    private float _currentSpeed;
    private float _lastFired = 0f;
    private float _right, _up;
    private GameObject _projectile;
    private int _invincibleHash = Animator.StringToHash("InvincibleTrigger");
    private int _rollLeftHash = Animator.StringToHash("RollLeft");
    private int _rollRightHash = Animator.StringToHash("RollRight");
    private Vector3 _newPosition;
    private Vector3 _playerRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _collider2D = GetComponent<Collider2D>();
        _currentSpeed = _movementSpeed;
        _uiManager.UpdateCurrentLivesImages(_lives);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        DoABarrelRoll(_right);
        GetPlayerRotation();
    }

    private void Movement()
    {
        _right = Input.GetAxis("Horizontal");
        _up = Input.GetAxis("Vertical");

        var movement = new Vector2(_right, _up);
        
        OnPlayerMoved?.Invoke(movement);
        
        _playerTransform.Translate(movement * (Time.deltaTime * _currentSpeed));
        _newPosition = _playerTransform.position;
        
        ClampVert();
        ClampHoriz();
        Fire();
        Thrusters();
    }

    private void Thrusters()
    {
        var turbo = _movementSpeed * _turboMultiplier;
        if (_turboAmount > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            _currentSpeed = _movementSpeed * _turboMultiplier;
        }
        else
        {
            _currentSpeed = _movementSpeed;
        }
    }

    private void Fire()
    {
        if (_tripleShotActive)
        {
            _projectile = _tripleShot;
        }
        else
        {
            _projectile = _laser;
        }
        if (_canFire)
        {
            if (Input.GetKey(KeyCode.Space) && Time.time > _lastFired)
            {
                if (_is3D)
                {
                    var laser = Instantiate(_projectile, _laserOffset.position, Quaternion.Euler(0, _playerRotation.y, 0));
                }
                else
                {
                    var laser = Instantiate(_projectile, _laserOffset.position, Quaternion.identity);
                }
                _audioSource.PlayOneShot(_laserSound);
                _lastFired = Time.time + _cooldownDelay;
            }
        }
    }

    private void DisableLaser()
    {
        _canFire = false;
    }

    private void ClampVert()
    {
        if (_playerTransform.position.y > _maxY)
        {
            _newPosition.y = _maxY;
        }

        if (_playerTransform.position.y < _minY)
        {
            _newPosition.y = _minY;
        } 
        
        _playerTransform.position = _newPosition;
    }

    private void ClampHoriz()
    {
        if (_canWrap)
        {
            if (_playerTransform.position.x > _xWrap)
            {
                _newPosition.x = -_xWrap;
            }

            if (_playerTransform.position.x < -_xWrap)
            {
                _newPosition.x = _xWrap;
            }
        }
        else
        {
            if (_playerTransform.position.x > _xClamp)
            {
                _newPosition.x = _xClamp;
            }

            if (_playerTransform.position.x < -_xClamp)
            {
                _newPosition.x = -_xClamp;
            }
        }

        _playerTransform.position = _newPosition;
    }

    private void OnEnable()
    {
        ExplodeUnity.OnExploded += DisableLaser;
        _playerTransform.position = _startTransform.position;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLaser"))
        {
            DamagePlayer();
        }
    }

    public void DamagePlayer()
    {
        if (_shieldActive)
        {
            _shieldActive = false;
            _shield.SetActive(false);
            return;
        }

        if (_lives > 0)
        {
            StartCoroutine(ReceivedDamagedRoutine());
            _lives--;
        }
        _uiManager.UpdateCurrentLivesImages(_lives);
        ShowDamage();
        
        if (_lives <= 0)
        {
            SpawnManager spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            spawnManager.StopSpawning();

            Instantiate(_explosion, transform.position, Quaternion.identity);
            foreach (var obj in transform)
            {
                transform.GetComponentInChildren<Transform>().gameObject.SetActive(false);
            }
        }
    }

    private void DoABarrelRoll(float direction)
    {
        // if ctrl key is pressed
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (_right < -.1)
            {
                _animator.SetTrigger(_rollLeftHash);
            }

            if (_right > .1f)
            {
                _animator.SetTrigger(_rollRightHash);
            }
        }
    }

    private void GetPlayerRotation()
    {
        _playerRotation = this.transform.rotation.eulerAngles;
    }


    IEnumerator BarrelRollRoutine()
    {
        
        yield return new WaitForSeconds(1f);
        
    }
    
    private IEnumerator RollRoutine()
    {
        yield return new WaitForSeconds(1f);
        _animator.SetTrigger(_rollLeftHash);
        yield return new WaitForSeconds(1f);
        _animator.SetTrigger(_rollRightHash);
    }

    private IEnumerator TripleShotRoutine()
    {
        _tripleShotActive = true;
        yield return new WaitForSeconds(_powerUpTime);
        _tripleShotActive = false;
    }

    public void TripleShot()
    {
        StartCoroutine(TripleShotRoutine());
    }
    
    private IEnumerator SpeedBoostRoutine()
    {
        _currentSpeed = _speedBoost;
        yield return new WaitForSeconds(_powerUpTime);
        _currentSpeed = _movementSpeed;
    }

    private void ShowDamage()
    {
        int randomDamage = Random.Range(0, _damageImages.Length);
        if (_lives > 1)
        {
            if (_damageImages[randomDamage].activeSelf == false)
            {
                _damageImages[randomDamage].SetActive(true);
            }
        }

        if (_lives == 1)
        {
            foreach (var obj in _damageImages)
            {
                obj.SetActive(true);
            }
        }
    }

    private IEnumerator ReceivedDamagedRoutine()
    {
        if (_lives > 0)
        {
            _collider2D.enabled = false;
            _animator.SetTrigger(_invincibleHash);
            yield return new WaitForSeconds(_invincibleLength);
            _collider2D.enabled = true;
        }
    }


    public void SpeedBoost()
    {
        StartCoroutine(SpeedBoostRoutine());
    }
    
    public void Shield()
    {
        _shield.SetActive(true);
        _shieldActive = true;
    }
}
