using UnityEngine;

public class PauseInput : MonoBehaviour
{
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (PauseManager.Instance != null)
            {
                PauseManager.Instance.TogglePause();
            }
        }
    }
}
