using System;
using System.Text;
using hyperstack;
using hyperstack.Owin.Http;
using shortorder.messages;
using Symbiote.Core.Serialization;
using Symbiote.Messaging.Impl.Mesh;

namespace shortorder.http
{
    public class NewOrder : Application
    {
        public StringBuilder ResponseBuilder { get; set; }
        public INode Node { get; set; }

        public override bool OnBytes( ArraySegment<byte> bytes, Action readNext )
        {
            ResponseBuilder.Append( Encoding.UTF8.GetString( bytes.Array, bytes.Offset, bytes.Array.Length - bytes.Offset ) );
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
                var message = ResponseBuilder.ToString().FromJson<CreateOrder>();
                Node.Publish( message, x => x.CorrelationId = message.Id.ToString() );
                Response
                    .Begin( HttpStatus.Ok )
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

        public NewOrder( INode node ) 
        {
            Node = node;
            ResponseBuilder = new StringBuilder();
        }
    }
}