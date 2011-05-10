using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hyperstack;
using Symbiote.Core.Extensions;
using Symbiote.Daemon;

namespace shortorder.http
{
    public class HostService : IDaemon
    {
        public IOwinHost Host { get; set; }

        public void Start()
        {
            "Starting shortoder http server"
                .ToInfo<HostService>();
            Host.Start();
        }

        public void Stop()
        {
            "Stopping shortoder http server"
                .ToInfo<HostService>();
            Host.Stop();
        }

        public HostService( IOwinHost host )
        {
            Host = host;
        }
    }
}
