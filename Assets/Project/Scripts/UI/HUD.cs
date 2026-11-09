using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("Suspicion eye (full = caught)")]
    public Image suspicionFill;

    [Header("Meters: Hunger, Energy, Boredom (full = good, empty = bad)")]
    public Image[] meterFills;

    [Header("Day and phase")]
    public TMP_Text dayTimeLabel;

    [Header("To-do note")]
    public TMP_Text todoText;

    [Header("Optional old stage label (can stay empty)")]
    public TMP_Text stageLabel;

    public Color calmColor = new Color32(0xF9, 0xBE, 0x3A, 255);
    public Color dangerColor = new Color32(0xDA, 0x25, 0x1D, 255);
    public Color meterColor = new Color32(0x2B, 0x2D, 0x5B, 255);
    public Color meterLowColor = new Color32(0xDA, 0x25, 0x1D, 255);
    [Range(0f, 1f)] public float lowThreshold = 0.25f;

    [Range(0, 100)] public float previewSuspicion = 30f;

    string[] taskNames = new string[0];
    bool[] taskDone = new bool[0];

    public static HUD Instance { get; private set; }

    void Awake() { Instance = this; }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        int slot = PlayerPrefs.GetInt("ActiveSlot", 0);
        if (SlotSaves.Exists(slot))
        {
            SlotData data = SlotSaves.Load(slot);
            SetSuspicion(data.suspicion);
        }
        else
        {
            SetSuspicion(0f);
        }
    }

    public void SetSuspicion(float percent)
    {
        if (suspicionFill == null) return;
        percent = Mathf.Clamp(percent, 0f, 100f);
        suspicionFill.fillAmount = percent / 100f;
        suspicionFill.color = Color.Lerp(calmColor, dangerColor, percent / 100f);
    }

    public void SetMeter(int index, float percent)
    {
        if (meterFills == null || index < 0 || index >= meterFills.Length) return;
        Image fill = meterFills[index];
        if (fill == null) return;
        float t = Mathf.Clamp01(percent / 100f);
        fill.fillAmount = t;
        fill.color = t <= lowThreshold ? meterLowColor : meterColor;
    }

    public void SetHunger(float percent) { SetMeter(0, percent); }
    public void SetEnergy(float percent) { SetMeter(1, percent); }
    public void SetBoredom(float percent) { SetMeter(2, percent); }

    public void SetDayPhase(string day, string phase)
    {
        if (dayTimeLabel == null) return;
        dayTimeLabel.text = day + "  " + phase;
    }

    public void SetDayTime(string day, int hour24, int minute)
    {
        if (dayTimeLabel == null) return;
        int hour12 = hour24 % 12;
        if (hour12 == 0) hour12 = 12;
        string suffix = hour24 < 12 ? "AM" : "PM";
        dayTimeLabel.text = day + "  " + hour12 + ":" + minute.ToString("00") + " " + suffix;
    }

    public void SetTasks(string[] tasks, bool[] done)
    {
        taskNames = tasks != null ? (string[])tasks.Clone() : new string[0];
        taskDone = new bool[taskNames.Length];
        if (done != null)
        {
            for (int i = 0; i < taskDone.Length && i < done.Length; i++)
                taskDone[i] = done[i];
        }
        RefreshTasks();
    }

    public void SetTaskDone(int index, bool done)
    {
        if (index < 0 || index >= taskDone.Length) return;
        taskDone[index] = done;
        RefreshTasks();
    }

    void RefreshTasks()
    {
        if (todoText == null) return;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < taskNames.Length; i++)
        {
            if (i > 0) sb.Append('\n');
            if (taskDone[i]) sb.Append("<s>- ").Append(taskNames[i]).Append("</s>");
            else sb.Append("- ").Append(taskNames[i]);
        }
        todoText.text = sb.ToString();
    }

    public void SetStage(string stageName, int week)
    {
        if (stageLabel != null) stageLabel.text = stageName + "  |  Week " + week;
    }

    [ContextMenu("Preview HUD")]
    void Preview()
    {
        SetSuspicion(previewSuspicion);
        SetHunger(70f);
        SetEnergy(45f);
        SetBoredom(15f);
        SetDayPhase("MONDAY", "AFTER SCHOOL");
        SetTasks(new[] { "Wash dishes", "Homework", "Take out trash" },
                 new[] { false, true, false });
    }

#if UNITY_EDITOR
    float debugValue;

    void Update()
    {
        var k = UnityEngine.InputSystem.Keyboard.current;
        if (k == null) return;
        if (k.equalsKey.wasPressedThisFrame)
        {
            debugValue = Mathf.Clamp(debugValue + 10f, 0f, 100f);
            SetSuspicion(debugValue);
        }
        if (k.minusKey.wasPressedThisFrame)
        {
            debugValue = Mathf.Clamp(debugValue - 10f, 0f, 100f);
            SetSuspicion(debugValue);
        }
    }
#endif
}
