using System.Collections.Generic;
using TroUble.Data;
using UnityEngine;

namespace TroUble.Events {
    [CreateAssetMenu( menuName = "TroUble/Channels/Task List" )]
    public class TaskListChannel : EventChannel<List<TaskStatus>> { }
}
