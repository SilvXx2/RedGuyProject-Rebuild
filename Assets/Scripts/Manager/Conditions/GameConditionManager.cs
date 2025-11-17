using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameConditionManager : MonoBehaviour
{
    [Header("Referencia al jugador")]
    [SerializeField] private Health playerHealth;

    [Header("Escenas")] 
    [SerializeField] private string gameOverSceneName;
    [SerializeField] private string victorySceneName;

    private void Awake()
    {
        if (!playerHealth)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                playerHealth = player.GetComponent<Health>();
            }
        }

        if (playerHealth)
        {
            playerHealth.OnDeath += OnPlayerDeath;
        }
        else
        {
            Debug.LogWarning("GameConditionManager: No se encontró Health del jugador.", this);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth)
        {
            playerHealth.OnDeath -= OnPlayerDeath;
        }
    }

    private void OnPlayerDeath(Health _)
    {
        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.Log("GameConditionManager: Player muerto. No hay escena configurada, podrías pausar o mostrar UI aquí.");
        }
    }

    public void TriggerVictory()
    {
        if (!string.IsNullOrEmpty(victorySceneName))
        {
        SceneManager.LoadScene(victorySceneName);
        }
    }
}