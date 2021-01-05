using System.Xml.Serialization;

namespace Tianyi.Soap
{
    [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SoapRoot<T>
    {
        [XmlElement("Body",Namespace ="")]
        public T Body { get; set; }
    }
}
