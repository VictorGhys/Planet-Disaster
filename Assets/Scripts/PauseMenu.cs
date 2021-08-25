using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused { get; set; } = false;

    [SerializeField]
    private GameObject _pauseMenuUI;

    [SerializeField]
    private GameObject _resumeButton;

    private EventSystem _eventSystem;

    private void Awake()
    {
        _pauseMenuUI.SetActive(false);
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }
        if (_eventSystem.currentSelectedGameObject == null && _pauseMenuUI.activeSelf)
        {
            _eventSystem.SetSelectedGameObject(_resumeButton);
        }
    }

    private void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0001f;
        GameIsPaused = true;
        _eventSystem.SetSelectedGameObject(_resumeButton);
    }

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        _eventSystem.SetSelectedGameObject(null);
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