using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;

    // Start is called before the first frame update
    void Start()
    {
        _newGameButton.onClick.AddListener(NewGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NewGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
