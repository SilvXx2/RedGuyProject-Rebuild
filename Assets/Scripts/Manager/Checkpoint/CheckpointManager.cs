using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Checkpoints;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    public bool HasCheckpoint => _lastMemento != null;

    private CheckpointMemento _lastMemento;

    private readonly HashSet<string> collectedCoinIds = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureExists()
    {
        if (Instance == null)
        {
            var go = new GameObject("CheckpointManager");
            go.AddComponent<CheckpointManager>();
        }
    }

    public void RegisterCollectedCoin(string coinId)
    {
        if (!string.IsNullOrEmpty(coinId))
            collectedCoinIds.Add(coinId);
    }

    public void SaveCheckpoint(Vector3 respawnPosition)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        int coins = 0;
        if (CoinManager.Instance != null)
            coins = CoinManager.Instance.CoinCount;

        _lastMemento = new CheckpointMemento(sceneName, respawnPosition, coins, collectedCoinIds);

        Debug.Log($"Checkpoint guardado en '{sceneName}', pos {respawnPosition}, coins {coins}, monedas recogidas: {collectedCoinIds.Count}");
    }

    public void ContinueFromLastCheckpoint()
    {
        if (_lastMemento == null)
        {
            Debug.LogWarning("No hay checkpoint guardado para continuar.");
            return;
        }

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != _lastMemento.SceneName)
        {
            SceneManager.sceneLoaded += OnSceneLoadedForContinue;
            SceneManager.LoadScene(_lastMemento.SceneName);
        }
        else
        {
            RestoreFromMemento(_lastMemento);
        }
    }

    private void OnSceneLoadedForContinue(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedForContinue;

        if (_lastMemento != null && scene.name == _lastMemento.SceneName)
            RestoreFromMemento(_lastMemento);
    }

    private void RestoreFromMemento(CheckpointMemento memento)
    {
        collectedCoinIds.Clear();
        foreach (var id in memento.CollectedCoinIds)
            collectedCoinIds.Add(id);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = memento.PlayerPosition;
        }
        else
        {
            Debug.LogWarning("No se encontró Player al restaurar checkpoint.");
        }

        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.SetCoins(memento.Coins);
        }

        if (CoinRegistry.Instance != null)
        {
            CoinRegistry.Instance.DisableCollectedCoins(memento.CollectedCoinIds);
        }

        Debug.Log($"Restaurado checkpoint en '{memento.SceneName}', pos {memento.PlayerPosition}, coins {memento.Coins}, monedas recogidas: {memento.CollectedCoinIds.Count}");
    }
}
