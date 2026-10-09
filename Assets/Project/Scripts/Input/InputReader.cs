using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TroUble.Input {
    /// <summary>
    /// The only class that touches the Input System. Everyone else listens to its C# events.
    /// </summary>
    [CreateAssetMenu( menuName = "TroUble/Input Reader" )]
    public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions {
        private InputSystem_Actions inputActions;

        public event Action<Vector2> Move;
        public event Action<Vector2> Look;
        public event Action<bool> SprintChanged;
        public event Action<bool> CrouchChanged;
        public event Action InteractStarted;
        public event Action InteractCanceled;

        private void OnEnable() {
            Setup();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
        }

        private void OnDisable() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeChanged;
#endif
            if ( inputActions == null ) { return; }
            inputActions.Player.RemoveCallbacks( this );
            DisableAll();
        }

        private void Setup() {
            if ( inputActions != null && Application.isPlaying ) { inputActions.Dispose(); }
            inputActions = new InputSystem_Actions();
            inputActions.Player.AddCallbacks( this );
            EnableGameplay();
        }

#if UNITY_EDITOR
        // Domain reload is off, so this asset's OnEnable doesn't run again when Play starts, and the
        // Input System disables and destroys every action when Play ends. Rebuild them on every Play.
        private void OnPlayModeChanged( UnityEditor.PlayModeStateChange state ) {
            if ( state == UnityEditor.PlayModeStateChange.EnteredPlayMode ) { Setup(); }
        }
#endif

        public void OnMove( InputAction.CallbackContext context ) { Move?.Invoke( context.ReadValue<Vector2>() ); }
        public void OnLook( InputAction.CallbackContext context ) { Look?.Invoke( context.ReadValue<Vector2>() ); }

        // Buttons report started (pressed), performed, then canceled (released). Only started and canceled matter.
        public void OnSprint( InputAction.CallbackContext context ) {
            if ( context.started ) { SprintChanged?.Invoke( true ); }
            else if ( context.canceled ) { SprintChanged?.Invoke( false ); }
        }

        public void OnCrouch( InputAction.CallbackContext context ) {
            if ( context.started ) { CrouchChanged?.Invoke( true ); }
            else if ( context.canceled ) { CrouchChanged?.Invoke( false ); }
        }

        public void OnInteract( InputAction.CallbackContext context ) {
            if ( context.started ) { InteractStarted?.Invoke(); }
            else if ( context.canceled ) { InteractCanceled?.Invoke(); }
        }

        // In the input asset but not used by the game. The interface still requires them.
        public void OnAttack( InputAction.CallbackContext context ) { }
        public void OnJump( InputAction.CallbackContext context ) { }
        public void OnPrevious( InputAction.CallbackContext context ) { }
        public void OnNext( InputAction.CallbackContext context ) { }

        public void EnableGameplay() { inputActions.UI.Disable(); inputActions.Player.Enable(); }
        public void EnableUI() { inputActions.Player.Disable(); inputActions.UI.Enable(); }
        public void DisableAll() { inputActions.Player.Disable(); inputActions.UI.Disable(); }
    }
}
