using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EuphoriaCasus.Models
{
    [Serializable()]
    [XmlRoot("Location")]
    public class Location
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Latitude")]
        public double Latitude { get; set; }

        [XmlElement("Longitude")]
        public double Longitude { get; set; }

        [XmlArray("Distances")]
        [XmlArrayItem("Distance")]
        public List<Distance> Distances { get; set; }
    }
}