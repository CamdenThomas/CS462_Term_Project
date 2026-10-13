using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueLabel : MonoBehaviour
{
    public Slider slider;
    public TMP_Text label;

    void OnEnable()
    {
        slider.onValueChanged.AddListener(UpdateLabel);
        UpdateLabel(slider.value);
    }

    void OnDisable()
    {
        slider.onValueChanged.RemoveListener(UpdateLabel);
    }

    void UpdateLabel(float v)
    {
        label.text = Mathf.RoundToInt(v * 100f).ToString();
    }
}
