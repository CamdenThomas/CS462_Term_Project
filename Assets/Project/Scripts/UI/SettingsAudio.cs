using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsAudio : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider master, music, sfx;

    void Start()
    {
        Init(master, "MasterVol");
        Init(music, "MusicVol");
        Init(sfx, "SfxVol");
    }

    void Init(Slider s, string param)
    {
        s.minValue = 0.0001f;
        s.maxValue = 1f;
        s.SetValueWithoutNotify(PlayerPrefs.GetFloat(param, 0.8f));
        Apply(param, s.value);
        s.onValueChanged.AddListener(v => { Apply(param, v); PlayerPrefs.SetFloat(param, v); });
    }

    void Apply(string param, float v) => mixer.SetFloat(param, Mathf.Log10(v) * 20f);
}
