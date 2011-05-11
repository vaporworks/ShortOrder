using Symbiote.Core;
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
                .Rabbit( x => x.AddBroker( b => b.Defaults() ).EnrollAsMeshNode( false ) )
                .Couch( x => x.Server( "localhost" ) )
                .AddConsoleLogger<DomainService>( x => x.Debug().MessageLayout( m => m.Message().Newline() ) )
                .RunDaemon();
        }
    }
}