using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ReportCard : MonoBehaviour
{
    public GameObject dim;
    public GameObject panelBorder;
    public GameObject panel;
    public Transform rowsParent;
    public TMP_Text commentText;
    public GameObject[] freezeObjects;
    public UnityEvent onContinue;

    public static ReportCard Instance { get; private set; }
    public static bool IsOpen { get; private set; }
    public static bool JustClosed { get { return Time.frameCount == closedFrame; } }

    static int closedFrame = -1;

    CursorLockMode savedLock;
    bool savedVisible;

    void Awake()
    {
        Instance = this;
        IsOpen = false;
        closedFrame = -1;
        SetVisible(false);
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            if (IsOpen) GameFreeze.Unfreeze(this);
            IsOpen = false;
        }
    }

    public void Show(string[] traits, string[] grades, string comment)
    {
        for (int i = 0; i < rowsParent.childCount; i++)
        {
            Transform row = rowsParent.GetChild(i);
            bool used = i < traits.Length && i < grades.Length;
            row.gameObject.SetActive(used);
            if (!used) continue;

            Transform t = row.Find("Trait");
            Transform g = row.Find("Grade");
            if (t != null)
            {
                TMP_Text traitText = t.GetComponent<TMP_Text>();
                if (traitText != null) traitText.text = traits[i];
            }
            if (g != null)
            {
                TMP_Text gradeText = g.GetComponent<TMP_Text>();
                if (gradeText != null) gradeText.text = grades[i];
            }
        }

        commentText.text = comment;

        if (!IsOpen)
        {
            savedLock = Cursor.lockState;
            savedVisible = Cursor.visible;
        }

        GameFreeze.Freeze(this, freezeObjects);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        IsOpen = true;
        SetVisible(true);
    }

    public void Continue()
    {
        if (IsOpen)
        {
            GameFreeze.Unfreeze(this);
            Cursor.lockState = savedLock;
            Cursor.visible = savedVisible;
        }
        IsOpen = false;
        closedFrame = Time.frameCount;
        SetVisible(false);
        onContinue.Invoke();
    }

    void SetVisible(bool on)
    {
        if (dim != null) dim.SetActive(on);
        if (panelBorder != null) panelBorder.SetActive(on);
        if (panel != null) panel.SetActive(on);
    }

    void Update()
    {
        var k = UnityEngine.InputSystem.Keyboard.current;
        if (k == null) return;

        if (IsOpen && k.escapeKey.wasPressedThisFrame)
        {
            Continue();
            return;
        }

#if UNITY_EDITOR
        if (k.tKey.wasPressedThisFrame && !IsOpen)
        {
               Show(new[] { "GRADES", "CREATIVITY", "COMMUNICATION", "DISCIPLINE", "FAMILY TRUST", "BROTHER BOND" },
                new[] { "A", "B", "C", "A", "B", "D" },"Test comment from the parents.");
        }
#endif
    }
}
