using UnityEngine;
using UnityEngine.SceneManagement;

namespace TroUble.Core {
    /// <summary>
    /// Loads another scene on top of this one as soon as this scene starts. House uses it to pull in GameUI,
    /// so the HUD and menus live in their own scene file. If that scene is already open (both open in the
    /// editor when you press Play) it is left alone, so you never get two copies.
    /// The scene must be listed in Build Settings.
    /// </summary>
    public class AdditiveSceneLoader : MonoBehaviour {
        [SerializeField] private string sceneName = "GameUI";

        private void Awake() {
            if ( SceneManager.GetSceneByName( sceneName ).isLoaded ) { return; }
            SceneManager.LoadScene( sceneName, LoadSceneMode.Additive );
        }
    }
}
