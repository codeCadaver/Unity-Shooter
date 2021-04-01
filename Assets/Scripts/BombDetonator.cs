using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDetonator : MonoBehaviour
{
    [SerializeField] private float _wickDelay = 1f, _wickSpeed = 2f;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _bombPieces;
    [SerializeField] private GameObject _wick;
    [SerializeField] private GameObject _sparks;
    [SerializeField] private GameObject _flames;
    [SerializeField] private GameObject _smoke;
    [SerializeField] private GameObject _explosion;

    private bool _canMove = false;
    
    
    private WaitForSeconds _routineDelay;
    // Start is called before the first frame update
    void Start()
    {
        _routineDelay = new WaitForSeconds(_wickDelay);
        StartCoroutine(MoveWickRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        MoveWick();
    }
    
    // start timer delay
        // move wick and particles
        // if wick triggers explosion
            // disable bomb renderer
            // disable smoke, sparks, flame
            // enable bomb pices
            // enable explosion FX

    private IEnumerator MoveWickRoutine()
    {
        yield return _routineDelay;
        _canMove = true;
    }

    private void MoveWick()
    {
        if (_canMove)
        {
            _wick.transform.Translate(Vector3.forward * (Time.deltaTime * -_wickSpeed));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wick"))
        {
            _bomb.GetComponent<MeshRenderer>().enabled = false;
            _smoke.SetActive(false);
            _sparks.SetActive(false);
            _flames.SetActive(false);
            _bombPieces.SetActive(true);
            _explosion.SetActive(true);
            _wick.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
