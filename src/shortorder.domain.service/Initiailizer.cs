using System.Configuration;
using shortorder.domain.service.support;
using Symbiote.Core;
using Symbiote.Core.Actor;
using Symbiote.Daemon;
using Symbiote.Rabbit;
using Relax;
using Symbiote.Log4Net;

namespace shortorder.domain.service
{
    public class Initiailizer : IMinion
    {
        public void Initialize()
        {
            Assimilate
                .Initialize()
                .Daemon( x => x.Name( "shortorder.domain.service" ) )   
                //.Dependencies( x => x.For( typeof( IActorStore<> ) ).Use( typeof( CouchActorStore<> ) ) )
                .Rabbit( x => x.AddBroker( b => b.Defaults().Address( ConfigurationManager.AppSettings["RabbitHost"] ) ).EnrollAsMeshNode( false ) )
                .Couch(x => x.Server(ConfigurationManager.AppSettings["CouchHost"]))
                .AddConsoleLogger<DomainService>( x => x.Debug().MessageLayout( m => m.Message().Newline() ) )
                .RunDaemon();
        }
    }
}