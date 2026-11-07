using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject dim;
    public string menuSceneName = "MainMenu";

    bool paused;
    CursorLockMode previousLock;
    bool previousVisible;

    void Awake()
    {
        pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (dim != null) dim.SetActive(false);
    }

    void OnDestroy()
    {
        // The scene can be unloaded while paused (restart with R, quit to menu). GameFreeze is static and
        // outlives this object, so let go of our freeze or the next pause in the new scene would not work.
        if (paused) GameFreeze.Unfreeze(this);
    }

    void Update()
    {
        if (ReportCard.IsOpen || ReportCard.JustClosed) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (settingsPanel != null && settingsPanel.activeSelf) CloseSettings();
            else if (paused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        if (paused) return;
        paused = true;
        previousLock = Cursor.lockState;
        previousVisible = Cursor.visible;

        GameFreeze.Freeze(this);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (dim != null) dim.SetActive(true);
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        if (!paused) return;
        paused = false;
        GameFreeze.Unfreeze(this);

        Cursor.lockState = previousLock;
        Cursor.visible = previousVisible;
        pausePanel.SetActive(false);
        if (dim != null) dim.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (settingsPanel == null) return;
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void QuitToMenu()
    {
        if (paused)
        {
            paused = false;
            GameFreeze.Unfreeze(this);
        }
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(menuSceneName);
    }
}
