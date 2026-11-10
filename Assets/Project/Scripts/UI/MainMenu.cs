using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject[] otherPanels;
    [Tooltip("Greyed out while there is no save to continue, so it never opens a screen of empty, disabled slots.")]
    [SerializeField] Button continueButton;

    void Start() { RefreshContinue(); }

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
        RefreshContinue();
    }

    void RefreshContinue()
    {
        if (continueButton == null) return;
        bool anySave = false;
        for (int i = 0; i < SlotSaves.SlotCount; i++)
            if (SlotSaves.Exists(i)) anySave = true;
        continueButton.interactable = anySave;
        var hover = continueButton.GetComponent<MenuButtonHover>();
        if (hover != null) hover.Refresh();
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
