using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSceneManager : MonoBehaviour
{
    public bool isDebugConsoleOpened = false;
    public GameObject gameOverScreen;
    public GameObject playerMovement;
    public bool isPaused = false;
    public GameObject pauseScreen; 
    public VoidEventChannel onPlayerDeath;
    public VoidEventChannel onPause;
    public VoidEventChannel onResume;

    [Header("Listen to events")]
    public StringEventChannel onLevelEnded;
    public BoolEventChannel onDebugConsoleOpenEvent;

    private void Start()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void OnEnable()
    {
        onLevelEnded.OnEventRaised += LoadScene;
        onDebugConsoleOpenEvent.OnEventRaised += OnDebugConsoleOpen;
        onPlayerDeath.OnEventRaised += Die;
    }

    private void OnDisable()
    {
        onLevelEnded.OnEventRaised -= LoadScene;
        onDebugConsoleOpenEvent.OnEventRaised -= OnDebugConsoleOpen;
        onPlayerDeath.OnEventRaised -= Die;
    }

    public void LoadScene(string sceneName)
    {
        if (UtilsScene.DoesSceneExist(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log($"Unknown scene named {sceneName}. Please add the scene to the build settings.");
        }
    }

    public void LoadScene(int sceneIndex)
    {
        if (UtilsScene.DoesSceneExist(sceneIndex))
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.Log($"Unknown scene with index {sceneIndex}. Please add the scene to the build settings.");
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void RestartLastCheckpoint()
    {
        Debug.Log("RestartLastCheckpoint");
        // Refill life to full
        // Position to last checkpoint
        // Remove menu
        // Reset Rigidbody
        // Reactivate Player movements
        // Reset Player's rotation
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDebugConsoleOpen(bool debugConsoleOpened)
    {
        isDebugConsoleOpened = debugConsoleOpened;
    }

    private void Die() 
    {
        gameOverScreen.SetActive(true);
    }

    public void Pause() 
    {
        if (Time.timeScale == 0) 
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseScreen.SetActive(false);
            onResume.Raise();
        } 
        else 
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseScreen.SetActive(true);
            onPause.Raise();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Pause();
        }
#if UNITY_EDITOR 
        if(Input.GetKeyDown(KeyCode.R)) 
        {
            RestartLevel();
        }
#endif
    }
}
