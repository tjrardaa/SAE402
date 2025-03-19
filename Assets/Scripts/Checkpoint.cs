using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public BoxCollider2D bc2d;

    public Vector3Variable lastCheckpointPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerSpawn playerSpawn = collision.GetComponent<PlayerSpawn>();
            if (playerSpawn != null)
            {
                // Met à jour la position actuelle du point de spawn du joueur
                playerSpawn.currentSpawnPosition = transform.position;

                lastCheckpointPosition.CurrentValue = transform.position;
                Debug.Log(lastCheckpointPosition.CurrentValue);
                // Désactive le BoxCollider2D pour éviter de réactiver ce checkpoint
                bc2d.enabled = false;


            }
        }
    }
}
