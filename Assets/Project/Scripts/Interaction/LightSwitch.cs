using UnityEngine;
using TroUble.Data;

namespace TroUble.Interaction {
    /// <summary>
    /// A wall switch or a lamp: press E to turn its lights on or off. The fixture meshes stop glowing while off, and
    /// anything that animates the bulb (a FlickerLight) is paused with it.
    /// The switch reads its state from its first light, so two switches wired to the same lights (bottom and top of
    /// the stairs) always agree. The state the lights have in the scene is how they start.
    /// </summary>
    public class LightSwitch : MonoBehaviour, IInteractable {
        private static readonly int EmissionId = Shader.PropertyToID( "_EmissionColor" );

        [SerializeField] private Light[] lights;
        [Tooltip( "Bulb or fixture meshes that glow while the light is on." )]
        [SerializeField] private Renderer[] fixtures;
        [Tooltip( "Scripts that drive the bulb (e.g. FlickerLight). Paused while the light is off." )]
        [SerializeField] private Behaviour[] drivers;
        [Tooltip( "Optional. The rocker that tips up for on and down for off." )]
        [SerializeField] private Transform rocker;
        [SerializeField] private float rockerAngle = 14f;
        [Tooltip( "What it is, for the prompt: \"light\" or \"lamp\"." )]
        [SerializeField] private string noun = "light";
        [SerializeField] private SfxCue clickSound;

        private MaterialPropertyBlock block;
        private bool shownOn;

        public bool IsOn => lights != null && lights.Length > 0 && lights[0] != null && lights[0].enabled;
        public string Prompt => IsOn ? $"Turn off the {noun}" : $"Turn on the {noun}";
        public float HoldTime => 0f;

        private void Awake() {
            block = new MaterialPropertyBlock();
            Apply( IsOn );
        }

        // Another switch on the same lights may have flipped them: keep this rocker and glow in step.
        private void Update() {
            if ( IsOn != shownOn ) { Apply( IsOn ); }
        }

        public bool CanInteract( IInteractor who ) { return lights != null && lights.Length > 0; }

        public void Interact( IInteractor who ) {
            Apply( !IsOn );
            if ( clickSound != null ) { clickSound.PlayAt( transform.position ); }
        }

        private void Apply( bool on ) {
            shownOn = on;
            // Drivers first: FlickerLight restores its glow when disabled, and the fixture setting below must win.
            foreach ( Behaviour driver in drivers ) { if ( driver != null ) { driver.enabled = on; } }
            foreach ( Light light in lights ) { if ( light != null ) { light.enabled = on; } }
            foreach ( Renderer fixture in fixtures ) {
                if ( fixture == null ) { continue; }
                if ( on ) {
                    fixture.SetPropertyBlock( null );
                } else {
                    fixture.GetPropertyBlock( block );
                    block.SetColor( EmissionId, Color.black );
                    fixture.SetPropertyBlock( block );
                }
            }
            if ( rocker != null ) { rocker.localRotation = Quaternion.Euler( on ? -rockerAngle : rockerAngle, 0f, 0f ); }
        }
    }
}
