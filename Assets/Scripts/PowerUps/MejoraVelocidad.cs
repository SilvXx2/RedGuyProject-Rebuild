using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MejoraVelocidad : MonoBehaviour
{
    [Header("UI (icono en el Canvas, no en este objeto)")]
    public Image itemIcon;

    [Header("Buff de velocidad")]
    [SerializeField] private float speedMultiplier = 1.3f; 
    [SerializeField] private float duration = 10f;

    private void Start()
    {
        if (itemIcon != null)
            itemIcon.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerMovement pj = collision.GetComponent<PlayerMovement>();
        if (pj == null)
        {
            Debug.LogWarning("MejoraVelocidad no encontró PlayerMovement en el Player.");
            return;
        }

        pj.AddSpeedBuff(speedMultiplier, duration);

        if (itemIcon != null)
            itemIcon.enabled = true;

        HidePickupVisuals();

        StartCoroutine(EndPowerUp());
    }

    private void HidePickupVisuals()
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    private IEnumerator EndPowerUp()
    {
        yield return new WaitForSeconds(duration);

        if (itemIcon != null)
            itemIcon.enabled = false;

        Destroy(gameObject);
    }
}
