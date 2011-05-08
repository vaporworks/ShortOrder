using System.Linq;
using hyperstack;
using rocketsockets;
using Symbiote.Core;
using Symbiote.Daemon;
using Symbiote.Rabbit;

namespace shortorder.http
{
    public class Initializer : IMinion
    {
        public void Initialize()
        {
            DaemonAssimilation.RunDaemon( Assimilate
                                .Initialize()
                                .RocketSockets( x => x.UseDefaultEndpoint() )
                                .HyperStack( x => x
                                                      .RegisterApplications( h =>
                                                      {
                                                    
                                                      } )
                                                      .ConfigureHost( c => c.BasePath( @"..\..\files" ))
                                                      .RegisterApplications( routes => routes
                                                            .DefineApplication<IdService>( request => 
                                                                "uniqueid".Equals( request.PathSegments.FirstOrDefault() ) 
                                                                && request.Method.Equals( "GET" ) )
                                                            .DefineApplication<MenuItemList>( request => 
                                                                "menu".Equals( request.PathSegments.FirstOrDefault() )
                                                                && request.Method.Equals( "GET" ) )
                                                            .DefineApplication<NewMenuItem>( request =>
                                                                "menuitem".Equals( request.PathSegments.FirstOrDefault() ) 
                                                                && request.PathSegments.Count == 2
                                                                && request.Method.Equals( "PUT" ) )
                                                            .DefineApplication<UpdateMenuItem>( request =>
                                                                "menuitem".Equals( request.PathSegments.FirstOrDefault() ) 
                                                                && request.PathSegments.Count == 2
                                                                && request.Method.Equals( "POST" ) )
                                                            .DefineApplication<NewOrder>( request =>
                                                                "order".Equals( request.PathSegments.FirstOrDefault() )
                                                                && request.Method.Equals( "PUT" ) )
                                                            .DefineApplication<OrderList>( request =>
                                                                "order".Equals( request.PathSegments.FirstOrDefault() )
                                                                && request.PathSegments.Count == 1
                                                                && request.Method.Equals( "GET" ) )
                                                            .DefineApplication<OrderStatus>( request =>
                                                                "order".Equals( request.PathSegments.FirstOrDefault() )
                                                                && request.PathSegments.Count == 2
                                                                && request.Method.Equals( "GET" ) )
                                                        )
                                            )
                                .Rabbit( x => x.AddBroker( b => b.Defaults() ).EnrollAsMeshNode( true ) )
                                .Daemon( x => x.Name( "shortstack.http" ) ) );
        }
    }
}