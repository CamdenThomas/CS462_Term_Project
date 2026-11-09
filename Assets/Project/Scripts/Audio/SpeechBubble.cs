using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TroUble.Audio {
    /// <summary>
    /// A speech bubble floating over a character's head. Builds itself in code (no prefab needed), always faces
    /// the camera, and only shows while the character's head is actually in view and not behind a wall, so the
    /// bubble never gives away a parent the player can't see. Their voice still carries through walls.
    /// </summary>
    public class SpeechBubble : MonoBehaviour {
        private const float MaxTextWidth = 360f;
        private const float WorldScale = 0.0035f;    // canvas pixels to metres

        private Canvas canvas;
        private RectTransform background;
        private TextMeshProUGUI label;
        private LayoutElement labelLayout;
        private static readonly RaycastHit[] hits = new RaycastHit[8];

        private Transform owner;
        private LayerMask blockers;
        private float maxShowDistance;
        private bool showing;

        public static SpeechBubble Create( Transform owner, Transform anchor, TMP_FontAsset font, LayerMask blockers, float maxShowDistance ) {
            var go = new GameObject( "SpeechBubble", typeof( RectTransform ) );
            go.transform.SetParent( anchor, false );
            SpeechBubble bubble = go.AddComponent<SpeechBubble>();
            bubble.Build( font );
            bubble.owner = owner;
            bubble.blockers = blockers;
            bubble.maxShowDistance = maxShowDistance;
            return bubble;
        }

        private void Build( TMP_FontAsset font ) {
            canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            var root = (RectTransform) transform;
            root.localScale = Vector3.one * WorldScale;
            root.sizeDelta = new Vector2( MaxTextWidth + 40f, 200f );
            root.pivot = new Vector2( 0.5f, 0f );

            // Cream card with dark text, matching the menus' cream (#FFF4D6) and black (#111111).
            var bg = new GameObject( "Background", typeof( RectTransform ) );
            bg.transform.SetParent( transform, false );
            background = (RectTransform) bg.transform;
            background.anchorMin = background.anchorMax = background.pivot = new Vector2( 0.5f, 0f );
            Image image = bg.AddComponent<Image>();
            image.color = new Color32( 0xFF, 0xF4, 0xD6, 0xF0 );
            var layout = bg.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset( 18, 18, 12, 12 );
            layout.childControlWidth = layout.childControlHeight = true;
            layout.childForceExpandWidth = layout.childForceExpandHeight = false;
            var fitter = bg.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var text = new GameObject( "Text", typeof( RectTransform ) );
            text.transform.SetParent( bg.transform, false );
            label = text.AddComponent<TextMeshProUGUI>();
            if ( font != null ) { label.font = font; }
            label.fontSize = 34f;
            label.color = new Color32( 0x11, 0x11, 0x11, 0xFF );
            label.alignment = TextAlignmentOptions.Center;
            label.textWrappingMode = TextWrappingModes.Normal;
            labelLayout = text.AddComponent<LayoutElement>();

            Hide();
        }

        /// <summary>Lays out the whole line (so the bubble doesn't grow while typing) and shows none of it yet.</summary>
        public void Show( string line ) {
            label.text = line;
            label.maxVisibleCharacters = 0;
            labelLayout.preferredWidth = Mathf.Min( label.GetPreferredValues( line ).x, MaxTextWidth );
            showing = true;
        }

        public void SetVisibleCharacters( int count ) { label.maxVisibleCharacters = count; }

        public void Hide() {
            showing = false;
            canvas.enabled = false;
        }

        private void LateUpdate() {
            if ( !showing ) { return; }
            Camera cam = Camera.main;
            if ( cam == null ) { canvas.enabled = false; return; }

            transform.rotation = cam.transform.rotation;
            canvas.enabled = InView( cam );
        }

        private bool InView( Camera cam ) {
            Vector3 head = transform.position;
            Vector3 viewport = cam.WorldToViewportPoint( head );
            if ( viewport.z <= 0f || viewport.x < 0f || viewport.x > 1f || viewport.y < 0f || viewport.y > 1f ) { return false; }
            if ( viewport.z > maxShowDistance ) { return false; }
            if ( blockers == 0 ) { return true; }

            // Behind a wall? The character's own collider doesn't count.
            Vector3 from = cam.transform.position;
            Vector3 to = head - from;
            int count = Physics.RaycastNonAlloc( from, to.normalized, hits, to.magnitude, blockers, QueryTriggerInteraction.Ignore );
            for ( int i = 0; i < count; i++ ) {
                if ( !hits[i].collider.transform.IsChildOf( owner ) ) { return false; }
            }
            return true;
        }
    }
}
