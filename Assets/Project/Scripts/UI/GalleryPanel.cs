using UnityEngine;

public class GalleryPanel : MonoBehaviour
{
    [System.Serializable]
    public class EndingInfo
    {
        public string endingName;
        public Sprite sprite;
        public string hint;
    }

    public EndingCard[] cards;
    public EndingInfo[] endings;

    void OnEnable() { Refresh(); }

    public void Refresh()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            EndingInfo info = i < endings.Length ? endings[i] : null;
            string label = info != null ? info.endingName : "Ending " + (i + 1);
            string hint = info != null ? info.hint : "";
            Sprite sprite = info != null ? info.sprite : null;
            cards[i].Setup(label, hint, sprite, EndingSaves.IsUnlocked(i));
        }
    }
}
