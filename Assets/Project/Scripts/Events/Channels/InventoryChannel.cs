using System.Collections.Generic;
using TroUble.Data;
using UnityEngine;

namespace TroUble.Events {
    [CreateAssetMenu( menuName = "TroUble/Channels/Inventory" )]
    public class InventoryChannel : EventChannel<List<ItemData>> { }
}
