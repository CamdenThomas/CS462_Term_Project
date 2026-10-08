using TroUble.Data;
using UnityEngine;

namespace TroUble.Events {
    [CreateAssetMenu( menuName = "TroUble/Channels/Detection" )]
    public class DetectionChannel : EventChannel<DetectionEvent> { }
}
