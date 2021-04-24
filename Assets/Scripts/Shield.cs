using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private int _maxShieldHealth = 3;
    [SerializeField] private Material[] _shieldStrengMaterials;
    [SerializeField] private float _fresnalStartPower = 12, _fresnalMaxMultiplier = 30, _fresnalMinMultiplier = 5, _fresnalMainPower = 1.5f;

    private int _currentShieldHealth;
    private MeshRenderer _mesh;

    void OnAwake()
    {
        _mesh = GetComponent<MeshRenderer>();
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
        StartCoroutine(ShieldsUpRoutine());
    }

    IEnumerator ShieldsUpRoutine()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        float power = _fresnalStartPower;
        SetFresnalPower("_fresnalPower", power, mesh);

        while (power > -0.5f)
        {
            yield return new WaitForEndOfFrame();
            power -= Time.deltaTime * _fresnalMaxMultiplier;
            SetFresnalPower("_fresnalPower", power, mesh);
        }

        while (power < 1.5f)
        {
            yield return new WaitForEndOfFrame();
            power += Time.deltaTime * _fresnalMinMultiplier;
            SetFresnalPower("_fresnalPower", power, mesh);
        }

        power = _fresnalMainPower;
        SetFresnalPower("_fresnalPower", power, mesh);
    }

    private void SetFresnalPower(string fresnal, float power, MeshRenderer mesh)
    {
        mesh.material.SetFloat(fresnal, power);
    }

}
