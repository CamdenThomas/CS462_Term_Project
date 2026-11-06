using UnityEngine;

public static class EndingSaves
{
    public const int Count = 6;

    public static bool IsUnlocked(int id)
    {
        return PlayerPrefs.GetInt("UIEnding_" + id, 0) == 1;
    }

    public static void Unlock(int id)
    {
        PlayerPrefs.SetInt("UIEnding_" + id, 1);
        PlayerPrefs.Save();
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Unlock All Endings")]
    static void UnlockAll()
    {
        for (int i = 0; i < Count; i++) PlayerPrefs.SetInt("UIEnding_" + i, 1);
        PlayerPrefs.Save();
    }

    [UnityEditor.MenuItem("Tools/Lock All Endings")]
    static void LockAll()
    {
        for (int i = 0; i < Count; i++) PlayerPrefs.DeleteKey("UIEnding_" + i);
        PlayerPrefs.Save();
    }
#endif
}