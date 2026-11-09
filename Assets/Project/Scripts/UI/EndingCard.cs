using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingCard : MonoBehaviour
{
    public GameObject lockedView;
    public Image thumb;
    public TMP_Text nameLabel;

    public void Setup(string endingName, string hint, Sprite sprite, bool unlocked)
    {
        if (lockedView != null) lockedView.SetActive(false);
        if (sprite != null) thumb.sprite = sprite;
        thumb.gameObject.SetActive(true);
        thumb.color = unlocked ? Color.white : Color.black;
        nameLabel.text = unlocked ? endingName : hint;
    }
}
