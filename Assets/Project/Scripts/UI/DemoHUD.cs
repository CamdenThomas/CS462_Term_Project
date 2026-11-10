using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TroUble.Data;
using TroUble.Events;
using TroUble.Player;

namespace TroUble.UI {
    /// <summary>
    /// DEMO ONLY. The demo's on-screen messages: the two demo tasks on the HUD's sticky note, what you are carrying,
    /// and the caught and win banners. It only listens; DemoDirector decides when the demo is won or restarts.
    /// The sticky note lives in the GameUI scene, which loads a moment after House, so the tasks are pushed to it
    /// as soon as it shows up (and again whenever one gets crossed off).
    /// </summary>
    public class DemoHUD : MonoBehaviour {
        [SerializeField] private CatchChannel playerCaught;
        [SerializeField] private VoidEventChannel demoWon;
        [SerializeField] private InventoryChannel inventoryChanged;

        [SerializeField] private GameObject caughtBanner;
        [SerializeField] private GameObject wonBanner;
        [SerializeField] private TMP_Text objective;
        [SerializeField] private GameObject carryingRoot;
        [SerializeField] private TMP_Text carrying;

        [Header( "Sticky note tasks" )]
        [SerializeField] private PlayerAnchor player;
        [SerializeField] private string taskSteal = "Steal a cookie";
        [SerializeField] private string taskHide = "Hide with it (pantry box or vent)";

        [SerializeField, TextArea] private string goalGetIt = "Sneak a cookie out of the kitchen cabinet.";
        [SerializeField, TextArea] private string goalHide = "Hide before Mom sees you with it. Try the vent under the stairs.";

        private bool stole;
        private bool hid;
        private HUD shownOn;
        private bool noteChanged = true;

        private void OnEnable() {
            playerCaught.Subscribe( OnCaught );
            demoWon.Subscribe( OnWon );
            inventoryChanged.Subscribe( OnInventory );
            caughtBanner.SetActive( false );
            wonBanner.SetActive( false );
            carryingRoot.SetActive( false );
            objective.text = goalGetIt;
        }

        private void OnDisable() {
            playerCaught.Unsubscribe( OnCaught );
            demoWon.Unsubscribe( OnWon );
            inventoryChanged.Unsubscribe( OnInventory );
        }

        private void OnCaught( CatchReport report ) { caughtBanner.SetActive( true ); }

        private void OnWon() { wonBanner.SetActive( true ); }

        private void OnInventory( List<ItemData> items ) {
            bool hasContraband = false;
            List<string> names = new();
            foreach ( ItemData item in items ) {
                names.Add( item.DisplayName );
                if ( item.IsContraband ) { hasContraband = true; }
            }
            carryingRoot.SetActive( names.Count > 0 );
            carrying.text = $"Carrying: {string.Join( ", ", names )}";
            objective.text = hasContraband ? goalHide : goalGetIt;
            if ( hasContraband && !stole ) { stole = true; noteChanged = true; }
        }

        private void Update() {
            // Hiding only counts once you have the cookie on you, the same rule DemoDirector wins on.
            if ( stole && !hid && player != null && player.IsSet && player.Interactor.IsHidden
                 && player.Inventory.FirstContraband() != null ) {
                hid = true;
                noteChanged = true;
            }

            HUD hud = HUD.Instance;
            if ( hud == null || ( hud == shownOn && !noteChanged ) ) { return; }
            hud.SetTasks( new[] { taskSteal, taskHide }, new[] { stole, hid } );
            shownOn = hud;
            noteChanged = false;
        }
    }
}
