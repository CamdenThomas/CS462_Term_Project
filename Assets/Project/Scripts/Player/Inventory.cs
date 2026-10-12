using System.Collections.Generic;
using UnityEngine;
using TroUble.Data;
using TroUble.Events;
using TroUble.Interaction;

namespace TroUble.Player {
    /// <summary>
    /// What the player is carrying. Raises InventoryChanged whenever an item is added or removed.
    /// </summary>
    public class Inventory : MonoBehaviour, IItemHolder {
        [SerializeField] private int capacity = 4;
        [SerializeField] private InventoryChannel inventoryChanged;
        [SerializeField] private List<ItemData> items = new();

        public IReadOnlyList<ItemData> Items => items;

        public bool Add( ItemData item ) {
            if ( item == null || items.Count >= capacity ) { return false; }
            items.Add( item );
            inventoryChanged.Raise( items );
            return true;
        }

        public bool Remove( ItemData item ) {
            if ( !items.Remove( item ) ) { return false; }
            inventoryChanged.Raise( items );
            return true;
        }

        public bool Has( ItemData item ) { return items.Contains( item ); }
    }
}
