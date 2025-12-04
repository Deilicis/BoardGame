#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Pause UI")]
    public GameObject pauseMenu;       // assign the pause menu panel (contains pause buttons)
    public GameObject pauseOverlay;    // assign a full-screen Image/Panel that blocks raycasts (semi-transparent)

    [Header("Optional")]
    public SceneChanger sceneChanger;  // optional: use existing SceneChanger to go to menu with fade

    bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure game starts unpaused and overlay hidden
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        // Optional: toggle pause with Escape
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
#else
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
#endif
    }

    public void TogglePause()
    {
        if (isPaused) ResumeGame(); else PauseGame();
    }

    public void PauseGame()
    {
        if (pauseOverlay != null) pauseOverlay.SetActive(true);
        if (pauseMenu != null) pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (pauseOverlay != null) pauseOverlay.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ClosePauseMenu()
    {
        ResumeGame();
    }

    public void QuitToMenu()
    {
        // Make sure timeScale is reset before changing scenes
        Time.timeScale = 1f;
        if (sceneChanger != null)
        {
            sceneChanger.GoToMenu();
        }
        else
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
