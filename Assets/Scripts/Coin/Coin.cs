using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoins(1);
            Destroy(gameObject);
        }
    }
}
