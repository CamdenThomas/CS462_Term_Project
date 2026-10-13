using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class SlotCard : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public GameObject border;
    public TMP_Text slotNum;
    public TMP_Text slotName;
    public TMP_Text details;
    public TMP_Text timeplay;

    [Header("Empty look")]
    public GameObject emptyView;
    public GameObject thumb;
    public Image cardBackground;
    public Image dash;
    public Color emptyColor = new Color(0.08f, 0.09f, 0.23f, 0f);
    public Color dashHoverColor = new Color(0.976f, 0.745f, 0.227f, 1f);

    Color filledColor;
    Color dashNormalColor;
    bool colorStored;

    [HideInInspector] public int index;
    [HideInInspector] public bool isEmpty;

    void Awake() { StoreColors(); }
    void OnEnable() { SetHighlight(false); }

    void StoreColors()
    {
        if (colorStored) return;
        if (cardBackground != null) filledColor = cardBackground.color;
        if (dash != null) dashNormalColor = dash.color;
        colorStored = true;
    }

    public void OnPointerEnter(PointerEventData e) { SetHighlight(true); }
    public void OnPointerExit(PointerEventData e)  { SetHighlight(false); }
    public void OnSelect(BaseEventData e)          { SetHighlight(true); }
    public void OnDeselect(BaseEventData e)        { SetHighlight(false); }

    public void SetHighlight(bool on)
    {
        StoreColors();
        if (on && !GetComponent<Button>().interactable) on = false;

        if (border != null) border.SetActive(on && !isEmpty);

        if (dash != null) dash.color = (on && isEmpty) ? dashHoverColor : dashNormalColor;
    }

    void ShowFilledParts(bool show)
    {
        if (thumb != null) thumb.SetActive(show);
        if (slotNum != null) slotNum.gameObject.SetActive(show);
        if (slotName != null) slotName.gameObject.SetActive(show);
        if (details != null) details.gameObject.SetActive(show);
        if (timeplay != null) timeplay.gameObject.SetActive(show);
        if (emptyView != null) emptyView.SetActive(!show);
    }

    public void SetFilled(string name, string detailLine, string timeLine)
    {
        StoreColors();
        isEmpty = false;
        slotName.text = name;
        details.text = detailLine;
        timeplay.text = timeLine;
        ShowFilledParts(true);
        if (cardBackground != null) cardBackground.color = filledColor;
    }

    public void SetEmpty()
    {
        StoreColors();
        isEmpty = true;
        ShowFilledParts(false);
        if (cardBackground != null) cardBackground.color = emptyColor;
    }
}
