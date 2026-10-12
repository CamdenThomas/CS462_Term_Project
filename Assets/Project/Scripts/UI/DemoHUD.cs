using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TroUble.Data;
using TroUble.Events;

namespace TroUble.UI {
    /// <summary>
    /// DEMO ONLY. The demo's on-screen messages: the goal, what you are carrying, and the caught and win banners.
    /// It only listens to channels; DemoDirector decides when the demo is won or restarts.
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

        [SerializeField, TextArea] private string goalGetIt = "Sneak a cookie out of the kitchen cabinet.";
        [SerializeField, TextArea] private string goalHide = "Hide before Mom sees you with it. Try the vent under the stairs.";

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
        }
    }
}
