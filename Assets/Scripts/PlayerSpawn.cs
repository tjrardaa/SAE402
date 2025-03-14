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
        // Initialise la position avec le dernier checkpoint ou la position actuelle
        if (lastCheckpointPosition.CurrentValue != null){
               transform.position = lastCheckpointPosition.CurrentValue;
        }
        else
        {
            // Si aucun checkpoint enregistré, utilise la position initiale
            lastCheckpointPosition.CurrentValue = transform.position;
        }

        currentSpawnPosition = gameObject.transform.position;
        initialSpawnPosition = gameObject.transform.position;
    }
}
