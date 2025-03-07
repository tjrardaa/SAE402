using UnityEngine;
using System.Collections;

public class Bloc : MonoBehaviour
{
    public Animator animator;
    public GameObject Items; // Préférez le nom "item" au singulier si c'est un seul préfab
    public int numberHits = 3;
    public SpriteRenderer sr;
    public bool isHidden = false;
    public PlatformEffector2D platformEffector2D; // Ajout de cette référence

    private ContactPoint2D[] listContact = new ContactPoint2D[10];
    private bool canBeDestroyed;

    private void Awake()
    {
        sr.enabled = !isHidden;
        if (platformEffector2D != null) // Vérification de null
            platformEffector2D.enabled = isHidden;
        animator.ResetTrigger("IsHit"); // Correction de la casse
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.GetContacts(listContact);

        if (listContact[0].normal.y > 0.5f && // Vérifie si le contact vient du haut
            collision.gameObject.CompareTag("Player") && numberHits > 0)
        {
            StartCoroutine(Hit());
        }
    }

    private IEnumerator Hit()
    {
        yield return null; // Attendre un frame avant d'exécuter l'action
        animator.SetTrigger("IsHit");
        numberHits--;

        if (Items != null)
        {
            GameObject item = Instantiate(
                Items,
                transform.position,
                Quaternion.identity
            );
            Collectible collectible = item.GetComponent<Collectible>();

            if (collectible != null)
            {
                collectible.canBeDestroyedOnContact = false; // Empêche la destruction immédiate
                Vector3 endPosition = item.transform.localPosition + Vector3.up * 1.5f;

                // Appel à la méthode MoveItemBackAndForth pour déplacer l'objet
                yield return StartCoroutine(MoveItemBackAndForth(item.transform, endPosition, 1f));

                collectible.canBeDestroyedOnContact = true; // Réactive la destruction après le mouvement
                collectible.Picked(); // Appelle la méthode Picked après le mouvement
            }
        }
    }

    private IEnumerator MoveItemBackAndForth(Transform itemTransform, Vector3 endPosition, float duration)
    {
        Vector3 startPosition = itemTransform.localPosition;
        float elapsedTime = 0f;

        // Mouvement aller
        while (elapsedTime < duration)
        {
            itemTransform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        // Mouvement retour
        while (elapsedTime < duration)
        {
            itemTransform.localPosition = Vector3.Lerp(endPosition, startPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        itemTransform.localPosition = startPosition; // Retour à la position initiale
    }

    void Start()
    {
        // Initialisation si nécessaire
    }

    void Update()
    {
        // Logique de mise à jour si nécessaire
    }
}
