using TroUble.Data;
using UnityEngine;

namespace TroUble.Events {
    [CreateAssetMenu( menuName = "TroUble/Channels/Catch" )]
    public class CatchChannel : EventChannel<CatchReport> { }
}
