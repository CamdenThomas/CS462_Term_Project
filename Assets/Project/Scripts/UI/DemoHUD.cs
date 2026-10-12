using UnityEngine;
using TroUble.Data;
using TroUble.Events;

namespace TroUble.UI {
    /// <summary>
    /// DEMO ONLY. The big messages for the demo. For now just the "CAUGHT!" banner; DemoDirector restarts the scene.
    /// </summary>
    public class DemoHUD : MonoBehaviour {
        [SerializeField] private CatchChannel playerCaught;
        [SerializeField] private GameObject caughtBanner;

        private void OnEnable() {
            playerCaught.Subscribe( OnCaught );
            caughtBanner.SetActive( false );
        }

        private void OnDisable() { playerCaught.Unsubscribe( OnCaught ); }

        private void OnCaught( CatchReport report ) { caughtBanner.SetActive( true ); }
    }
}
