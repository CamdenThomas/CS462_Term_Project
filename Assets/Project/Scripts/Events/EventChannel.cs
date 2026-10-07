using System;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public abstract class EventChannel<T> : ScriptableObject {
    private string description;
    private Action<T> onRaised;
    void Raise( T value ) { }
    void Subscribe( Action<T> listener ) { }
    void Unsubscribe( Action<T> listener ) { }
}