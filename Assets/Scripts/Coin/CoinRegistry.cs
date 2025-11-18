using System.Collections.Generic;
using UnityEngine;

public class CoinRegistry : MonoBehaviour
{
    public static CoinRegistry Instance { get; private set; }

    private readonly Dictionary<string, CoinWithId> coinsById = new();

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
    }

    public void RegisterCoin(CoinWithId coin)
    {
        if (!string.IsNullOrEmpty(coin.CoinId))
        {
            if (!coinsById.ContainsKey(coin.CoinId))
                coinsById.Add(coin.CoinId, coin);
        }
        else
        {
            Debug.LogWarning($"Coin {coin.gameObject.name} no tiene CoinId asignado.");
        }
    }

    public void UnregisterCoin(CoinWithId coin)
    {
        if (!string.IsNullOrEmpty(coin.CoinId))
            coinsById.Remove(coin.CoinId);
    }

    public IEnumerable<string> GetExistingCoinIds()
    {
        return coinsById.Keys;
    }

    public void DisableCollectedCoins(HashSet<string> collectedIds)
    {
        foreach (var id in collectedIds)
        {
            if (coinsById.TryGetValue(id, out var coin))
            {
                coin.SetCollectedVisualState();
            }
        }
    }
}