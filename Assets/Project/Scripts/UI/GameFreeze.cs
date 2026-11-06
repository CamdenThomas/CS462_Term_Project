using System.Collections.Generic;
using UnityEngine;

public static class GameFreeze
{
    static readonly List<Behaviour> frozen = new List<Behaviour>();
    static float savedTimeScale = 1f;
    static bool active;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        frozen.Clear();
        active = false;
    }

    public static void Freeze(GameObject[] extraTargets)
    {
        if (active) return;
        active = true;
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        frozen.Clear();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) FreezeTree(player);
        else Debug.LogWarning("GameFreeze: no object tagged Player was found.");

        if (Camera.main != null) FreezeSelf(Camera.main.gameObject);

        if (extraTargets != null)
        {
            foreach (GameObject t in extraTargets)
            {
                if (t != null) FreezeTree(t);
            }
        }

        Debug.Log("GameFreeze: froze " + frozen.Count + " components.");
    }

    static void FreezeTree(GameObject go)
    {
        foreach (Behaviour b in go.GetComponentsInChildren<Behaviour>())
            TryFreeze(b);
    }

    static void FreezeSelf(GameObject go)
    {
        foreach (Behaviour b in go.GetComponents<Behaviour>())
            TryFreeze(b);
    }

    static void TryFreeze(Behaviour b)
    {
        if (b == null || !b.enabled) return;
        if (b is Camera || b is AudioListener || b is AudioSource || b is Light || b is Canvas) return;
        b.enabled = false;
        frozen.Add(b);
    }

    public static void Unfreeze()
    {
        if (!active) return;
        active = false;
        foreach (Behaviour b in frozen)
        {
            if (b != null) b.enabled = true;
        }
        frozen.Clear();
        Time.timeScale = savedTimeScale;
    }
}