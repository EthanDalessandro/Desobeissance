using UnityEngine;

namespace _PROJECT_.GP.Scripts.Interactables
{
    /// <summary>
    /// Base class for all interactable objects.
    /// Centralizes interaction settings (Type, Hold Duration, Spam Count).
    /// </summary>
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Interaction Settings")]
        [SerializeField, Tooltip("Type of interaction required.")]
        protected InteractionType _interactionType = InteractionType.Simple;

        [SerializeField, Tooltip("Duration in seconds for Hold interaction.")]
        protected float _holdDuration = 1f;

        [SerializeField, Tooltip("Number of presses for Spam interaction.")]
        protected int _spamCount = 5;
        public InteractionType InteractionType => _interactionType;
        public float HoldDuration => _holdDuration;
        public int SpamCount => _spamCount;
        public bool CanInteract { get; set; }


        /// <summary>
        /// Called when the player looks at or highlights the object.
        /// </summary>
        public abstract void InteractIn();

        /// <summary>
        /// Called when the player looks away or stops highlighting the object.
        /// </summary>
        public abstract void InteractOut();

        /// <summary>
        /// Called when the interaction is successfully completed.
        /// </summary>
        public abstract void Interact();
    }
}
