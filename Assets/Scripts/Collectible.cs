using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{
    public CollectibleVariable data;
    public GameObject collectedEffect;
    public SpriteRenderer spriteRenderer;

    public UnityEvent onPickUp;
       
    public bool canBeDestroyedOnContact = true; // Renommé pour plus de clarté

    private Vector3 startAngle;
    private float finalAngle;
    private float rotationOffset = 15f;
    private float oscillationSpeed = 1.5f;

    private void Awake()
    {
        if (spriteRenderer != null && data != null)
        {
            spriteRenderer.sprite = data.sprite;
        }

        startAngle = transform.eulerAngles;
    }

    private void Update()
    {
        finalAngle = startAngle.z + Mathf.Sin(Time.time * oscillationSpeed) * rotationOffset;
        transform.eulerAngles = new Vector3(startAngle.x, startAngle.y, finalAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeDestroyedOnContact && collision.CompareTag("Player"))
        {
            Picked();
        }
    }

    public void Picked()
    {
        if (collectedEffect != null)
        {
            GameObject effect = Instantiate(collectedEffect, transform.position, transform.rotation);
            Animator effectAnimator = effect.GetComponent<Animator>();
            if (effectAnimator != null)
            {
                Destroy(effect, effectAnimator.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                Destroy(effect, 1f); // Fallback if no animator
            }
        }

        if (data != null)
        {
            data.PickItem(transform.position);
        }
        
        onPickUp?.Invoke();

        Destroy(gameObject);
    }
}
