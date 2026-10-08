using System;
using UnityEngine;

namespace TroUble.Events {
    public abstract class EventChannel<T> : ScriptableObject {
        [SerializeField, TextArea]
        private string description;
        private Action<T> onRaised;

        public void Raise( T value ) { onRaised?.Invoke( value ); }
        public void Subscribe( Action<T> listener ) { onRaised += listener; }
        public void Unsubscribe( Action<T> listener ) { onRaised -= listener; }

#if UNITY_EDITOR
        // Domain reload is off, so this asset keeps its listeners between Play sessions.
        // Drop them when Play mode exits.
        private void OnEnable() { UnityEditor.EditorApplication.playModeStateChanged += ClearOnExit; }
        private void OnDisable() { UnityEditor.EditorApplication.playModeStateChanged -= ClearOnExit; }
        private void ClearOnExit( UnityEditor.PlayModeStateChange state ) {
            if ( state == UnityEditor.PlayModeStateChange.ExitingPlayMode ) onRaised = null;
        }
#endif
    }
}
