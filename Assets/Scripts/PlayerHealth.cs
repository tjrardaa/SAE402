using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    public Animator animator;
    public SpriteRenderer sr;
    public PlayerInvulnerable playerInvulnerable;

    [Tooltip("Please uncheck it on production")]
    public bool needResetHP = true;

    [Header("ScriptableObjects")]
    public PlayerData playerData;

    [Header("Debug")]
    public VoidEventChannel onDebugDeathEvent;

    [Header("Broadcast event channels")]
    public VoidEventChannel onPlayerDeath;

    [Header("Invulnerability Settings")]
    public bool isInvulnerable = false;
    public float invulnerableTime = 2.25f;
    public float invulnerableFlash = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize player data
        if (playerData == null)
        {
            playerData = ScriptableObject.CreateInstance<PlayerData>();
        }
        if (needResetHP || playerData.currentHealth <= 0)
        {
            playerData.currentHealth = playerData.maxHealth;
        }
        {
            Debug.Log("PV actuels du joueur : " + playerData.currentHealth);
        }
    }

private void OnEnable()
{
    if (onDebugDeathEvent != null)
        onDebugDeathEvent.OnEventRaised += Die;
}

    public void TakeDamage(float damage)
    {
        if (playerInvulnerable != null && playerInvulnerable.isInvulnerable && damage < float.MaxValue) return;
        if (isInvulnerable) return;

        playerData.currentHealth -= damage;
        if (playerData.currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invulnerable());
        }
    }

    // Méthode pour infliger des dégâts au joueur (renommée pour éviter la confusion)
    public void Hurt(int damage = 1)
    {
        if (isInvulnerable) return;

        // Réduit les points de vie actuels en fonction des dégâts reçus
        playerData.currentHealth -= damage;

        // Vérifie si les points de vie sont tombés à 0 ou moins
        if (playerData.currentHealth <= 0)
        {
            Die(); // Appelle la méthode pour gérer la mort du joueur
        }
        else
        {
            StartCoroutine(Invulnerable());
        }
    }

private void Die()
{
    Debug.Log("Le joueur est mort !");
    onPlayerDeath?.Raise(); 

    if (GetComponent<Rigidbody2D>() != null)
        GetComponent<Rigidbody2D>().simulated = false;

    if (transform != null)
        transform.Rotate(0f, 0f, 45f);

    if (animator != null)
        animator.SetTrigger("Death");

    Invoke("ShowGameOverScreen", 1.5f);
}


private void ShowGameOverScreen()
{
if (gameOverScreen != null)
{
    gameOverScreen.ShowGameOverScreen();
}
else
{
    Debug.LogError("GameOverScreen non trouvé dans la scène !");
}
}



public void OnPlayerDeathAnimationCallback()
{
    // Désactive uniquement après un délai (évite la disparition immédiate)
    Invoke("HidePlayer", 2f);
}

private void HidePlayer()
{
    if (sr != null)
        sr.enabled = false;
}


private void OnDisable()
{
    if (onDebugDeathEvent != null)
        onDebugDeathEvent.OnEventRaised -= Die;
}

    // Coroutine pour gérer l'invulnérabilité temporaire
    IEnumerator Invulnerable()
    {
        isInvulnerable = true;
        Color startColor = sr.color;

        WaitForSeconds invulnerableFlashWait = new WaitForSeconds(invulnerableFlash);

        for (float i = 0; i < invulnerableTime; i += invulnerableFlash)
        {
            sr.color = sr.color.a == 1 ? Color.clear : startColor;
            yield return invulnerableFlashWait;
        }
        sr.color = startColor; // Assure que le sprite est visible à la fin
        isInvulnerable = false;
    }
}
