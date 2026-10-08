using TroUble.Data;
using UnityEngine;

namespace TroUble.Events {
    [CreateAssetMenu( menuName = "TroUble/Channels/Game State" )]
    public class GameStateChannel : EventChannel<GameState> { }
}
