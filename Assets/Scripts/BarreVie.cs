using UnityEngine;
using UnityEngine.UI;

public class BarreVie : MonoBehaviour
{
    // Référence à l'image de remplissage de la barre de vie
    public Image fillImage;

    // Référence au script gérant la vie du joueur
    public PlayerHealth playerHealth;

    // Gradient pour changer la couleur de la barre de vie en fonction du niveau de vie
    public Gradient lifeColorGradient;
    
    public PlayerData playerData;

    void Start()
    {
        // Assurez-vous que playerHealth est assigné dans l'inspecteur Unity
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.playerData != null)
        {
            // Calcul du ratio de vie actuelle
            float lifeRatio = (float)playerHealth.playerData.currentHealth / (float)playerHealth.playerData.maxHealth;

            // Mise à jour du remplissage de l'image
            fillImage.fillAmount = lifeRatio;

            // Mise à jour de la couleur de l'image en fonction du ratio de vie
            fillImage.color = lifeColorGradient.Evaluate(lifeRatio);
        }
    }
}
