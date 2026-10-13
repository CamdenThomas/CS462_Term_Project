using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveSlotsPanel : MonoBehaviour
{
    public TMP_Text title;
    public SlotCard[] cards;
    public GameObject overwritePopup;   

    [Header("Game")]
    public string gameSceneName = "House";

    // 0 = New Game, 1 = Load Game
    int mode;
    int pendingSlot;

    class FakeSave { public string name, detail, time; }
    FakeSave[] saves = new FakeSave[3]
    {
        new FakeSave {
            name = "CHILDHOOD",
            detail = "Week 3  |  Suspicion 40%",
            time = "Playtime 1h24m  |  Saved Oct 13, 2026" },
        null,
        null
    };

    void Awake()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            int slot = i;
            cards[i].index = i;
            cards[i].GetComponent<Button>().onClick.AddListener(() => OnSlotClicked(slot));
        }
    }

    public void SetMode(int newMode)
    {
        mode = newMode;
        title.text = (mode == 0) ? "NEW GAME" : "LOAD GAME";
        if (overwritePopup != null) overwritePopup.SetActive(false);
        Refresh();
    }

    void Refresh()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            var save = saves[i];
            var button = cards[i].GetComponent<Button>();

            if (save != null)
            {
                cards[i].SetFilled(save.name, save.detail, save.time);
                button.interactable = true;
            }
            else
            {
                cards[i].SetEmpty();
                button.interactable = (mode == 0);
            }
            cards[i].SetHighlight(false);
        }
    }

    void OnSlotClicked(int slot)
    {
        bool filled = saves[slot] != null;

        if (mode == 1)
        {
            StartGame(slot);
        }
        else if (!filled)
        {
            StartGame(slot);
        }
        else
        {
            pendingSlot = slot;
            if (overwritePopup != null) overwritePopup.SetActive(true);
        }
    }

    // Called by the Overwrite button
    public void ConfirmOverwrite()
    {
        if (overwritePopup != null) overwritePopup.SetActive(false);
        StartGame(pendingSlot);
    }

    void StartGame(int slot)
    {
        PlayerPrefs.SetInt("ActiveSlot", slot);
        PlayerPrefs.SetInt("LoadMode", mode);
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}
