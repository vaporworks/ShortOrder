using System;
using hyperstack;
using hyperstack.Owin.Http;
using shortorder.messages;
using Symbiote.Messaging.Impl.Mesh;

namespace shortorder.http
{
    public class OrderList : Application
    {
        public INode Node { get; set; }

        public override bool OnBytes( ArraySegment<byte> bytes, Action readNext )
        {
            return false;
        }

        public override void OnError( Exception exception )
        {
            Response
                .Begin( HttpStatus.InternalServerError )
                .End();
        }

        public override void OnComplete()
        {
            try 
            {
                var list = Node.Request<RequestOrderList, OrderList>( new RequestOrderList() { ActiveOnly = true } );
                Response
                    .Begin( HttpStatus.Ok, headers => headers.ChunkResponse().ContentType( ContentType.Json ) )
                    .AppendJson( list )
                    .End();
            }
            catch (Exception)
            {
                Response
                    .Begin( HttpStatus.BadRequest )
                    .End();
                throw;
            }
        }

        public OrderList( INode node )
        {
            Node = node;
        }
    }
}