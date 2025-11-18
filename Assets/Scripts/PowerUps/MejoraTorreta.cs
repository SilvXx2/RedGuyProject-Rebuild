using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MejoraTorreta : MonoBehaviour
{
    [Header("UI (icono en el Canvas, no en este objeto)")]
    public Image itemIcon;

    [Header("Buff de torreta")]
    [SerializeField] private float fireRateMultiplier = 1.3f;      
    [SerializeField] private float bulletSpeedMultiplier = 1.3f;   
    [SerializeField] private float duration = 10f;

    private void Start()
    {
        if (itemIcon != null)
            itemIcon.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player == null)
        {
            Debug.LogWarning("MejoraTorreta no encontró PlayerMovement en el Player.");
            return;
        }

        var machine = player.machine;
        if (machine == null || machine.ShootState == null)
        {
            Debug.LogWarning("MejoraTorreta no encontró ShootState en la máquina de estados.");
            return;
        }

        float originalCooldown = 0.75f; 
        float newCooldown = originalCooldown / fireRateMultiplier;
        machine.ShootState.SetCooldown(newCooldown);

        player.AddBulletSpeedBuff(bulletSpeedMultiplier, duration);

        if (itemIcon != null)
            itemIcon.enabled = true;

        HidePickupVisuals();

        StartCoroutine(RevertirCambios(machine, originalCooldown));
    }

    private void HidePickupVisuals()
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    private IEnumerator RevertirCambios(CharacterStateMachine machine, float originalCooldown)
    {
        yield return new WaitForSeconds(duration);

        if (machine != null && machine.ShootState != null)
            machine.ShootState.SetCooldown(originalCooldown);

        if (itemIcon != null)
            itemIcon.enabled = false;

        Destroy(gameObject);
    }
}