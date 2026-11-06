using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public Image suspicionFill;
    public TMP_Text suspicionLabel;
    public TMP_Text stageLabel;

    public Color calmColor = new Color32(0xF9, 0xBE, 0x3A, 255);
    public Color dangerColor = new Color32(0xDA, 0x25, 0x1D, 255);

    [Range(0, 100)] public float previewSuspicion = 30f;

    public static HUD Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        int slot = PlayerPrefs.GetInt("ActiveSlot", 0);
        if (SlotSaves.Exists(slot))
        {
            SlotData data = SlotSaves.Load(slot);
            SetStage(data.stageName, data.week);
            SetSuspicion(data.suspicion);
        }
        else
        {
            SetSuspicion(0f);
        }
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void SetSuspicion(float percent)
    {
        percent = Mathf.Clamp(percent, 0f, 100f);
        suspicionFill.fillAmount = percent / 100f;
        suspicionFill.color = Color.Lerp(calmColor, dangerColor, percent / 100f);
        suspicionLabel.text = "SUSPICION " + Mathf.RoundToInt(percent) + "%";
    }

    public void SetStage(string stageName, int week)
    {
        stageLabel.text = stageName + "  |  Week " + week;
    }

    [ContextMenu("Preview HUD")]
    void Preview()
    {
        SetSuspicion(previewSuspicion);
        SetStage("CHILDHOOD", 1);
    }

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
}