using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [Tooltip("Position actuelle de réapparition du joueur")]
    [SerializeField] public Vector3 currentSpawnPosition;

    [Tooltip("Position initiale de départ du niveau")]
    [SerializeField] private Vector3 initialSpawnPosition;

    [Tooltip("Référence à la variable scriptable de position de checkpoint")]
    public Vector3Variable lastCheckpointPosition;

    private void Awake()
    {
        Debug.Log(lastCheckpointPosition.CurrentValue);
        // Vérifie si un checkpoint a été enregistré
        if (lastCheckpointPosition.CurrentValue != null)
        {
            // Positionne le joueur au dernier checkpoint
            transform.position = (Vector3)lastCheckpointPosition.CurrentValue;
        }
        else
        {
            // Si aucun checkpoint, utilise la position actuelle comme point de départ
            lastCheckpointPosition.CurrentValue = transform.position;
        }

        currentSpawnPosition = transform.position;
        initialSpawnPosition = transform.position;
    }

}
