#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [Header("Pause UI")]
    public GameObject pauseMenu;       // assign the pause menu panel (contains pause buttons)
    public GameObject pauseOverlay;    // assign a full-screen Image/Panel that blocks raycasts (semi-transparent)

    [Header("Optional")]
    public SceneChanger sceneChanger;  // optional: use existing SceneChanger to go to menu with fade

    bool isPaused = false;

    public static GameManager instance;
    public GameObject victoryScreen; // Assign in inspector
    public GameObject victory;
    public TextMeshProUGUI winnerNameText;
    // public SpriteRenderer winnerSpriteRenderer; // Remove or comment out
    public Image winnerImage; // Assign this in the inspector


    void Awake()
    {
        instance = this;
    }
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
    public void ShowVictoryScreen(PlayerMovementScript winner)
    {
        victoryScreen.SetActive(true);
        victory.SetActive(true);
        // Set winner name
        winnerNameText.text = winner.GetComponent<NameScript>().playerName;

        // Set winner sprite for UI Image
        SpriteRenderer playerSprite = winner.GetComponentInChildren<SpriteRenderer>();
        if (playerSprite != null && winnerImage != null)
            winnerImage.sprite = playerSprite.sprite;
        Debug.Log("Winner sprite: " + (playerSprite != null ? playerSprite.sprite.name : "null"));

        // Save leaderboard data when the game ends
        SaveLeaderboard();
    }
    public void SaveLeaderboard()
    {
        var players = GameTurnManager.instance.GetAllPlayers();
        var data = new SaveLoadScript.LeaderboardData();
        data.entries = players.Select(p => new SaveLoadScript.LeaderboardEntry
        {
            playerName = p.GetComponent<NameScript>().playerName,
            timePlayed = p.timePlayed,
            diceSum = p.totalDiceSum,
            hasFinished = p.hasFinished
        }).ToArray();

        string json = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/leaderboard.json", json);
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
    public void ResetGame()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

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
