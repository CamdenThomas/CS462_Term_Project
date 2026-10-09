using System;
using System.Collections.Generic;

namespace TroUble.Data {
    /// <summary>Payload of FavorMenuOpened. The UI calls OnChosen, so it never references BrotherAI.</summary>
    [System.Serializable]
    public struct FavorMenuRequest {
        public List<FavorData> Options;
        public Attitude Attitude;
        public Action<FavorData> OnChosen;
    }
}
