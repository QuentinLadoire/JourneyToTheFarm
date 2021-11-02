using JTTF.Enum;
using JTTF.Character;

namespace JTTF.Interface
{
    public interface IInteractable
    {
        public bool IsInteractable { get; }
        public float ActionDuration { get; }
        public ActionType ActionType { get; }

        public void Select();
        public void Deselect();

        public void StartToInteract();
        public void Interact(Player player);
        public void StopToInteract();
    }
}
