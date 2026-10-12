using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TroUble.Data;
using TroUble.Events;
using TroUble.Input;
using TroUble.Player;

namespace TroUble.Core {
    /// <summary>
    /// DEMO ONLY. Runs the demo's rules: caught means a short freeze and then the scene starts over, getting into
    /// hiding with the cookie unnoticed means you win, and R starts over at any time.
    /// GameManager, ProgressionManager and ConsequenceSystem replace this later.
    /// </summary>
    public class DemoDirector : MonoBehaviour {
        [SerializeField] private InputReader input;
        [SerializeField] private PlayerAnchor player;
        [SerializeField] private CatchChannel playerCaught;
        [SerializeField] private DetectionChannel playerDetected;
        [SerializeField] private VoidEventChannel demoWon;
        [SerializeField] private float restartDelay = 2f;
        [SerializeField] private float safeTime = 1.5f;

        private float awareness;
        private float safeTimer;
        private bool over;

        private void OnEnable() {
            playerCaught.Subscribe( OnCaught );
            playerDetected.Subscribe( OnDetected );
            input.RestartPressed += StartOver;
        }

        private void OnDisable() {
            playerCaught.Unsubscribe( OnCaught );
            playerDetected.Unsubscribe( OnDetected );
            input.RestartPressed -= StartOver;
            Time.timeScale = 1f;
        }

        private void Update() {
            if ( over || !player.IsSet ) { return; }

            // Won once the player has sat in hiding with the contraband for a moment and nobody is onto them.
            // If Mom watched them climb in, her awareness keeps rising instead and this never counts up.
            bool safe = player.Interactor.IsHidden && player.Inventory.FirstContraband() != null && awareness <= 0f;
            safeTimer = safe ? safeTimer + Time.deltaTime : 0f;
            if ( safeTimer < safeTime ) { return; }

            over = true;
            Time.timeScale = 0f;
            demoWon.Raise();
        }

        private void OnDetected( DetectionEvent e ) { awareness = e.Awareness; }

        private void OnCaught( CatchReport report ) {
            if ( over ) { return; }
            over = true;
            StartCoroutine( RestartAfterBanner() );
        }

        private IEnumerator RestartAfterBanner() {
            // Time stands still while the banner shows, so this wait has to use real seconds.
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime( restartDelay );
            StartOver();
        }

        private void StartOver() {
            Time.timeScale = 1f;
            SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
        }
    }
}
