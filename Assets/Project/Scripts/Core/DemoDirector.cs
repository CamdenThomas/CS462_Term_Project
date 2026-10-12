using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TroUble.Data;
using TroUble.Events;

namespace TroUble.Core {
    /// <summary>
    /// DEMO ONLY. Runs the demo's rules. For now: when the player is caught, freeze for a moment so the banner
    /// can be read, then start the scene over. GameManager and ConsequenceSystem replace this later.
    /// </summary>
    public class DemoDirector : MonoBehaviour {
        [SerializeField] private CatchChannel playerCaught;
        [SerializeField] private float restartDelay = 2f;

        private bool restarting;

        private void OnEnable() { playerCaught.Subscribe( OnCaught ); }

        private void OnDisable() {
            playerCaught.Unsubscribe( OnCaught );
            Time.timeScale = 1f;
        }

        private void OnCaught( CatchReport report ) {
            if ( restarting ) { return; }
            restarting = true;
            StartCoroutine( Restart() );
        }

        private IEnumerator Restart() {
            // Time stands still while the banner shows, so this wait has to use real seconds.
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime( restartDelay );
            Time.timeScale = 1f;
            SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
        }
    }
}
