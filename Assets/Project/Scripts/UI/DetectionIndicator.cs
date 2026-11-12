using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TroUble.Data;
using TroUble.Events;

namespace TroUble.UI {
    /// <summary>
    /// The "is someone noticing me" marker. A "?" that fills while a watcher is getting suspicious, and a "!" once
    /// the player is spotted. Once the HUD (GameUI scene) has loaded, the marker moves inside its Suspicion eye so the
    /// two read as one indicator, and the same awareness fills the eye.
    /// </summary>
    public class DetectionIndicator : MonoBehaviour {
        [SerializeField] private DetectionChannel playerDetected;
        [SerializeField] private GameObject indicatorRoot;
        [SerializeField] private TMP_Text symbol;
        [SerializeField] private Image fill;
        [SerializeField] private Color suspiciousColor = new Color( 0.976f, 0.745f, 0.227f );
        [SerializeField] private Color spottedColor = new Color( 0.855f, 0.145f, 0.114f );

        [Header( "Sits inside the HUD's Suspicion eye" )]
        [SerializeField] private bool dockInSuspicionEye = true;
        [Tooltip( "Height of the marker once it is inside the eye, in canvas units." )]
        [SerializeField] private float dockedSize = 62f;

        private HUD dockedIn;
        private readonly Dictionary<Transform, DetectionEvent> watching = new();

        private void OnEnable() {
            playerDetected.Subscribe( OnDetected );
            indicatorRoot.SetActive( false );
        }

        private void OnDisable() { playerDetected.Unsubscribe( OnDetected ); }

        // The HUD lives in the GameUI scene, which loads a moment after this one, so dock as soon as it appears.
        private void Update() {
            HUD hud = HUD.Instance;
            if ( !dockInSuspicionEye || hud == null || hud == dockedIn || hud.suspicionFill == null ) { return; }
            var marker = (RectTransform)indicatorRoot.transform;
            var eye = hud.suspicionFill.rectTransform;
            float height = marker.rect.height > 0f ? marker.rect.height : dockedSize;
            marker.SetParent( eye.parent, false );
            marker.anchorMin = marker.anchorMax = eye.anchorMin;
            marker.anchoredPosition = eye.anchoredPosition;
            marker.localScale = Vector3.one * ( dockedSize / height );
            marker.SetAsLastSibling();
            dockedIn = hud;
        }

        // With two parents, one calming down must not hide the other one noticing you, so remember what each watcher
        // last reported and always show the most aware of them.
        private void OnDetected( DetectionEvent latest ) {
            if ( latest.Watcher != null ) {
                if ( latest.Level == DetectionLevel.None ) { watching.Remove( latest.Watcher ); } else { watching[latest.Watcher] = latest; }
            }
            DetectionEvent e = latest.Watcher != null ? default : latest;
            foreach ( var pair in watching ) {
                if ( pair.Key != null && pair.Value.Awareness > e.Awareness ) { e = pair.Value; }
            }

            if ( HUD.Instance != null ) { HUD.Instance.SetSuspicion( e.Level == DetectionLevel.None ? 0f : e.Awareness * 100f ); }
            indicatorRoot.SetActive( e.Level != DetectionLevel.None );
            if ( e.Level == DetectionLevel.None ) { return; }

            bool isSpotted = e.Level == DetectionLevel.Spotted;
            symbol.text = isSpotted ? "!" : "?";
            fill.fillAmount = e.Awareness;
            fill.color = isSpotted ? spottedColor : suspiciousColor;
        }
    }
}
