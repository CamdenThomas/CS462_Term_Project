using UnityEngine;
using TroUble.Input;

namespace TroUble.Player {
    /// <summary>
    /// Mouse look with a pitch clamp. Sits on the Player: turning left and right rotates the whole body,
    /// looking up and down tilts only the camera pivot. Eye height rises each stage, so the house shrinks
    /// as the player grows.
    /// </summary>
    public class FirstPersonLook : MonoBehaviour {
        [SerializeField] private InputReader input;
        [SerializeField] private Transform pivot;
        [SerializeField] private float sensitivity = 0.1f;
        [SerializeField] private float pitchLimit = 80f;

        private float pitch = 0f;

        private void OnEnable() { input.Look += OnLook; Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        private void OnDisable() { input.Look -= OnLook; Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }

        // Mouse delta is already "how far it moved this frame", so no Time.deltaTime here.
        private void OnLook( Vector2 lookInput ) {
            float mouseX = lookInput.x * sensitivity;
            float mouseY = lookInput.y * sensitivity;
            pitch -= mouseY;
            pitch = Mathf.Clamp( pitch, -pitchLimit, pitchLimit );
            pivot.localRotation = Quaternion.Euler( pitch, 0f, 0f );
            transform.Rotate( Vector3.up * mouseX );
        }

        public void SetEyeHeight( float height ) { pivot.localPosition = new Vector3( 0f, height, 0f ); }
    }
}
