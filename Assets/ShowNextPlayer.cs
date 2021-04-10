using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNextPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _nextPlayer;
    [SerializeField] private float _shrinkDelay = 1.5f;

    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(OnShrinkRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShowNextPlayer()
    {
        _nextPlayer.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private IEnumerator OnShrinkRoutine()
    {
        yield return new WaitForSeconds(_shrinkDelay);
        _animator.SetTrigger("ShrinkTrigger");
    }
}
