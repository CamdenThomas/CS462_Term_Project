using TroUble.Data;
using UnityEngine;

namespace TroUble.Events {
    [CreateAssetMenu( menuName = "TroUble/Channels/Clock" )]
    public class ClockChannel : EventChannel<ClockTime> { }
}
