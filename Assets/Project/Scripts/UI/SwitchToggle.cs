using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Toggle))]
public class SwitchToggle : MonoBehaviour
{
    public Image track;
    public RectTransform knob;
    public Color onColor  = new Color(0.976f, 0.745f, 0.227f);
    public Color offColor = new Color(0.17f, 0.18f, 0.36f);
    public float travel = 26f;
    public float speed = 14f;

    Toggle toggle;
    Coroutine anim;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnChanged);
    }

    void OnEnable()
    {

        float x = toggle.isOn ? travel : -travel;
        knob.anchoredPosition = new Vector2(x, knob.anchoredPosition.y);
        track.color = toggle.isOn ? onColor : offColor;
    }

    void OnChanged(bool on)
    {
        if (anim != null) StopCoroutine(anim);
        anim = StartCoroutine(Slide(on));
    }

    IEnumerator Slide(bool on)
    {
        float targetX = on ? travel : -travel;
        Color targetColor = on ? onColor : offColor;
        while (Mathf.Abs(knob.anchoredPosition.x - targetX) > 0.1f)
        {
            float x = Mathf.Lerp(knob.anchoredPosition.x, targetX, Time.unscaledDeltaTime * speed);
            knob.anchoredPosition = new Vector2(x, knob.anchoredPosition.y);
            track.color = Color.Lerp(track.color, targetColor, Time.unscaledDeltaTime * speed);
            yield return null;
        }
        knob.anchoredPosition = new Vector2(targetX, knob.anchoredPosition.y);
        track.color = targetColor;
    }
}
