using System;
using System.IO;
using hyperstack;
using hyperstack.Config;
using hyperstack.Owin.Http;
using Symbiote.Core.Collections;

namespace shortorder.http
{
    public class FileServer : Application
    {
        public HttpServerConfiguration Configuration { get; set; }
        public ExclusiveConcurrentDictionary<Tuple<string, DateTime>, byte[]> FileCache { get; set; }

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
            var path = GetFilePathFromRequest();
            var bytes = GetFileBytesForPath( path );
            var fileType = Response.GetContentTypeFromPath( path );

            Response
                .Begin( HttpStatus.Ok, 
                        headers => headers.ContentLength( bytes.Length ).ContentType( fileType ) )
                .AppendFileContentToBody( path )
                .End();
        }

        public string GetFilePathFromRequest() 
        {
            var path = Path.Combine( Configuration.BaseContentPath, Request.RequestUri.Remove(0, 1).Replace( '/', Path.DirectorySeparatorChar ) );
            path = Path.GetFullPath( path );
            if( !File.Exists( path ) ) 
            {
                path = Path.Combine( Configuration.BaseContentPath, "Index.html" );
            }
            return Path.GetFullPath( path );
        }

        public byte[] GetFileBytesForPath( string path ) 
        {
            var fileType = Response.GetContentTypeFromPath( path );
            var file = new FileInfo( path );
            var lastWrite = file.LastWriteTimeUtc;
            var key = Tuple.Create( path, lastWrite );
            return FileCache.ReadOrWrite( key, () => GetBytesFromFile( file ) );
        }

        public byte[] GetBytesFromFile( FileInfo file ) 
        {
            var bytes = new byte[file.Length];
            using( var handle = file.Open( FileMode.Open, FileAccess.Read, FileShare.Read ) ) 
            {
                var buffer = new byte[8*1024];
                var total = 0;  
                var read = 0;
                while( ( read = handle.Read( buffer, 0, buffer.Length ) ) > 0 ) 
                {
                    Buffer.BlockCopy( buffer, 0, bytes, total, read );
                    total += read;
                }
            }
            return bytes;
        }

        public FileServer( HttpServerConfiguration configuration )
        {
            Configuration = configuration;
            Console.WriteLine( "FileServer started" );
            FileCache = new ExclusiveConcurrentDictionary<Tuple<string, DateTime>, byte[]>();
        }
    }
}