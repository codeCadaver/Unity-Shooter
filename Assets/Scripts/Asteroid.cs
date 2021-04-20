using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speed = 5f, _rotationSpeed = 15f;
    [SerializeField] private GameObject _explosion;

    private Animator _animator;
    private Collider2D _collider2D;
    private int _explosionHash = Animator.StringToHash("ExplosionTrigger");
    private PlayerPrototype _player;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerPrototype>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.Translate(Vector2.down * (Time.deltaTime * _speed), Space.World);
        transform.Rotate(Vector3.forward * (_rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            _animator.SetTrigger(_explosionHash);
            _collider2D.enabled = false;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            _animator.SetTrigger(_explosionHash);
            _collider2D.enabled = false;
            _player.DamagePlayer();
        }
    }

    private void DestroyAsteroid()
    {
        Destroy(this.gameObject);
    }
}
