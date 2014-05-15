using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CubeWorldGameEngine.Helpers;

namespace CubeWorldGameEngine.Helpers
{
    [Serializable]
    
    public class ResoursesPacksInfo
    {
        [XmlAttribute] 
        public int ID { get; set; }

        //[XmlElement("Name")]
        public string Name { get; set; }

        [XmlIgnore]
        public string FileName  { get; set; }

        //свойства текстур
        //[XmlElement("TextureFileNameInfo")]
        public List<ResoursesInfo> ResoursesList  { get; set; }

        [XmlIgnore]
        public List<TexturesList> TexturesInfoList { get; set; }
        
        [XmlIgnore]
        public List<BaseBlocksList> BaseBlocksInfoList { get; set; }

        public override string ToString()
        {
            return ID + " " + Name;
        }

        public ResoursesPacksInfo()
        {
            Name = "";
            FileName = "";
            ID = 0;
            TexturesInfoList = new List<TexturesList>();
            BaseBlocksInfoList = new List<BaseBlocksList>();
            ResoursesList = new List<ResoursesInfo>();
        }

 
    }


}
