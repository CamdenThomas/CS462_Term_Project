using UnityEngine;

namespace TroUble.Interaction {
    /// <summary>
    /// A door or cabinet door. For now it only reports that it was used; swinging comes next.
    /// </summary>
    public class Door : MonoBehaviour, IInteractable {
        [SerializeField] private string prompt = "Open";
        [SerializeField] private float holdTime = 0f;

        public string Prompt => prompt;
        public float HoldTime => holdTime;

        public bool CanInteract( IInteractor who ) { return true; }

        public void Interact( IInteractor who ) { Debug.Log( $"{name} used by {who.Transform.name}", this ); }
    }
}
