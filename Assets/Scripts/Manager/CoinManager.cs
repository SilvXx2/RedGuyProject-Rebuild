using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public event Action<int> OnCoinChanged; 

    private int coinCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public int CoinCount => coinCount; 

    public void AddCoins(int amount)
    {
        coinCount += amount;
        OnCoinChanged?.Invoke(coinCount); 
        Debug.Log("Monedas actuales: " + coinCount);
    }
}
