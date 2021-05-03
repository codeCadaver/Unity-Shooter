using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField] private bool _isHoming = true;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private GameObject _explostion;

    private List<GameObject> _enemyList = new List<GameObject>();
    private GameObject[] _enemies = new GameObject[5];
    private Transform _target;
    
    // Start is called before the first frame update
    void Start()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        StartCoroutine(DestructRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Homing();
    }

    private void MoveStraight()
    {
        transform.Translate(Vector3.up * (_speed * Time.deltaTime), Space.Self);
    }

    private void Homing()
    {
        if (_isHoming)
        {
            if (_enemies != null)
            {
                for (int enemy = 0; enemy < _enemies.Length; enemy++)
                {
                    if (_enemies[enemy] != null)
                    {
                        if (_enemies[enemy].GetComponent<Enemy>().wasTarget == false)
                        {
                            _target = _enemies[enemy].transform;
                            _target.GetComponent<Enemy>().wasTarget = true;
                        }
                    }

                    if (_target == null)
                    {
                        _isHoming = false;
                        MoveStraight();
                    }
                }
                RotateTowardsEnemy();
            }
        }
        else
        {
            MoveStraight();
        }
    }

    private void RotateTowardsEnemy()
    {
        if (_target != null)
        {
            float angle = Mathf.Atan2(_target.position.y, _target.position.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle - 90)), _speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }
        else
        {
            MoveStraight();
        }
    }

    private void SelfDestruct()
    {
        Instantiate(_explostion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    IEnumerator DestructRoutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        SelfDestruct();
    }
}
