using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.UI;


public class UIManager : MonoBehaviour
{
    public static Action<bool> OnGameOver;

    [SerializeField] private int _maxLives = 3;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _gameOverText, _restartText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;
    // [SerializeField] private Sprite[] _emptyLifeSprites;
    [SerializeField] private Sprite _lifeEmptySprite, _lifeFullSprite;
    [SerializeField] private Image _lifeEmptyImage, _lifeFullImage;
    [SerializeField] private Transform _lifeImageContainer, _lifeTransform;

    private bool _gameOver = false;
    private int _score = 0;
    private Sprite[] _emptyLifeSprites;
    // private List<Image> _emptyLifeImages = new List<Image>();
    // private List<Image> _fullLifeImages = new List<Image>();
    
    // Start is called before the first frame update
    void Start()
    {
        _emptyLifeSprites = new Sprite[_maxLives];
        _gameOver = false;
        _scoreText.text = "Score: 0";
        
        _restartText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        

        // UpdateLifeImages(_emptyLifeImages, maxLives);
        // UpdateLifeImages(_fullLifeImages, currentLives);
        
        SetEmptyLives();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDestroyed += UpdateScore;
    }

    private void UpdateScore(int value)
    {
        _score += value;
        _scoreText.text = $"Score: {_score}";
    }

    // public void UpdateMaxLivesImages(int lives)
    // {
    //     // var imagePosition = _lifeEmptyImage.transform.position;
    //     // for (int i = 0; i < maxLives - 1; i++)
    //     // {
    //     //     _emptyLifeImages.Add(Instantiate(_lifeEmptyImage, _lifeTransform.position, Quaternion.identity,
    //     //         _lifeImageContainer));
    //     //     imagePosition.x += _emptyLifeImages[i].sprite.rect.width/2.5f;
    //     //     _emptyLifeImages[i].transform.position = imagePosition;
    //     // }
    //
    //     UpdateLifeImages(_emptyLifeImages, lives);
    // }
    //
    // public void UpdateCurrentLives(int lives)
    // {
    //     // var imagePosition = _lifeFullImage.transform.position;
    //     // for (int i = 0; i < lives - 1; i++)
    //     // {
    //     //     _fullLifeImages.Add(Instantiate(_lifeFullImage, _lifeTransform.position, Quaternion.identity, _lifeImageContainer));
    //     //     imagePosition.x += _fullLifeImages[i].sprite.rect.width / 2.5f;
    //     //     _fullLifeImages[i].transform.position = imagePosition;
    //     // }
    //
    //     UpdateLifeImages(_fullLifeImages, lives);
    // }

    // private void UpdateLifeImages(List<Image> imageList, int lives)
    // {
    //     var imagePosition = _lifeEmptyImage.transform.position;
    //     for (int i = 0; i < lives - 1; i++)
    //     {
    //         imageList.Add(Instantiate(_lifeEmptyImage, _lifeTransform.position, Quaternion.identity,
    //             _lifeImageContainer));
    //         imagePosition.x += imageList[i].sprite.rect.width/2.5f;
    //         imageList[i].transform.position = imagePosition;
    //     }
    // }

    public void UpdateCurrentLivesImages(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
        Debug.Log($"Sprite: {_livesImage.sprite.name}");
        
        if (currentLives <= 0)
        {
            _gameOver = true;
            OnGameOver?.Invoke(_gameOver);
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            StartCoroutine(GameOverRoutine());
            
        }
    }

    private void SetEmptyLives()
    {
        var imagePosition = _lifeTransform.position;
        for (int i = 0; i < _emptyLifeSprites.Length; i++)
        {
            imagePosition.x += _lifeEmptySprite.rect.width / 2f;
            if (i == 0)
            {
                _emptyLifeSprites[i] = Instantiate(_lifeEmptySprite, _lifeTransform.position, Quaternion.identity, _lifeImageContainer);
            }
            else
            {
                _emptyLifeSprites[i] = Instantiate(_lifeEmptySprite, imagePosition, Quaternion.identity, _lifeImageContainer);
            }
            
        }
    }
    //
    // public void UpdateMaxLives(int maxLives)
    // {
    //     foreach (var image in _emptyLifeImages)
    //     {
    //         image.gameObject.SetActive(false);
    //     }
    //
    //     for (int i = 0; i < _emptyLifeImages.Count - 1; i++)
    //     {
    //         _emptyLifeImages[i].gameObject.SetActive(true);
    //     }
    // }

    IEnumerator GameOverRoutine()
    {
        while (_gameOver)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            _gameOverText.gameObject.SetActive(true);
        }
    }

}
