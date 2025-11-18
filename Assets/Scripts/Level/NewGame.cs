using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    [SerializeField] private string firstLevelSceneName = "Level1"; 

    public void OnNewGamePressed()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.SetCoins(0);
        }

        if (!string.IsNullOrEmpty(firstLevelSceneName))
        {
            SceneManager.LoadScene(firstLevelSceneName);
        }
    }
}