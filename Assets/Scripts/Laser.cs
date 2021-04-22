using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private bool _isEnemyLaser = false;
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _maxY = 9f;
    [SerializeField] private LayerMask _ownerLayer, _friendLayer;

    private Rigidbody2D _rigidbody2D;
    private float _laserPositionY;

    
    // Start is called before the first frame update
    void Start()
    {
        _laserPositionY = transform.position.y;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        // Physics.IgnoreLayerCollision(_ownerLayer, _friendLayer);
        // Physics.IgnoreLayerCollision(_ownerLayer.value, _friendLayer.value);
        // Physics2D.IgnoreLayerCollision(1 << LayerMask.NameToLayer("Enemy"),1 << LayerMask.NameToLayer("Enemy"));
        EnemyCheck();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Destroy();
    }

    private void Movement()
    {
        transform.Translate(Vector3.up * (Time.deltaTime * _speed), Space.Self);
    }

    private void Destroy()
    {
        if(ScreenCheck())
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void EnemyCheck()
    {
        if (_isEnemyLaser)
        {
            _speed = -_speed;
        }
    }

    private bool ScreenCheck()
    {
        if (_isEnemyLaser)
        {
            return  transform.position.y < -_maxY ;
        }
        else
        {
            return transform.position.y > _maxY;
        }
    }
    
}
