using TroUble.Data;
using UnityEngine;

namespace TroUble.Events {
    [CreateAssetMenu( menuName = "TroUble/Channels/Task" )]
    public class TaskChannel : EventChannel<TaskData> { }
}
