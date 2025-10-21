using UnityEngine;

public class PassLevel : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    private bool playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            LevelManager.Instance.PassLevel();
        }
    }
}
