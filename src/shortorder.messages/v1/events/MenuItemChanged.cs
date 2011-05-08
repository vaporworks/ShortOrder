using Symbiote.Core.UnitOfWork;

namespace shortorder.messages
{
    public class MenuItemChanged : EventBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}