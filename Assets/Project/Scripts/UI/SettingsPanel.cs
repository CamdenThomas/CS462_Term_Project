using UnityEngine;
using UnityEngine.Events;

namespace TroUble.UI {
    /// <summary>
    /// Sits on the root of the shared Settings prefab. The panel's Back button calls Close, and each menu that
    /// shows the panel hooks onClose up to its own way of going back (MainMenu.Back, PauseMenu.CloseSettings).
    /// That keeps the prefab itself free of any link to the menu it happens to be in.
    /// </summary>
    public class SettingsPanel : MonoBehaviour {
        [SerializeField] private UnityEvent onClose = new UnityEvent();

        public UnityEvent OnClose => onClose;

        public void Close() {
            onClose.Invoke();
        }
    }
}
