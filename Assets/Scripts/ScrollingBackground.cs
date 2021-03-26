using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 5f;
    private MeshRenderer _renderer;
    private Vector2 _offset;
    private Vector2 _playerOffset;
    
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
        _offset = new Vector2(Time.time * _playerOffset.x, Time.time * scrollSpeed);
        _renderer.material.mainTextureOffset = _offset;
    }

    private void OnEnable()
    {
        // PlayerMovement.OnPlayerMoved += PlayerDirection;
    }

    private void PlayerDirection(Vector2 direction)
    {
        _playerOffset.x = direction.x / 200f;
    }
}
