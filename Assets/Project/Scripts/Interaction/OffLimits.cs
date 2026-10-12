using UnityEngine;

namespace TroUble.Interaction {
    /// <summary>
    /// Marks this object and everything under it as somewhere the player isn't allowed to be poking around,
    /// like the cookie cabinet. A parent who sees the player looking at any part of it from within reach
    /// counts that as misbehaving. It needs no settings: being on the object is the whole job.
    /// </summary>
    public class OffLimits : MonoBehaviour { }
}
