using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplodeUnity : MonoBehaviour
{
    public static Action OnExploded;
    
    [SerializeField] private float _explodeDelay = 2f;
    [SerializeField] private GameObject _UnityPieces;
    

    private MeshRenderer _renderer;
    private WaitForSeconds _waitExplode;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _waitExplode = new WaitForSeconds(_explodeDelay);

        StartCoroutine(ExplodeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.materials[1].color = Color.Lerp(Color.white, Color.red, Time.time/(_explodeDelay + 0.5f));
    }

    private IEnumerator ExplodeRoutine()
    {
        yield return _waitExplode;
        OnExploded?.Invoke();
        _renderer.enabled = false;
        _UnityPieces.SetActive(true);
    }
}
