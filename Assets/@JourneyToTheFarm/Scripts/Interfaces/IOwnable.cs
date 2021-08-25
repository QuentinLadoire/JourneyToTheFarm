
namespace JTTF
{
    public interface IOwnable
    {
        public Player OwnerPlayer { get; }

        public void SetOwner(Player owner);
    }
}
