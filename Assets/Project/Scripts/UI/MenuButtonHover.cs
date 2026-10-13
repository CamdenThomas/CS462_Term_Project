using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButtonHover : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TMP_Text label;                      
    public GameObject magnet;
    public Color normalColor = new Color(1f, 0.957f, 0.839f, 1f);
    public Color hoverColor  = new Color(0.976f, 0.745f, 0.227f, 1f);

    void Awake()
    {
        if (label == null) label = GetComponent<TMP_Text>();
    }

    void OnEnable() { SetHover(false); }

    public void OnPointerEnter(PointerEventData e) { SetHover(true); }
    public void OnPointerExit(PointerEventData e)  { SetHover(false); }
    public void OnSelect(BaseEventData e)          { SetHover(true); }
    public void OnDeselect(BaseEventData e)        { SetHover(false); }

    void SetHover(bool on)
    {
        if (label != null) label.color = on ? hoverColor : normalColor;
        if (magnet != null) magnet.SetActive(on);
    }
}
