namespace TroUble.Interaction {
    /// <summary>
    /// Anything the player can press E on. HoldTime 0 means instant; more than 0 means a timed hold.
    /// </summary>
    public interface IInteractable {
        string Prompt { get; }
        float HoldTime { get; }
        bool CanInteract( IInteractor who );
        void Interact( IInteractor who );
    }
}
