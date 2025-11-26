namespace _PROJECT_.GP.Scripts.Interactables
{
    /// <summary>
    /// Interface for objects that can be interacted with by the player.
    /// </summary>
    public interface IInteractable
    {
        public void InteractIn();
        public void InteractOut();
        public void Interact();
    }
}
