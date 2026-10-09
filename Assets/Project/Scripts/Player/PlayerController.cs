using UnityEngine;
using TroUble.Data;
using TroUble.Input;

namespace TroUble.Player {
    /// <summary>
    /// Moves the CharacterController and owns MoveMode. Reads only InputReader.
    /// </summary>
    [RequireComponent( typeof( CharacterController ) )]
    public class PlayerController : MonoBehaviour {
        [SerializeField] private InputReader input;
        [SerializeField] private FirstPersonLook look;

        [Header( "Speeds (m/s)" )]
        [SerializeField] private float walkSpeed = 2.5f;
        [SerializeField] private float sprintSpeed = 4.5f;
        [SerializeField] private float crouchSpeed = 1.2f;
        [SerializeField] private float gravity = -20f;

        [Header( "Body (m)" )]
        [SerializeField] private float standHeight = 1.2f;
        [SerializeField] private float crouchHeight = 0.8f;
        [SerializeField] private float standEye = 1.1f;
        [SerializeField] private float crouchEye = 0.7f;

        private CharacterController body;
        private Vector2 moveInput;
        private bool sprintHeld;
        private bool crouchHeld;
        private float verticalSpeed;

        public MoveMode Mode { get; private set; }
        public Vector3 Velocity { get; private set; }

        private void Awake() { body = GetComponent<CharacterController>(); }

        private void OnEnable() {
            input.Move += OnMove;
            input.SprintChanged += OnSprint;
            input.CrouchChanged += OnCrouch;
        }

        private void OnDisable() {
            input.Move -= OnMove;
            input.SprintChanged -= OnSprint;
            input.CrouchChanged -= OnCrouch;
        }

        // Input can arrive at any moment; just remember it. Movement happens once per frame in Update.
        private void OnMove( Vector2 value ) { moveInput = value; }
        private void OnSprint( bool held ) { sprintHeld = held; }
        private void OnCrouch( bool held ) { crouchHeld = held; }

        private void Update() {
            UpdateMode();
            ApplyMovement( Time.deltaTime );
        }

        private void UpdateMode() {
            MoveMode previous = Mode;
            if ( crouchHeld ) { Mode = MoveMode.Crouch; }
            else if ( sprintHeld && moveInput.y > 0f ) { Mode = MoveMode.Sprint; }
            else { Mode = MoveMode.Walk; }

            bool crouching = Mode == MoveMode.Crouch;
            if ( crouching != ( previous == MoveMode.Crouch ) ) {
                body.height = crouching ? crouchHeight : standHeight;
                body.center = new Vector3( 0f, body.height / 2f, 0f );
                look.SetEyeHeight( crouching ? crouchEye : standEye );
            }
        }

        private void ApplyMovement( float dt ) {
            // Relative to where the player faces, and diagonals no faster than straight lines.
            Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;
            direction = Vector3.ClampMagnitude( direction, 1f );

            float speed = Mode switch {
                MoveMode.Sprint => sprintSpeed,
                MoveMode.Crouch => crouchSpeed,
                MoveMode.Hidden => 0f,
                _ => walkSpeed
            };

            // A small constant push keeps the player stuck to the ramp on the way down instead of bouncing.
            if ( body.isGrounded && verticalSpeed < 0f ) { verticalSpeed = -2f; }
            else { verticalSpeed += gravity * dt; }

            Velocity = direction * speed + Vector3.up * verticalSpeed;
            body.Move( Velocity * dt );
        }

        /// <summary>Move the player instantly. CharacterController ignores plain position changes while enabled.</summary>
        public void Teleport( Vector3 position ) {
            body.enabled = false;
            transform.position = position;
            body.enabled = true;
        }
    }
}
