using System.Collections.Generic;
using TroUble.Data;

namespace TroUble.Interaction {
    /// <summary>
    /// Anything that holds items: the player's inventory, a stash, the lockbox.
    /// </summary>
    public interface IItemHolder {
        IReadOnlyList<ItemData> Items { get; }
        bool Add( ItemData item );
        bool Remove( ItemData item );
        bool Has( ItemData item );
    }
}
