using UnityEngine;
using TMPro;

public class SettingsTabs : MonoBehaviour
{
    [SerializeField] GameObject[] tabPanels;
    [SerializeField] TMP_Text[] tabLabels;
    [SerializeField] Color active = new Color32(0xF9, 0xBE, 0x3A, 255);
    [SerializeField] Color inactive = new Color32(0xFF, 0xF4, 0xD6, 160);

    void OnEnable() => Show(0);

    public void Show(int index)
    {
        for (int i = 0; i < tabPanels.Length; i++)
        {
            tabPanels[i].SetActive(i == index);
            tabLabels[i].color = i == index ? active : inactive;
        }
    }
}
