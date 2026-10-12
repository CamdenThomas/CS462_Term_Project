using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TroUble.Data;
using TroUble.Events;

namespace TroUble.UI {
    /// <summary>
    /// The "is someone noticing me" marker at the top of the screen. A "?" that fills while a watcher is getting
    /// suspicious, and a "!" once the player is spotted. Sits on the HUD canvas, not on the marker it hides.
    /// </summary>
    public class DetectionIndicator : MonoBehaviour {
        [SerializeField] private DetectionChannel playerDetected;
        [SerializeField] private GameObject indicatorRoot;
        [SerializeField] private TMP_Text symbol;
        [SerializeField] private Image fill;
        [SerializeField] private Color suspiciousColor = new Color( 0.976f, 0.745f, 0.227f );
        [SerializeField] private Color spottedColor = new Color( 0.855f, 0.145f, 0.114f );

        private void OnEnable() {
            playerDetected.Subscribe( OnDetected );
            indicatorRoot.SetActive( false );
        }

        private void OnDisable() { playerDetected.Unsubscribe( OnDetected ); }

        private void OnDetected( DetectionEvent e ) {
            indicatorRoot.SetActive( e.Level != DetectionLevel.None );
            if ( e.Level == DetectionLevel.None ) { return; }

            bool isSpotted = e.Level == DetectionLevel.Spotted;
            symbol.text = isSpotted ? "!" : "?";
            fill.fillAmount = e.Awareness;
            fill.color = isSpotted ? spottedColor : suspiciousColor;
        }
    }
}
