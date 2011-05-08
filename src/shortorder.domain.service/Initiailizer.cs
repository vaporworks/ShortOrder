using Symbiote.Daemon;

namespace shortorder.domain.service
{
    public class Initiailizer : IMinion
    {
        public void Initialize()
        {
            DaemonAssimilation.RunDaemon( Assimilate
                                .Initialize()
                                .Daemon( x => x.Name( "shortorder.domain.service" ) )
                                .Rabbit( x => x.AddBroker( b => b.Defaults() ).EnrollAsMeshNode( false ) ) );
        }
    }
}