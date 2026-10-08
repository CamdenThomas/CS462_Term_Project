using System;
using UnityEngine;

namespace TroUble.Events {
    [CreateAssetMenu( menuName = "TroUble/Channels/Void" )]
    public class VoidEventChannel : ScriptableObject {
        [SerializeField, TextArea]
        private string description;
        private Action onRaised;

        public void Raise() { onRaised?.Invoke(); }
        public void Subscribe( Action listener ) { onRaised += listener; }
        public void Unsubscribe( Action listener ) { onRaised -= listener; }

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
