using UnityEngine;
using TroUble.Data;

namespace TroUble.Interaction {
    /// <summary>
    /// An item lying in the world. Using it moves the item into whoever picked it up, then hides this object.
    /// </summary>
    public class ItemPickup : MonoBehaviour, IInteractable {
        [SerializeField] private ItemData item;
        [SerializeField] private SfxCue pickupSound;    // optional

        private bool collected;

        public string Prompt => $"Take {item.DisplayName}";
        public float HoldTime => 0f;

        public bool CanInteract( IInteractor who ) { return !collected; }

        public void Interact( IInteractor who ) {
            // Add returns false when the holder is full, and then the item stays where it is.
            if ( !who.Items.Add( item ) ) { return; }
            collected = true;
            if ( pickupSound != null ) { pickupSound.PlayAt( transform.position ); }
            gameObject.SetActive( false );
        }
    }
}
