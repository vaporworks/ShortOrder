using Relax;
using shortoder.domain;
using Symbiote.Core;

namespace shortorder.domain.service
{
    public class MenuItemMemento : 
        CouchDocument,
        IMemento<MenuItem>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public void Capture( MenuItem instance )
        {
            Id = instance.Id;
            Name = instance.Name;
            Description = instance.Description;
        }

        public void Reset( MenuItem instance )
        {
            instance.Id = Id;
            instance.Name = Name;
            instance.Description = Description;
        }

        public MenuItem Retrieve()
        {
            return new MenuItem()
            {
                Id = Id,
                Name = Name,
                Description = Description
            };
        }
    }
}