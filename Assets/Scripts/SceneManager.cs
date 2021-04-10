using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private float _alphaSpeed = 1.5f;

    private Color _backgroundColor;
    
    // Start is called before the first frame update
    void Start()
    {
        _backgroundColor = _mesh.material.color;
        _backgroundColor.a = 1;

        // StartCoroutine(BackgroundAlphaRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("TransparentBackground", 2f);
    }
    
    // cube moves around screen
    // after 2f
        // cube transforms into Unity Logo
            // shrink cube
            // play smoke effect
            // Unity Logo appears
            // Unity Logo shoots single laser cannon
        // Unity Logo transforms into player ship
            // shrink logo
            // play smoke effect
            // Player ship appears
            // Player ship barrel rolls
            // Player ship shoots triple shot green lasers
    private void TransparentBackground()
    {
        if (_backgroundColor.a > 0)
        {
            _backgroundColor.a -= Time.deltaTime / _alphaSpeed;
            _mesh.material.color = _backgroundColor;
        }
    }

    private IEnumerator BackgroundAlphaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            while (_backgroundColor.a > 0f)
            {
                TransparentBackground();
            }
        }
    }
}
