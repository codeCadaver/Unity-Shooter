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
    public static Action<bool> OnStartGame;

    [SerializeField] private int _maxLives = 3;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _gameOverText, _restartText, _startGameText;
    [SerializeField] private TMP_Text _noAmmoText;
    [SerializeField] private float _noAmmoTextDisplayTime = 2f;
    [SerializeField] private Image _thrusterImage;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Sprite _lifeEmptySprite, _lifeFullSprite;
    [SerializeField] private Image _lifeEmptyImage, _lifeFullImage;
    [SerializeField] private Transform _lifeImageContainer, _lifeTransform;
    [SerializeField] private GameObject _ammoUI;
    [SerializeField] private Transform _ammoUIStart, _ammoUIContainer;
    [SerializeField] private float _ammoUI_HOffset, _ammoUI_VOffset;
    [SerializeField] private int _maxAmmo = 15;

    private bool _gameOver = false;
    private bool _gameStarted = false;
    public GameObject[] _ammoImages;
    private int _score = 0;
    private Sprite[] _emptyLifeSprites;
    
    // Start is called before the first frame update
    void Start()
    {
        _ammoImages = new GameObject[_maxAmmo];
        _emptyLifeSprites = new Sprite[_maxLives];
        _gameOver = false;
        _scoreText.text = "Score: 0";
        
        _restartText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _startGameText.gameObject.SetActive(true);
        
        SetEmptyLives();
        SetAmmoUI(_maxAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        StartGame();
    }


    private void UpdateScore(int value)
    {
        _score += value;
    }

    public void UpdateCurrentLivesImages(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
        
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

    private void StartGame()
    {
        if (!_gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gameStarted = true;
                _startGameText.gameObject.SetActive(false);
                OnStartGame?.Invoke(_gameStarted);
            }
        }
    }

    public void OutOfAmmo()
    {
        StartCoroutine(NoAmmoRoutine());
    }

    IEnumerator NoAmmoRoutine()
    {
        _noAmmoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(_noAmmoTextDisplayTime);
        _noAmmoText.gameObject.SetActive(false);
    }

    public void ThrusterAmount(float amount)
    {
        _thrusterImage.fillAmount = amount;
    }

    private void SetAmmoUI(int amount)
    {
        int total = 0;
        int horizCount = 0;
        int vertCount = 0;
        
        foreach (var image in _ammoImages)
        {
            // set position offset
            Vector3 newPos = _ammoUIStart.position;
            newPos.x += horizCount * _ammoUI_HOffset;
            newPos.y += vertCount * _ammoUI_VOffset;
            GameObject newImage = Instantiate(_ammoUI, newPos, Quaternion.identity, _ammoUIContainer);
            _ammoImages[total] = newImage;
            total++;
            horizCount++;
            if (horizCount > 4)
            {
                horizCount = 0;
                vertCount--;
            }
        }
    }

    private void SubtractAmmo(float delay)
    {
        float currentTime = -1f;
        {
            // if (Time.time > currentTime + delay)
            {
                for (int i = _ammoImages.Length - 1; i >= 0; i--)
                {
                    if (!_ammoImages[i].activeSelf)
                    {
                        continue;
                    }
                    else
                    {
                        _ammoImages[i].GetComponent<AmmoUI>().Disappear();
                        currentTime = Time.time;
                        break;
                    }
                }
                
            }
        }
    }
     
    private void OnEnable()
    {
        Enemy.OnEnemyDestroyed += UpdateScore;
        PlayerPrototype.OnPlayerFired += SubtractAmmo;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDestroyed -= UpdateScore;
        PlayerPrototype.OnPlayerFired -= SubtractAmmo;
    }   
        
    public int GetMaxAmmo()
    {
        return _maxAmmo;
    }

    public void RefillAmmo()
    {
        foreach (var ammoImage in _ammoImages)
        {
            ammoImage.SetActive(true);
        }
    }
}
