using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameOverText;

    private bool _gameOver = false;
    
    private void Start()
    {
        _gameOver = true;
        // _gameOverText.gameObject.SetActive(false);
        PlayerDeath();
    }

    private void PlayerDeath()
    {
        StartCoroutine(OnGameOver());
    }

    private IEnumerator OnGameOver()
    {
        while (_gameOver)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(1f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
