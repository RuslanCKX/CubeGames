using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CubeWorldGameEngine.Helpers
{
    [Serializable]
    public class TexturesList
    {
        [XmlAttribute]
        public int ID { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        public List<TexturesInfo> TextureList { get; set; }

        public TexturesList()
        {
            ID = 0;
            Name = "";
            TextureList = new List<TexturesInfo>();
        }
    }
}
