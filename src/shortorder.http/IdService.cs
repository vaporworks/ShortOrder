using System;
using hyperstack;
using hyperstack.Owin.Http;

namespace shortorder.http
{
    public class IdService : Application
    {
        public override bool OnBytes( ArraySegment<byte> bytes, Action readNext )
        {
            return false;
        }

        public override void OnError( Exception exception )
        {   
        }

        public override void OnComplete()
        {
            Response
                .Begin( 
                    HttpStatus.Ok, 
                    headers => headers.ChunkResponse() )
                .AppendJson( Guid.NewGuid() )
                .End();
        }
    }
}