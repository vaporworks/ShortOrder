using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbiote.Core.UnitOfWork;

namespace shortorder.messages
{
    public class MenuItemAdded : EventBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
