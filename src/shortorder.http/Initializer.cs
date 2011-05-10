using System.Collections.Generic;
using System.Linq;
using hyperstack;
using hyperstack.Owin.Http;
using hyperstack.Owin.Impl;
using rocketsockets;
using rocketsockets.Config;
using Symbiote.Core;
using Symbiote.Daemon;
using Symbiote.Rabbit;
using Symbiote.Log4Net;

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
                                                      .ConfigureHost( c => c
                                                          .BasePath( @"..\..\..\WebClientOrderForm" )
                                                          .AddViewSearchFolder( "template" )
                                                      )
                                                      .RegisterApplications( routes => routes
                                                            .DefineApplication( request => request.RequestUri.EndsWith( "fav.ico" ), ( request, response, exception ) => 
                                                            {
                                                                response( HttpStatus.NoContent.ToString(), new Dictionary<string, string>(), ( bytes, error, continuation ) => () => { } ); 
                                                            } )
                                                            .DefineApplication<FileServer>( request =>
                                                            {
                                                                var last = request.PathSegments.LastOrDefault() ?? "";
                                                                return last.Contains( "." );
                                                            } )
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
                                                            .DefineApplication<FileServer>( request => 
                                                                request.PathSegments.Count == 0 )
                                                        )
                                            )
                                .AddConsoleLogger<HostService>( x => x.Debug().MessageLayout( m => m.Message().Newline() ) )
                                .Rabbit( x => x.AddBroker( b => b.Defaults() ).EnrollAsMeshNode( true ) )
                                .Daemon( x => x.Name( "shortstack.http" ) ) );
        }
    }
}