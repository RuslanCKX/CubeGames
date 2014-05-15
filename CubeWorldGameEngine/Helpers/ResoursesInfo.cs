using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CubeWorldGameEngine.Helpers
{
    [Serializable]
    public class ResoursesInfo
    {
        [XmlAttribute]
        public EngineMain.Resourses ResoursesType { get; set; }
        [XmlAttribute]
        public int IDResourses { get; set; } 
        public string Path { get; set; }
        
        public override string ToString()
        {
            return ResoursesType + " --> ID: " + IDResourses + "; Path: " + Path;
        }

        public ResoursesInfo()
        {
        }
    }
}
