using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuButtonHover : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TMP_Text label;                      
    public GameObject magnet;
    public Color normalColor = new Color(1f, 0.957f, 0.839f, 1f);
    public Color hoverColor  = new Color(0.976f, 0.745f, 0.227f, 1f);
    public Color disabledColor = new Color(1f, 0.957f, 0.839f, 0.35f);

    Button button;

    // A disabled button (e.g. Continue with no saves) stays dim and never lights up.
    bool Usable { get { return button == null || button.interactable; } }

    void Awake()
    {
        if (label == null) label = GetComponent<TMP_Text>();
        button = GetComponent<Button>();
    }

    /// <summary>Redraw after the button is switched on or off.</summary>
    public void Refresh() { SetHover(false); }

    void OnEnable() { SetHover(false); }

    public void OnPointerEnter(PointerEventData e) { SetHover(true); }
    public void OnPointerExit(PointerEventData e)  { SetHover(false); }
    public void OnSelect(BaseEventData e)          { SetHover(true); }
    public void OnDeselect(BaseEventData e)        { SetHover(false); }

    void SetHover(bool on)
    {
        on = on && Usable;
        if (label != null) label.color = !Usable ? disabledColor : on ? hoverColor : normalColor;
        if (magnet != null) magnet.SetActive(on);
    }
}
