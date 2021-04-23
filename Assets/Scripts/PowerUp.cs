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
        Shield = 2
    };

    [SerializeField] private PowerUpType _type;
    
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _bottomScreenPosY = -4f;
    [SerializeField] private string _playerString;
    [SerializeField] private AudioClip _powerupClip;

    private Animator _animator;
    private AudioSource _audioSource;
    private int collectedHash = Animator.StringToHash("Collected");
    private PlayerPrototype _player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
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
                    default:
                        break;
                }
                _animator.SetTrigger(collectedHash);
                // play sound
                // _audioSource.PlayOneShot(_powerupClip);
                AudioSource.PlayClipAtPoint(_powerupClip, Camera.main.transform.position);
            }
        }
    }

    private void Movement()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));
        if (transform.position.y < _bottomScreenPosY)
        {
            DestroyPowerup();
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
