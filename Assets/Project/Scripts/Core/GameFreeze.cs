using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The one place that stops the game: time scale 0 and the player's and camera's behaviours switched off.
/// Several things can hold a freeze at once (the pause menu and the report card, for example). Each one passes
/// itself as the owner, and the game only starts again once every owner has let go.
/// Freezing twice with the same owner does nothing, so callers don't need to track whether they already froze.
/// </summary>
public static class GameFreeze
{
    static readonly List<Behaviour> frozen = new List<Behaviour>();
    static readonly HashSet<object> owners = new HashSet<object>();
    static float savedTimeScale = 1f;

    public static bool IsFrozen { get { return owners.Count > 0; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        frozen.Clear();
        owners.Clear();
        savedTimeScale = 1f;
    }

    public static void Freeze(object owner, GameObject[] extraTargets = null)
    {
        if (owner == null || !owners.Add(owner)) return;

        if (owners.Count == 1)
        {
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            frozen.Clear();

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) FreezeTree(player);
            else Debug.LogWarning("GameFreeze: no object tagged Player was found.");

            if (Camera.main != null) FreezeSelf(Camera.main.gameObject);
        }

        if (extraTargets != null)
        {
            foreach (GameObject t in extraTargets)
            {
                if (t != null) FreezeTree(t);
            }
        }
    }

    public static void Unfreeze(object owner)
    {
        if (owner == null || !owners.Remove(owner)) return;
        if (owners.Count > 0) return;

        foreach (Behaviour b in frozen)
        {
            if (b != null) b.enabled = true;
        }
        frozen.Clear();
        Time.timeScale = savedTimeScale;
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
}
