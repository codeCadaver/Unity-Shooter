using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _screenTopY, _screenBottomY;
    [SerializeField] private float _randomX;
    
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
        Vector3 newPosition = transform.position;
        // move down
        transform.Translate(Vector3.down *(Time.deltaTime * _speed));
        // if bottom of screen
        if (transform.position.y < _screenBottomY)
        {
            // move to top
            newPosition.y = _screenTopY;
            newPosition.x = Random.Range(-_randomX, _randomX);
            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrototype player = other.GetComponent<PlayerPrototype>();
            if (player != null)
            {
                player.DamagePlayer();
            }
            else
            {
                Debug.LogError("PlayerPrototype = null");
            }
            
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrototype player = other.GetComponent<PlayerPrototype>();
            if (player != null)
            {
                player.DamagePlayer();
            }
            else
            {
                Debug.LogError("PlayerPrototype = null");
            }
            
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        Debug.Log(other.name);
    }
}
