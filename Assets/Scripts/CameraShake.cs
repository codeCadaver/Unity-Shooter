using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator _anim;
    private int shakeHash = Animator.StringToHash("Shake");
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        PlayerPrototype.OnPlayerDamage += ShakeCamera;
    }

    private void OnDisable()
    {
        PlayerPrototype.OnPlayerDamage -= ShakeCamera;
    }

    private void ShakeCamera()
    {
        _anim.SetTrigger(shakeHash);
    }
}
