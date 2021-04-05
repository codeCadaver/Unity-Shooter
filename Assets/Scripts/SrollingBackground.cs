using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SrollingBackground : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = -0.1f;

    private MeshRenderer _renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ScrollBackground();   
    }

    private void ScrollBackground()
    {
        _renderer.material.mainTextureOffset = new Vector2(transform.position.x, Time.time * _scrollSpeed);
    }
}
