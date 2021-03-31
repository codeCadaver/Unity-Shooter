using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private float dissolveSpeed = 1.5f;

    [SerializeField]
    private WaitForSeconds dissolveDelay;

    private float _time = 0;
    private float _timeReverse = 0;

    private bool _canDissolve = false;
    private bool _canReverse = false;
    
    // public float _dissolveAmount;
    private MeshRenderer _renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        dissolveDelay = new WaitForSeconds(1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        DissolveCube();
        DissolveReverse();
    }

    private void DissolveCube()
    {
        if (_canDissolve)
        // if (test == true)
        {
            _time += Time.deltaTime;
            _renderer.material.SetFloat("_CutoffHeight", Mathf.Lerp(42, -12, _time * dissolveSpeed));
        }
    }
    //
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dissolver"))
        {
            _canDissolve = true;
            _canReverse = false;
        }

        if (other.CompareTag("NotDissolver"))
        {
            _canReverse = true;
            _canDissolve = false;
        }
    }

    void DissolveReverse()
    {
        if (_canReverse)
        {
            _timeReverse += Time.deltaTime;
            _renderer.material.SetFloat("_CutoffHeight", Mathf.Lerp(-12, 42, _timeReverse * dissolveSpeed));
        }
    }
}
