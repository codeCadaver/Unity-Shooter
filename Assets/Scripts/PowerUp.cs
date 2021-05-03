using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PowerUp : MonoBehaviour
{
    private enum PowerUpType
    {
        TripleShot = 0,
        SpeedBoost = 1,
        Shield = 2,
        Ammo = 3,
        Health = 4,
        InvertedControls = 5,
        HomingMissile = 6
    };

    [SerializeField] private PowerUpType _type;
    
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _speedMultiplier = 3f;
    [SerializeField] private float _bottomScreenPosY = -4f;
    [SerializeField] private string _playerString;
    [SerializeField] private AudioClip _powerupClip;
    [SerializeField] private GameObject _explosion;

    private Animator _animator;
    private AudioSource _audioSource;
    private int collectedHash = Animator.StringToHash("Collected");
    private PlayerPrototype _player;
    private GameObject player;
    private bool _chasePlayer = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player = GameObject.Find(_playerString).GetComponent<PlayerPrototype>();
            if (_player != null)
            {
                switch (_type)
                {
                    case PowerUpType.TripleShot:
                        _player.TripleShot();
                        _player.AmmoReload();
                        break;
                    case PowerUpType.SpeedBoost:
                        _player.SpeedBoost();
                        break;
                    case PowerUpType.Shield:
                        _player.Shield();
                        // get shield
                        Shield shield = FindObjectOfType<Shield>();
                        shield.FullShields();
                        break;
                    case PowerUpType.Ammo:
                        _player.AmmoReload();
                        break;
                    case PowerUpType.Health:
                        _player.HealthRestore();
                        break;
                    case PowerUpType.InvertedControls:
                        _player.InvertControls();
                        break;
                    case PowerUpType.HomingMissile:
                        _player.HomingMissile();
                        break;
                        
                    default:
                        break;
                }
                _animator.SetTrigger(collectedHash);
                AudioSource.PlayClipAtPoint(_powerupClip, Camera.main.transform.position);
            }
        }

        if (other.CompareTag("EnemyLaser"))
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void Movement()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));
        if (transform.position.y < _bottomScreenPosY)
        {
            DestroyPowerup();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _chasePlayer = true;
            
            // transform.position =
                // Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * _speed * _speedMultiplier);
        }

        if (_chasePlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position,
                Time.deltaTime * _speed * _speedMultiplier);
        }
    }

    public void DestroyPowerup()
    {
        Destroy(this.gameObject);
    }

    private void PlayPickupSound()
    {
        
    }
}
