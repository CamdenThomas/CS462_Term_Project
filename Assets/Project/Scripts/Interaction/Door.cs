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
        [Tooltip( "Start a closed door partly open (left ajar). It still counts as closed: using it swings it the rest of the way open." )]
        [SerializeField, Range( 0f, 1f )] private float startAjar = 0f;

        private Quaternion closedRotation;
        private float swing;    // 0 = closed, 1 = fully open
        private float restSwing;    // where "closed" sits: ajar until someone first uses the door

        public string Prompt => isOpen ? closePrompt : openPrompt;
        public float HoldTime => holdTime;
        public bool IsOpen => isOpen;

        private void Awake() {
            if ( hinge == null ) { hinge = transform; }
            closedRotation = hinge.localRotation;
            restSwing = startAjar;
            swing = isOpen ? 1f : restSwing;
            ApplySwing();
        }

        private void Update() {
            float target = isOpen ? 1f : restSwing;
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

        public void Interact( IInteractor who ) {
            isOpen = !isOpen;
            restSwing = 0f;
        }

        public void SetLocked( bool locked ) { isLocked = locked; }
    }
}
