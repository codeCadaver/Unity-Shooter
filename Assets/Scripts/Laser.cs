using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _maxY = 9f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Destroy();
    }

    private void Movement()
    {
        transform.Translate(Vector3.up * (Time.deltaTime * _speed));
    }

    private void Destroy()
    {
        if (transform.position.y > _maxY)
        {
            Destroy(this.gameObject);
        }
    }
}
