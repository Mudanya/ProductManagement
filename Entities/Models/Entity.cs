using Entities.LinkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Entities.Models
{
    public class Entity
    {
        public Entity()
        {

        }
        private void WriteLinksToXml(string key, Object value, XmlWriter writer)
        {
            writer.WriteStartElement(key);
            if(value == typeof(List<Link>))
            {
                foreach(var val in value as List<Link>)
                {
                    writer.WriteStartElement(nameof(Link));
                    WriteLinksToXml(nameof(val.HRef),val.HRef, writer);
                    WriteLinksToXml(nameof(val.Rel),val.Rel, writer);
                    WriteLinksToXml(nameof(val.Method),val.Method, writer);
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteString(value.ToString());
            }
            writer.WriteEndElement();
        }
    }
}
