using System;
using System.Globalization;
using UnityEngine;

[Serializable]
public class SlotData
{
    public string stageName = "CHILDHOOD";
    public int week = 1;
    public int suspicion = 0;
    public float playSeconds = 0f;
    public string savedAt = "";
}

public static class SlotSaves
{
    public const int SlotCount = 3;

    static string Key(int slot) { return "UISlot_" + slot; }

    public static bool Exists(int slot) { return PlayerPrefs.HasKey(Key(slot)); }

    public static SlotData Load(int slot)
    {
        if (!Exists(slot)) return null;
        return JsonUtility.FromJson<SlotData>(PlayerPrefs.GetString(Key(slot)));
    }

    public static void Save(int slot, SlotData data)
    {
        data.savedAt = DateTime.Now.ToString("MMM d, yyyy", CultureInfo.InvariantCulture);
        PlayerPrefs.SetString(Key(slot), JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    public static void CreateNew(int slot) { Save(slot, new SlotData()); }

    public static void Delete(int slot)
    {
        PlayerPrefs.DeleteKey(Key(slot));
        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        for (int i = 0; i < SlotCount; i++) Delete(i);
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Clear Save Slots")]
    static void ClearFromMenu()
    {
        DeleteAll();
        Debug.Log("Save slots cleared.");
    }
#endif
}
