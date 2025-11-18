using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        if (continueButton == null)
            continueButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        UpdateInteractable();
    }

    private void Update()
    {
        UpdateInteractable();
    }

    private void UpdateInteractable()
    {
        if (continueButton == null) return;

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint)
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
    }

    public void OnContinuePressed()
    {
        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint)
        {
            CheckpointManager.Instance.ContinueFromLastCheckpoint();
        }
        else
        {
            Debug.Log("No hay checkpoint guardado para continuar.");
        }
    }
}