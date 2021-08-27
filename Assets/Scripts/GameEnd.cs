using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private GameMode _gameMode;

    [SerializeField] private GameObject _gameEndUI;

    [SerializeField] private Transform _hud;

    private bool _doOnce = true;
    private EventSystem _eventSystem;

    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private float _fadeInSpeed = 2.0f;

    [SerializeField] private float _menuDelay = 3.0f;
    private bool _menuShown = false;

    private void Awake()
    {
        _gameEndUI.SetActive(false);
        _canvasGroup.alpha = 0;
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    private void Update()
    {
        // If the player is dead, show the gameover screen.
        if (_gameMode.GetHasGameEnded() && _doOnce)
        {
            Invoke("ShowMenu", _menuDelay);
            _doOnce = false;
        }

        if (_canvasGroup.alpha < 1f && _menuShown)
        {
            _canvasGroup.alpha += _fadeInSpeed * Time.unscaledDeltaTime;
        }
    }

    private void ShowMenu()
    {
        _hud.gameObject.SetActive(false);
        Time.timeScale = 0.001f;
        _gameEndUI.SetActive(true);
        _menuShown = true;
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