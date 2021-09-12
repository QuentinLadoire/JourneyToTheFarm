using JTTF.Character;

namespace JTTF.Interface
{
    public interface IOwnable
    {
        public Player OwnerPlayer { get; }

        public void SetOwner(Player owner);
    }
}
