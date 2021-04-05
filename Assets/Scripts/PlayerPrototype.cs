using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerPrototype : MonoBehaviour
{

    public static Action<Vector2> OnPlayerMoved;
    
    [SerializeField] private bool _canWrap = false;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _tilt = 20f;
    [SerializeField] private float _xWrap,_xClamp, _minY, _maxY;
    [SerializeField] private float _cooldownDelay = 0.5f;
    [SerializeField] private GameObject _laser;
    [SerializeField] private int _lives = 3;
    [SerializeField] private Transform _laserOffset;

    private bool _canFire = true;
    private float _lastFired = 0f;
    private Vector3 _newPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float right = Input.GetAxis("Horizontal");
        float up = Input.GetAxis("Vertical");

        var movement = new Vector2(right, up);
        
        OnPlayerMoved?.Invoke(movement);
        
        transform.parent.Translate(movement * (Time.deltaTime * _movementSpeed));
        transform.rotation *= Quaternion.AngleAxis(_tilt * right, new Vector3(0, 1 * right, 0));
        _newPosition = transform.parent.position;
        
        ClampVert();
        ClampHoriz();
        Fire();
    }

    private void Fire()
    {
        if (_canFire)
        {
            if (Input.GetKey(KeyCode.Space) && Time.time > _lastFired)
            {
                var laser = Instantiate(_laser, _laserOffset.position, Quaternion.identity);
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
        if (transform.parent.position.y > _maxY)
        {
            _newPosition.y = _maxY;
        }

        if (transform.parent.position.y < _minY)
        {
            _newPosition.y = _minY;
        } 
        
        transform.parent.position = _newPosition;
    }

    private void ClampHoriz()
    {
        if (_canWrap)
        {
            if (transform.position.x > _xWrap)
            {
                _newPosition.x = -_xWrap;
            }

            if (transform.position.x < -_xWrap)
            {
                _newPosition.x = _xWrap;
            }
        }
        else
        {
            if (transform.position.x > _xClamp)
            {
                _newPosition.x = _xClamp;
            }

            if (transform.position.x < -_xClamp)
            {
                _newPosition.x = -_xClamp;
            }
        }

        transform.position = _newPosition;
    }

    private void OnEnable()
    {
        ExplodeUnity.OnExploded += DisableLaser;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with: {other.name}");
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

}
