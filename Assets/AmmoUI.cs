using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    private Animator _anim;

    private int enableHash = Animator.StringToHash("Grow");

    private int disableHash = Animator.StringToHash("Shrink");
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disappear()
    {
        StartCoroutine(DisappearRoutine());
    }

    IEnumerator DisappearRoutine()
    {
        _anim.SetTrigger(disableHash);
        yield return new WaitForSeconds(0.15f);
        this.gameObject.SetActive(false);
    }
}
