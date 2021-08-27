using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameMode _gameMode;

    [SerializeField]
    private GameObject _gameOverUI;

    [SerializeField]
    private Transform _hud;

    private bool _doOnce = true;
    private EventSystem _eventSystem;

    [SerializeField]
    private GameObject _resumeButton;

    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private float _fadeInSpeed = 2.0f;

    [SerializeField] private float _menuDelay = 3.0f;
    private bool _menuShown = false;

    private void Awake()
    {
        _gameOverUI.SetActive(false);
        _canvasGroup.alpha = 0;
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    private void Update()
    {
        // If the player is dead, show the gameover screen.
        if (_gameMode.GetIsGameOver() && _doOnce)
        {
            Invoke("ShowMenu", _menuDelay);
            _doOnce = false;
        }

        if (_canvasGroup.alpha < 1f && _menuShown)
        {
            _canvasGroup.alpha += _fadeInSpeed * Time.unscaledDeltaTime;
        }
        if (_eventSystem.currentSelectedGameObject == null && _gameOverUI.activeSelf)
        {
            _eventSystem.SetSelectedGameObject(_resumeButton);
        }
    }

    private void ShowMenu()
    {
        _hud.gameObject.SetActive(false);
        Time.timeScale = 0.001f;
        _gameOverUI.SetActive(true);
        _eventSystem.SetSelectedGameObject(_resumeButton);
        _menuShown = true;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        _hud.gameObject.SetActive(true);
        _gameOverUI.SetActive(false);
        _gameMode.RestartWave();
        _doOnce = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}