using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbiote.Daemon;
using Symbiote.Messaging;
using Symbiote.Rabbit;

namespace shortorder.domain.service
{
    public class DomainService : IDaemon
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Bus.AddRabbitChannel( x => x.Fanout( "shortorder.events" ).Durable().PersistentDelivery() );
            Bus.AddRabbitQueue( x => x.QueueName( "shortorder.events" ).ExchangeName( "shortorder.events" ).NoAck().Durable() );
        }

        public void Stop()  
        {
            
        }

        public DomainService( IBus bus )
        {
            Bus = bus;
        }
    }
}
