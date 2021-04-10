using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _speed = 5f;
    private CharacterController _controller;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var direction = _controller.velocity;
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        float gravity = -30f;
        float jumpHeight = 2f;

        _controller.Move(Vector3.right * (horiz * Time.deltaTime * _speed));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            
        }
            direction.y += (gravity * Time.deltaTime);
        _controller.Move(direction * Time.deltaTime);
    }
}
