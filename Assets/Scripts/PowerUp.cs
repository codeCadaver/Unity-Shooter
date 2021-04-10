using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _bottomScreenPosY = -4f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
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
            Debug.Log("Collected Powerup");
            Destroy(this.gameObject);
        }
    }

    private void Movement()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));
        if (transform.position.y < _bottomScreenPosY)
        {
            Destroy(this.gameObject);
        }
    }
}
