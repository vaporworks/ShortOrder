using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbiote.Core;
using Symbiote.Core.Extensions;
using Symbiote.Daemon;
using Symbiote.Log4Net;

namespace shortorder.host
{
    public class Program
    {
        static void Main(string[] args)
        {
            Assimilate
                .Initialize()
                .Daemon( x => x.Name( "shortorder.host" ).AsDynamicHost( b => b.HostApplicationsFrom( @"..\..\..\..\services\" ) ) )
                .AddConsoleLogger<HostService>( x => x.Debug().MessageLayout( m => m.Message().Newline() ) )
                .RunDaemon();
        }
    }

    public class HostService : IDaemon
    {
        public void Start()
        {
            "Starting short-order host."
                .ToInfo<HostService>();
        }

        public void Stop()
        {
            "Stopping short-order host."
                .ToInfo<HostService>();
        }
    }
}
