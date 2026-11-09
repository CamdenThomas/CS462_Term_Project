using System;
using System.Collections.Generic;
using UnityEngine;

namespace TroUble.Data {
    /// <summary>
    /// Everything one character can say, grouped by what triggers it. Lines may use {first}, {middle} and {last}
    /// for the player's name, so the full-name meter works with whatever the kid is called.
    /// </summary>
    [CreateAssetMenu( menuName = "TroUble/Data/Bark Set" )]
    public class BarkSet : ScriptableObject {
        [Serializable]
        public class Entry {
            public BarkTrigger trigger;
            [TextArea( 1, 3 )] public List<string> lines = new();
        }

        [SerializeField] private List<Entry> entries = new();

        // The last line said per trigger, so the same one never plays twice in a row.
        [NonSerialized] private readonly Dictionary<BarkTrigger, int> lastPicked = new();

        public bool Has( BarkTrigger trigger ) {
            Entry entry = Find( trigger );
            return entry != null && entry.lines.Count > 0;
        }

        /// <summary>A random line for the trigger, avoiding a repeat of the last one. Null if there are none.</summary>
        public string Pick( BarkTrigger trigger ) {
            Entry entry = Find( trigger );
            if ( entry == null || entry.lines.Count == 0 ) { return null; }

            int index = UnityEngine.Random.Range( 0, entry.lines.Count );
            if ( entry.lines.Count > 1 && lastPicked.TryGetValue( trigger, out int last ) && index == last ) {
                index = ( index + 1 ) % entry.lines.Count;
            }
            lastPicked[trigger] = index;
            return entry.lines[index];
        }

        private Entry Find( BarkTrigger trigger ) {
            foreach ( Entry e in entries ) {
                if ( e.trigger == trigger ) { return e; }
            }
            return null;
        }
    }
}
