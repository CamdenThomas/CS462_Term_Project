using UnityEngine;
using TroUble.Data;
using TroUble.Events;
using TroUble.Input;
using TroUble.Interaction;

namespace TroUble.Player {
    /// <summary>
    /// Finds what the player is looking at and uses it on E. Talks to the UI only through channels.
    /// </summary>
    public class PlayerInteractor : MonoBehaviour, IInteractor {
        [SerializeField] private InputReader input;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Camera cam;
        [SerializeField] private float reach = 1.6f;
        [SerializeField] private LayerMask mask;
        [SerializeField] private FocusChannel focusChanged;
        [SerializeField] private FloatChannel interactProgress;

        private IInteractable focused;
        private bool holding;
        private float holdTimer;

        public Transform Transform => transform;
        public IItemHolder Items => inventory;
        public IInteractable Focused => focused;

        private void OnEnable() {
            input.InteractStarted += BeginInteract;
            input.InteractCanceled += CancelInteract;
        }

        private void OnDisable() {
            input.InteractStarted -= BeginInteract;
            input.InteractCanceled -= CancelInteract;
            if ( focused != null ) { SetFocus( null ); }
        }

        private void Update() {
            IInteractable found = FindFocus();
            if ( found != focused ) { SetFocus( found ); }
            if ( holding ) { TickHold( Time.deltaTime ); }
        }

        private IInteractable FindFocus() {
            Transform eye = cam.transform;
            if ( !Physics.Raycast( eye.position, eye.forward, out RaycastHit hit, reach, mask ) ) { return null; }

            // The collider is often on a child (a door's panel), so look up the hierarchy for the script.
            IInteractable target = hit.collider.GetComponentInParent<IInteractable>();
            if ( target == null || !target.CanInteract( this ) ) { return null; }
            return target;
        }

        private void SetFocus( IInteractable target ) {
            focused = target;
            CancelInteract();
            RaiseFocus();
        }

        private void RaiseFocus() {
            bool hasFocus = focused != null;
            focusChanged.Raise( new FocusInfo {
                HasFocus = hasFocus,
                Prompt = hasFocus ? focused.Prompt : string.Empty,
                HoldTime = hasFocus ? focused.HoldTime : 0f
            } );
        }

        // E pressed. Instant things happen now; timed things start filling the bar.
        private void BeginInteract() {
            if ( focused == null ) { return; }
            if ( focused.HoldTime <= 0f ) { Use( focused ); return; }
            holding = true;
            holdTimer = 0f;
        }

        // E released, or the player looked away.
        private void CancelInteract() {
            holding = false;
            holdTimer = 0f;
            interactProgress.Raise( 0f );
        }

        private void TickHold( float dt ) {
            holdTimer += dt;
            float progress = Mathf.Clamp01( holdTimer / focused.HoldTime );
            interactProgress.Raise( progress );
            if ( progress < 1f ) { return; }

            IInteractable target = focused;
            CancelInteract();
            Use( target );
        }

        private void Use( IInteractable target ) {
            target.Interact( this );
            // Using something can change its prompt ("Open" becomes "Close") or remove it (a pickup),
            // so look again and tell the UI what is in front of the player now.
            SetFocus( FindFocus() );
        }
    }
}
