using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TroUble.Data;
using TroUble.Events;

namespace TroUble.UI {
    /// <summary>
    /// Shows "E: Open" and the hold bar. Only listens to channels; it never touches the player.
    /// Sits on the HUD canvas, not on the prompt it hides, so it keeps listening while the prompt is off.
    /// </summary>
    public class InteractionPromptUI : MonoBehaviour {
        [SerializeField] private FocusChannel focusChanged;
        [SerializeField] private FloatChannel interactProgress;
        [SerializeField] private GameObject promptRoot;
        [SerializeField] private TMP_Text label;
        [SerializeField] private GameObject holdBar;
        [SerializeField] private Image progressBar;

        private void OnEnable() {
            focusChanged.Subscribe( OnFocus );
            interactProgress.Subscribe( OnProgress );
            promptRoot.SetActive( false );
            progressBar.fillAmount = 0f;
        }

        private void OnDisable() {
            focusChanged.Unsubscribe( OnFocus );
            interactProgress.Unsubscribe( OnProgress );
        }

        private void OnFocus( FocusInfo info ) {
            promptRoot.SetActive( info.HasFocus );
            if ( !info.HasFocus ) { return; }
            label.text = $"E: {info.Prompt}";
            holdBar.SetActive( info.HoldTime > 0f );
        }

        private void OnProgress( float progress ) { progressBar.fillAmount = progress; }
    }
}
