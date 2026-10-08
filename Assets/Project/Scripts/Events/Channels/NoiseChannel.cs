using TroUble.Data;
using UnityEngine;

namespace TroUble.Events {
    [CreateAssetMenu( menuName = "TroUble/Channels/Noise" )]
    public class NoiseChannel : EventChannel<NoiseEvent> { }
}
