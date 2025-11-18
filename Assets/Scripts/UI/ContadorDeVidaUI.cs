using UnityEngine;
using TMPro;

public class ContadorDeVidaUI : MonoBehaviour
{
    [SerializeField] private TMP_Text lifeText;
    [SerializeField] private Health playerHealth;

    private void Awake()
    {
        if (lifeText == null)
        {
            Debug.LogError("ContadorDeVidaUI: falta asignar el TMP_Text.", this);
            enabled = false;
            return;
        }

        if (playerHealth == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerHealth = player.GetComponent<Health>();
        }

        if (playerHealth == null)
        {
            Debug.LogError("ContadorDeVidaUI: no se encontró Health en el Player.", this);
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += OnHealthChanged;
            OnHealthChanged(playerHealth);
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(Health h)
    {
        lifeText.text = $"Vidas: {h.CurrentHealth}";
    }
}


