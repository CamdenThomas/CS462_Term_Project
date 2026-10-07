using System;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public abstract class VoidEventChannel : ScriptableObject {
    private Action onRaised;
    void Raise() { }
    void Subscribe( Action listener ) { }
    void Unsubscribe( Action listener ) { }
}