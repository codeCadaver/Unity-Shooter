using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var left = Input.GetAxis("Horizontal");
        var up = Input.GetAxis("Vertical");
        
        transform.Translate(new Vector2(left, up) * (Time.deltaTime * speed));
    }
}
