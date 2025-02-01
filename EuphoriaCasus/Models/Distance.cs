using System;
using System.Xml.Serialization;

namespace EuphoriaCasus.Models
{
    [Serializable()]
    public class Distance
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Km")]
        public double Km { get; set; }

        [XmlElement("Seconds")]
        public int Seconds { get; set; }
    }
}