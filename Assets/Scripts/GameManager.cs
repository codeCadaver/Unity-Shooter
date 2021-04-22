using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _gameOver = false;

    // Update is called once per frame
    void Update()
    {
        RestartGame();
    }

    private void GameOver(bool gameOver)
    {
        _gameOver = gameOver;
    }

    private void RestartGame()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_gameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnEnable()
    {
        UIManager.OnGameOver += GameOver;
    }

    private void QuitApplication()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(0);
        }
    }

}
