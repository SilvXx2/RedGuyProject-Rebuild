using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private GameConditionManager gameConditionManager;

    private void Awake()
    {
        if (!gameConditionManager)
            gameConditionManager = FindObjectOfType<GameConditionManager>();

        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        gameConditionManager?.TriggerVictory();
    }
}