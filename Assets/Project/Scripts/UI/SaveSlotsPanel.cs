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
            SlotData d = SlotSaves.Load(i);
            var button = cards[i].GetComponent<Button>();

            if (d != null)
            {
                int hours = (int)(d.playSeconds / 3600f);
                int minutes = ((int)(d.playSeconds / 60f)) % 60;

                cards[i].SetFilled(
                    d.stageName,
                    "Week " + d.week + "  |  Suspicion " + d.suspicion + "%",
                    "Playtime " + hours + "h" + minutes.ToString("00") + "m  |  Saved " + d.savedAt);
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
        bool filled = SlotSaves.Exists(slot);

        if (mode == 1)
        {
            StartGame(slot, false);
        }
        else if (!filled)
        {
            StartGame(slot, true);
        }
        else
        {
            pendingSlot = slot;
            if (overwritePopup != null) overwritePopup.SetActive(true);
        }
    }

    public void ConfirmOverwrite()
    {
        if (overwritePopup != null) overwritePopup.SetActive(false);
        StartGame(pendingSlot, true);
    }

    void StartGame(int slot, bool isNewGame)
    {
        if (isNewGame) SlotSaves.CreateNew(slot);

        PlayerPrefs.SetInt("ActiveSlot", slot);
        PlayerPrefs.SetInt("LoadMode", isNewGame ? 0 : 1);
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}
