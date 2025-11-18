using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CoinWithId : MonoBehaviour
{
    [SerializeField] private string coinId; 

    public string CoinId => coinId;

    private bool collected = false;

    private void Awake()
    {
        if (string.IsNullOrEmpty(coinId))
        {
            coinId = $"{gameObject.scene.name}_{gameObject.name}_{GetInstanceID()}";
        }
    }

    private void OnEnable()
    {
        CoinRegistry.Instance?.RegisterCoin(this);
    }

    private void OnDisable()
    {
        CoinRegistry.Instance?.UnregisterCoin(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            if (CoinManager.Instance != null)
                CoinManager.Instance.AddCoins(1);

            if (CheckpointManager.Instance != null)
                CheckpointManager.Instance.RegisterCollectedCoin(coinId);

            SetCollectedVisualState();
        }
    }

    public void SetCollectedVisualState()
    {
        gameObject.SetActive(false);
    }
}