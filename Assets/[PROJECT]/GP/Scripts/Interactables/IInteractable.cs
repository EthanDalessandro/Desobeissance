namespace _PROJECT_.GP.Scripts.Interactables
{
    /// <summary>
    /// Defines the method of interaction required for an object.
    /// </summary>
    public enum InteractionType
    {
        /// <summary>
        /// Instant interaction upon button press.
        /// </summary>
        Simple,
        /// <summary>
        /// Requires holding the button for a specific duration.
        /// </summary>
        Hold,
        /// <summary>
        /// Requires pressing the button multiple times.
        /// </summary>
        Spam
    }

    /// <summary>
    /// Interface for objects that can be interacted with by the player.
    /// Supports different interaction types (Simple, Hold, Spam).
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// The type of interaction required (Simple, Hold, Spam).
        /// </summary>
        public InteractionType InteractionType { get; }

        /// <summary>
        /// Duration in seconds required for Hold interactions.
        /// </summary>
        public float HoldDuration { get; }

        /// <summary>
        /// Number of presses required for Spam interactions.
        /// </summary>
        public int SpamCount { get; }

        public bool CanInteract { get; set; }

        /// <summary>
        /// Called when the player looks at or highlights the object.
        /// </summary>
        public void InteractIn();

        /// <summary>
        /// Called when the player looks away or stops highlighting the object.
        /// </summary>
        public void InteractOut();

        /// <summary>
        /// Called when the interaction is successfully completed.
        /// </summary>
        public void Interact();
    }
}
