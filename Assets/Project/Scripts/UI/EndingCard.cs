using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingCard : MonoBehaviour
{
    public GameObject lockedView;
    public Image thumb;
    public TMP_Text nameLabel;

    public void Setup(string endingName, Sprite sprite, bool unlocked)
    {
        lockedView.SetActive(!unlocked);
        thumb.gameObject.SetActive(unlocked);
        if (sprite != null) thumb.sprite = sprite;
        nameLabel.text = unlocked ? endingName : "???";
    }
}