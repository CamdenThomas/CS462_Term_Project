using UnityEngine;

namespace TroUble.Interaction {
    /// <summary>
    /// A door or cabinet door. Swings around its hinge between closed and open, and can be locked.
    /// The pose it has in the scene is its closed pose.
    /// </summary>
    public class Door : MonoBehaviour, IInteractable {
        [SerializeField] private Transform hinge;
        [SerializeField] private float openAngle = 100f;
        [SerializeField] private float swingTime = 0.25f;
        [SerializeField] private string openPrompt = "Open";
        [SerializeField] private string closePrompt = "Close";
        [SerializeField] private float holdTime = 0f;
        [SerializeField] private bool isOpen;
        [SerializeField] private bool isLocked;

        private Quaternion closedRotation;
        private float swing;    // 0 = closed, 1 = fully open

        public string Prompt => isOpen ? closePrompt : openPrompt;
        public float HoldTime => holdTime;
        public bool IsOpen => isOpen;

        private void Awake() {
            if ( hinge == null ) { hinge = transform; }
            closedRotation = hinge.localRotation;
            swing = isOpen ? 1f : 0f;
            ApplySwing();
        }

        private void Update() {
            float target = isOpen ? 1f : 0f;
            if ( Mathf.Approximately( swing, target ) ) { return; }
            swing = Mathf.MoveTowards( swing, target, Time.deltaTime / swingTime );
            ApplySwing();
        }

        // Ease in and out so the door doesn't snap, then turn the hinge around its up axis.
        private void ApplySwing() {
            float angle = openAngle * Mathf.SmoothStep( 0f, 1f, swing );
            hinge.localRotation = closedRotation * Quaternion.Euler( 0f, angle, 0f );
        }

        public bool CanInteract( IInteractor who ) { return !isLocked; }

        public void Interact( IInteractor who ) { isOpen = !isOpen; }

        public void SetLocked( bool locked ) { isLocked = locked; }
    }
}
