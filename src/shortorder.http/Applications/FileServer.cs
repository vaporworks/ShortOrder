using System;
using System.IO;
using hyperstack;
using hyperstack.Config;
using hyperstack.Owin.Http;

namespace shortorder.http
{
    public class FileServer : Application
    {
        public HttpServerConfiguration Configuration { get; set; }

        public override bool OnBytes( ArraySegment<byte> bytes, Action readNext )
        {
            return false;
        }

        public override void OnError( Exception exception )
        {
            Response
                .Begin( HttpStatus.NotFound )
                .End();
        }

        public override void OnComplete()
        {
            var path = Path.Combine( Configuration.BaseContentPath, Request.RequestUri.Remove(0, 1).Replace( '/', Path.DirectorySeparatorChar ) );
            path = Path.GetFullPath( path );
            if( !File.Exists( path ) ) 
            {
                path = Path.Combine( Configuration.BaseContentPath, "Index.html" );
            }
            path = Path.GetFullPath( path );

            var fileType = Response.GetContentTypeFromPath( path );
            var file = new FileInfo( path );
            var length = file.Length;

            Response
                .Begin( HttpStatus.Ok, 
                        headers => headers.ContentLength( length ).ContentType( fileType ) )
                .AppendFileContentToBody( path )
                .End();
        }

        public FileServer( HttpServerConfiguration configuration )
        {
            Configuration = configuration;
        }
    }
}