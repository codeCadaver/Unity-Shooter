using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCubeFX : MonoBehaviour
{
    [SerializeField] private MeshRenderer _backgroundRenderer;
    [SerializeField] private float _backgroundAlphaSpeed = 1.5f;
    [SerializeField] private float _shrinkDelay = 1.5f;
    [SerializeField] private GameObject _fx;

    private Animator _animator;
    private Color _backgroundColor;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _backgroundColor = _backgroundRenderer.material.color;
        _backgroundColor.a = 0;
        
        StartCoroutine(ShrinkRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        ShowBackground();
    }
    
    // re-enable background
    
 

    private void ShowBackground()
    {
        if (_backgroundColor.a < 1)
        {
            _backgroundColor.a += Time.deltaTime / _backgroundAlphaSpeed;
            _backgroundRenderer.material.color = _backgroundColor;
        }
    }
    // start particleFX
    private IEnumerator ShrinkRoutine()
    {
        yield return new WaitForSeconds(_shrinkDelay);
        _animator.SetTrigger("ShrinkTrigger");
        _fx.SetActive(true);
    }


}
