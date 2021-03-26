using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public static Action<Vector2> OnPlayerMoved;
    
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private GameObject player;
    [SerializeField] private Material[] playerMaterials;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float left = Input.GetAxis("Horizontal");
        float up = Input.GetAxis("Vertical");

        var movement = new Vector2(left, up);
        
        OnPlayerMoved?.Invoke(movement);
        
        transform.Translate(movement * (Time.deltaTime * movementSpeed));
    }

}
