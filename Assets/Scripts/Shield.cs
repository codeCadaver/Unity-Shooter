using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private int _maxShieldHealth = 3;
    [SerializeField] private Material[] _shieldStrengMaterials;

    private int _currentShieldHealth;
    private Renderer _mesh;

    void OnAwake()
    {
        _mesh = GetComponent<Renderer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = _shieldStrengMaterials[0];
        // _mesh.material = _shieldStrengMaterials[0];
        _currentShieldHealth = _maxShieldHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLaser") || other.CompareTag("Enemy"))
        {
            DamageShield();
            Destroy(other.gameObject);
        }
    }

    private void DamageShield()
    {
        if (_currentShieldHealth > 0)
        {
            _currentShieldHealth--;
            GetComponent<MeshRenderer>().material = _shieldStrengMaterials[1];
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        FullShields();
    }

    public void FullShields()
    {
        _currentShieldHealth = _maxShieldHealth;
        GetComponent<MeshRenderer>().material = _shieldStrengMaterials[0];
    }

}
