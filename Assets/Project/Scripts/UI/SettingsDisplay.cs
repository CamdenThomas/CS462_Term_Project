using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsDisplay : MonoBehaviour
{
    [SerializeField] Toggle fullscreen;
    [SerializeField] TMP_Dropdown resolution, quality;
    Resolution[] options;

    void Start()
    {
        fullscreen.isOn = Screen.fullScreen;
        fullscreen.onValueChanged.AddListener(v => Screen.fullScreen = v);

        options = Screen.resolutions;
        var labels = new List<string>();
        int current = 0;
        for (int i = 0; i < options.Length; i++)
        {
            labels.Add(options[i].width + " x " + options[i].height);
            if (options[i].width == Screen.width && options[i].height == Screen.height) current = i;
        }
        resolution.ClearOptions();
        resolution.AddOptions(labels);
        resolution.value = current;
        resolution.RefreshShownValue();
        resolution.onValueChanged.AddListener(i => Screen.SetResolution(options[i].width, options[i].height, Screen.fullScreen));

        quality.ClearOptions();
        quality.AddOptions(new List<string>(QualitySettings.names));
        quality.value = QualitySettings.GetQualityLevel();
        quality.RefreshShownValue();
        quality.onValueChanged.AddListener(QualitySettings.SetQualityLevel);
    }
}
