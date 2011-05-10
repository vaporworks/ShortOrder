using System;
using System.Text;
using hyperstack;
using hyperstack.Owin.Http;
using Symbiote.Core.Serialization;
using Symbiote.Messaging.Impl.Mesh;

namespace shortorder.http
{
    public class NewMenuItem : Application
    {
        public StringBuilder ResponseBuilder { get; set; }
        public INode Node { get; set; }

        public override bool OnBytes( ArraySegment<byte> bytes, Action readNext )
        {
            ResponseBuilder.Append( Encoding.UTF8.GetString( bytes.Array, bytes.Offset, bytes.Count ) );
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
                var message = ResponseBuilder.ToString().FromJson<messages.AddMenuItem>();
                Node.Publish( message );
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

        public NewMenuItem( INode node )
        {
            Node = node;
            ResponseBuilder = new StringBuilder();
        }
    }
}