using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace LinqToolkit.SimpleQuery {

    public static class SimpleQueryOptionsExtentions {
        public static string Transform(
            this SimpleQueryOptions source,
            XslCompiledTransform transform
            ) {
            return
                source.Transform(
                    transform,
                    new XmlWriterSettings() {
                        ConformanceLevel = ConformanceLevel.Fragment,
                        Indent = false
                    }
                    );
        }
        public static string Transform(
            this SimpleQueryOptions source, 
            XslCompiledTransform transform, 
            XmlWriterSettings settings
            ) {
            if ( source == null ) {
                throw new ArgumentNullException( "source" );
            }
            if ( transform == null ) {
                throw new ArgumentNullException( "transform" );
            }
            var serializer = new XmlSerializer( typeof( SimpleQueryOptions ) );
            var serializeWriter = new StringWriter();
            serializer.Serialize( serializeWriter, source );
            var resultWriter = new StringWriter();
            transform.Transform(
                XmlReader.Create( new StringReader( serializeWriter.ToString() ) ),
                XmlWriter.Create( resultWriter, settings )
                );
            return resultWriter.ToString();
        }
    }
}
