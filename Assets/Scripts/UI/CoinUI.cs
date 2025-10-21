using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void Awake()
    {
        if (coinText == null)
            coinText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void Start()
    {
        TrySubscribe(); 
    }

    private void OnDisable()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.OnCoinChanged -= UpdateUI;
    }

    private void TrySubscribe()
    {
        if (CoinManager.Instance == null) return;
        CoinManager.Instance.OnCoinChanged -= UpdateUI; 
        CoinManager.Instance.OnCoinChanged += UpdateUI;
        UpdateUI(CoinManager.Instance.CoinCount);
    }

    private void UpdateUI(int count)
    {
        if (coinText != null)
            coinText.text = $"Monedas: {count}";
    }
}