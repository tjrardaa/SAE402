using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
void Start()
{
    gameOverUI.SetActive(false); // Désactive l'écran Game Over au début du jeu
}

    public GameObject gameOverUI;

    public void ShowGameOverScreen()
    {
        gameOverUI.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Remplace "MainMenu" par le nom exact de ta scène de menu
    }
}