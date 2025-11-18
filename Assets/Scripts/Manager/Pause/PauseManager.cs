using UnityEngine;

public interface IPauseHandler
{
    void OnPauseChanged(bool isPaused);
}

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [SerializeField] private GameObject pauseMenuRoot;

    private bool _isPaused;
    public bool IsPaused => _isPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void TogglePause()
    {
        SetPaused(!_isPaused);
    }

    public void SetPaused(bool paused)
    {
        if (_isPaused == paused) return;

        _isPaused = paused;
        Time.timeScale = paused ? 0f : 1f;

        if (pauseMenuRoot != null)
            pauseMenuRoot.SetActive(paused);
    }

    public void ResumeGame()
    {
        SetPaused(false);
    }
}
