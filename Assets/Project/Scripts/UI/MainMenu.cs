using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject[] otherPanels;

    public void Open(GameObject panel)
    {
        menuPanel.SetActive(false);
        foreach (var p in otherPanels) p.SetActive(false);
        panel.SetActive(true);
    }

    public void Back()
    {
        foreach (var p in otherPanels) p.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
