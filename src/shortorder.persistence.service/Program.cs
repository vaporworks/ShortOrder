using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using shortorder.messages;
using Symbiote.Core;
using Symbiote.Daemon;
using Symbiote.Messaging;
using Symbiote.Rabbit;

namespace shortorder.persistence.service
{
    class Program
    {
        static void Main(string[] args)
        {
            Assimilate.Initialize();
        }
    }

    public class Initializer : IMinion
    {
        public void Initialize()
        {
            Assimilate
                .Initialize()
                .Daemon( x => x.Name( "shortorder.persistenceservice ) )" ) )
                .Rabbit( x => x.AddBroker( b => b.Defaults() ) )
                .RunDaemon();
        }
    }

    public class HandleOrderCreated : IHandle<OrderCreated>
    {
        public Action<IEnvelope> Handle( OrderCreated message )
        {
            
            return x => x.Acknowledge();
        }
    }
}
