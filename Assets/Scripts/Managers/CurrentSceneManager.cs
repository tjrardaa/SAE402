using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSceneManager : MonoBehaviour
{
    // Variables pour activer/désactiver le débogage en console
    public bool isDebugConsoleOpened = false;

    // Référence à l'écran de Game Over
    public GameObject gameOverScreen;

    // Référence au GameObject du joueur (peut être utilisé pour réactiver les mouvements)
    public GameObject playerMovement;

    // Booléen indiquant si le jeu est en pause ou non
    public bool isPaused = false;

    // Référence à l'écran de pause
    public GameObject pauseScreen;

    // Événement appelé lorsque le joueur meurt
    public VoidEventChannel onPlayerDeath;

    // Événement appelé lorsque le jeu est mis en pause
    public VoidEventChannel onPause;

    // Événement appelé lorsque le jeu est repris
    public VoidEventChannel onResume;

    [Header("Listen to events")]
    // Événement appelé lorsqu'un niveau est terminé (peut être utilisé pour charger le niveau suivant)
    public StringEventChannel onLevelEnded;

    // Événement appelé lorsque la console de débogage est ouverte ou fermée
    public BoolEventChannel onDebugConsoleOpenEvent;

    // Nom de la scène du menu principal
    private string mainMenuSceneName = "MainMenu"; // Assurez-vous que cette scène existe dans vos Build Settings

    // Nom de la première scène de jeu
    private string firstLevelSceneName = "Level1"; // Assurez-vous que cette scène existe dans vos Build Settings

    private void Start()
    {
        // Définir le framerate cible pour une expérience de jeu plus stable
        Application.targetFrameRate = 60;

        // S'assurer que le temps est à l'échelle normale au démarrage
        Time.timeScale = 1f;

        // Désactiver l'écran de Game Over au démarrage
        gameOverScreen.SetActive(false);

        // Désactiver l'écran de pause au démarrage
        pauseScreen.SetActive(false);
    }

    private void OnEnable()
    {
        // S'abonner aux événements lorsque le script est activé
        onLevelEnded.OnEventRaised += LoadScene;
        onDebugConsoleOpenEvent.OnEventRaised += OnDebugConsoleOpen;
        onPlayerDeath.OnEventRaised += Die;
    }

    private void OnDisable()
    {
        // Se désabonner des événements lorsque le script est désactivé pour éviter les erreurs
        onLevelEnded.OnEventRaised -= LoadScene;
        onDebugConsoleOpenEvent.OnEventRaised -= OnDebugConsoleOpen;
        onPlayerDeath.OnEventRaised -= Die;
    }

    // Charge une scène de jeu à partir de son nom
    public void LoadScene(string sceneName)
    {
        // Vérifier si la scène existe avant de la charger
        if (UtilsScene.DoesSceneExist(sceneName))
        {
            // Charger la scène
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // Afficher un message d'erreur si la scène n'existe pas
            Debug.Log($"Unknown scene named {sceneName}. Please add the scene to the build settings.");
        }
    }

    // Charge une scène de jeu à partir de son index dans les Build Settings
    public void LoadScene(int sceneIndex)
    {
        // Vérifier si la scène existe avant de la charger
        if (UtilsScene.DoesSceneExist(sceneIndex))
        {
            // Charger la scène
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            // Afficher un message d'erreur si la scène n'existe pas
            Debug.Log($"Unknown scene with index {sceneIndex}. Please add the scene to the build settings.");
        }
    }

    // Recommence le niveau actuel
    public void RestartLevel()
    {
        // Réinitialiser le temps
        Time.timeScale = 1f;

        // Charger à nouveau la scène actuelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Fonction statique pour redémarrer au dernier checkpoint (à implémenter)
    public static void RestartLastCheckpoint()
    {
        Debug.Log("RestartLastCheckpoint");
        // TODO: Implémenter la logique pour redémarrer au dernier checkpoint
        // - Restaurer la vie du joueur
        // - Replacer le joueur à la position du dernier checkpoint
        // - Enlever tout menu affiché
        // - Réinitialiser le Rigidbody du joueur
        // - Réactiver les mouvements du joueur
        // - Réinitialiser la rotation du joueur
    }

    // Quitte le jeu
    public static void QuitGame()
    {
#if UNITY_EDITOR
        // Si on est dans l'éditeur, arrête le mode Play
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Sinon, quitte l'application (fonctionne uniquement dans la version build du jeu)
        Application.Quit();
#endif
    }

    // Méthode appelée lorsque la console de débogage est ouverte ou fermée
    private void OnDebugConsoleOpen(bool debugConsoleOpened)
    {
        isDebugConsoleOpened = debugConsoleOpened;
    }

    // Méthode appelée lorsque le joueur meurt
    private void Die()
    {
        gameOverScreen.SetActive(true);
    }

    // Met le jeu en pause ou le reprend
   public void Pause()
{
    if (pauseScreen == null)
    {
        Debug.LogError("Pause Screen is not assigned in the inspector!");
        return;
    }

    if (Time.timeScale == 0)
    {
        Time.timeScale = 1;
        isPaused = false;
        pauseScreen.SetActive(false);
        onResume?.Raise();
    }
    else
    {
        Time.timeScale = 0;
        isPaused = true;
        pauseScreen.SetActive(true);
        onPause?.Raise();
    }
}



    // Démarre une nouvelle partie en chargeant la première scène de jeu
    public void StartGame()
    {
        // Vérifier si la scène de jeu existe avant de la charger
        if (UtilsScene.DoesSceneExist(firstLevelSceneName))
        {
            // Réinitialiser le temps au cas où le jeu était en pause
            Time.timeScale = 1f;

            // Charger la première scène de jeu
            SceneManager.LoadScene(firstLevelSceneName);
        }
        else
        {
            // Afficher un message d'erreur si la scène de jeu n'existe pas
            Debug.LogError($"La scène {firstLevelSceneName} n'existe pas. Veuillez l'ajouter dans les Build Settings.");
        }
    }

    // Retourne au menu principal
    public void ReturnToMainMenu()
    {
        // Vérifier si la scène du menu principal existe avant de la charger
        if (UtilsScene.DoesSceneExist(mainMenuSceneName))
        {
            // Réinitialiser le temps au cas où le jeu était en pause
            Time.timeScale = 1f;

            // Charger la scène du menu principal
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            // Afficher un message d'erreur si la scène du menu principal n'existe pas
            Debug.LogError($"La scène {mainMenuSceneName} n'existe pas. Veuillez l'ajouter dans les Build Settings.");
        }
    }

    private void Update()
    {
        // Mettre le jeu en pause ou le reprendre avec la touche Echap
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

#if UNITY_EDITOR
        // Redémarrer le niveau actuel avec la touche R (uniquement dans l'éditeur)
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
#endif
    }
}
