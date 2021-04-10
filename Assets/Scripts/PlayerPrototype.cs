using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

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
    [SerializeField] private GameObject _laser, _tripleShot;
    [SerializeField] private int _lives = 3;
    [SerializeField] private Transform _laserOffset;

    private Animator _animator;
    private bool _canBarrelRoll = true;
    private bool _canFire = true;
    private Vector3 _playerRotation;
    private float _lastFired = 0f;
    private Vector3 _newPosition;
    private float _right, _up;
    private GameObject _projectile;

    public bool _tripleShotActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        // StartCoroutine(RollRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        // DoABarrelRoll();
        // Debug.Log(_right);
        DoABarrelRoll(_right);
        GetPlayerRotation();
    }

    private void Movement()
    {
        _right = Input.GetAxis("Horizontal");
        _up = Input.GetAxis("Vertical");

        var movement = new Vector2(_right, _up);
        
        OnPlayerMoved?.Invoke(movement);
        
        _playerTransform.Translate(movement * (Time.deltaTime * _movementSpeed));
        // transform.rotation *= Quaternion.AngleAxis(_tilt * right, new Vector3(0, 1 * right, 0));
        _newPosition = _playerTransform.position;
        
        //BarrelRoll()
        
        
        ClampVert();
        ClampHoriz();
        Fire();
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
                    // laser.transform.rotation = Quaternion.Euler(Vector3.up);
                }
                else
                {
                    var laser = Instantiate(_projectile, _laserOffset.position, Quaternion.identity);
                }
                // set delay
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with: {other.name}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    public void DamagePlayer()
    {
        _lives--;

        if (_lives <= 0)
        {
            SpawnManager spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            spawnManager.StopSpawning();
            
            Destroy(this.gameObject);
        }
    }

    private void DoABarrelRoll(float direction)
    {
        // if ctrl key is pressed
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // transform.Rotate(Vector3.up * (Input.GetAxis("Horizontal") * _rollSpeed * Time.time));
            // _anim.SetFloat("Direction", direction);
            if (_right < -.1)
            {
                _animator.SetTrigger("RollLeft");
            }

            if (_right > .1f)
            {
                _animator.SetTrigger("RollRight");
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
        _animator.SetTrigger("RollLeft");
        yield return new WaitForSeconds(1f);
        _animator.SetTrigger("RollRight");
    }


}
