using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSceneManage : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject playerMovement;
    public bool isPaused = false;
    public GameObject pauseScreen; 
    public VoidEventChannel onPlayerDeath;
    public VoidEventChannel onPause;
    public VoidEventChannel onResume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void OnEnable()
    {
        onPlayerDeath.OnEventRaised += Die;    
    }

    private void OnDisable()
    {
        onPlayerDeath.OnEventRaised -= Die;        
    }

    private void Die() {
        gameOverScreen.SetActive(true);
    }

    public void Pause() {
        if (Time.timeScale == 0) {
                Time.timeScale = 1;
                isPaused = false;
                pauseScreen.SetActive(false);
                onResume.Raise();
            } else {
                Time.timeScale = 0;
                isPaused = true;
                pauseScreen.SetActive(true);
                onPause.Raise();
            }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
#if UNITY_EDITOR 
        if(Input.GetKeyDown(KeyCode.R)) {
            RestartGame();
        }
#endif
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
