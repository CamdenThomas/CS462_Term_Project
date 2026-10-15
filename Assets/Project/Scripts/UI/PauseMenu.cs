using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject dim;
    public string menuSceneName = "MainMenu";

    [Header("Freeze the player while paused")]
    public GameObject player;

    bool paused;
    CursorLockMode previousLock;
    bool previousVisible;
    readonly List<Behaviour> frozen = new List<Behaviour>();

    void Awake()
    {
        pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (dim != null) dim.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (settingsPanel != null && settingsPanel.activeSelf) CloseSettings();
            else if (paused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        paused = true;
        previousLock = Cursor.lockState;
        previousVisible = Cursor.visible;

        Time.timeScale = 0f;
        FreezeInput();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (dim != null) dim.SetActive(true);
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        UnfreezeInput();

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
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(menuSceneName);
    }


    void FreezeInput()
    {
        if (player == null) player = GameObject.FindWithTag("Player");

        if (player != null) FreezeObject(player, true);
        if (Camera.main != null) FreezeObject(Camera.main.gameObject, false);
    }

    void FreezeObject(GameObject go, bool includeChildren)
    {
        Behaviour[] list = includeChildren
            ? go.GetComponentsInChildren<Behaviour>(false)
            : go.GetComponents<Behaviour>();

        foreach (var b in list)
        {
            if (b == null || !b.enabled) continue;
            if (b is Camera || b is AudioListener || b is AudioSource ||
                b is Light || b is Canvas) continue;

            b.enabled = false;
            frozen.Add(b);
        }
    }

    void UnfreezeInput()
    {
        foreach (var b in frozen)
            if (b != null) b.enabled = true;
        frozen.Clear();
    }
}
