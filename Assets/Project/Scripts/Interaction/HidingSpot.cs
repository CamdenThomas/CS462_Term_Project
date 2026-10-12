using UnityEngine;

namespace TroUble.Interaction {
    /// <summary>
    /// A place to tuck into: a vent, a box, a closet. Using it puts you in; using it again lets you out.
    /// With a max hide time it pushes you out when the time is up (the bathroom, later).
    /// </summary>
    public class HidingSpot : MonoBehaviour, IInteractable {
        [SerializeField] private Transform viewPoint;   // where the eyes go, facing the way it faces
        [SerializeField] private Transform exitPoint;   // where the feet land on the way out
        [SerializeField] private string enterPrompt = "Hide";
        [SerializeField] private string exitPrompt = "Climb out";
        [SerializeField] private float holdTime = 0f;
        [SerializeField] private float maxHideTime = 0f;    // 0 = stay as long as you like

        private IInteractor occupant;
        private float timer;

        public bool Occupied => occupant != null;
        public string Prompt => Occupied ? exitPrompt : enterPrompt;
        public float HoldTime => Occupied ? 0f : holdTime;

        // Only the one inside can use it while it is taken.
        public bool CanInteract( IInteractor who ) { return !Occupied || occupant == who; }

        public void Interact( IInteractor who ) {
            if ( Occupied ) { ForceExit(); return; }
            occupant = who;
            timer = 0f;
            who.SetHidden( true, viewPoint );
        }

        private void Update() {
            if ( !Occupied || maxHideTime <= 0f ) { return; }
            timer += Time.deltaTime;
            if ( timer >= maxHideTime ) { ForceExit(); }
        }

        private void ForceExit() {
            IInteractor who = occupant;
            occupant = null;
            who.SetHidden( false, exitPoint );
        }
    }
}
