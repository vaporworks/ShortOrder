using System;
using System.Linq;
using hyperstack;
using hyperstack.Owin.Http;
using shortorder.messages;
using Symbiote.Messaging.Impl.Mesh;

namespace shortorder.http
{
    public class OrderStatus : Application
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
                var orderId = Guid.Parse( Request.PathSegments.Last() );
                OrderRank rank = Node.Request<RequestOrderStatus, OrderRank>( new RequestOrderStatus() { Id = orderId }, x => x.CorrelationId = orderId.ToString() );
                Response
                    .Begin( HttpStatus.Ok, headers => headers.ChunkResponse().ContentType( ContentType.Json ) )
                    .AppendJson( rank )
                    .End();
            }
            catch (Exception e)
            {
                Response
                    .Begin( HttpStatus.BadRequest )
                    .End();
                throw;
            }
        }

        public OrderStatus( INode node )
        {
            Node = node;
        }
    }
}