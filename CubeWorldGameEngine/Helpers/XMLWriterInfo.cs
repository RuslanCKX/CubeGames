using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace CubeWorldGameEngine.Helpers
{
    public class XMLWriterInfo
    {

        public static void WriteResoursesPacksInfo()
        {
            EngineMain.SortAll();

            XmlSerializer resPacksSerializer = new XmlSerializer(typeof(List<ResoursesPacksInfo>));
            
            StreamWriter sw = new StreamWriter(EngineMain.GetFullResoursesPath(EngineMain.ResPacksFileName), false, Encoding.GetEncoding(1251));
            resPacksSerializer.Serialize(sw, EngineMain.ResPacks);
            sw.Close();

            WriteResoursesTexturesInfo();
        }

        public static void WriteResoursesTexturesInfo()
        {
            foreach (ResoursesPacksInfo resoursesPacks in EngineMain.ResPacks)
            {
                foreach (ResoursesInfo resoursesInfo in resoursesPacks.ResoursesList)
                {
                    if (resoursesInfo.ResoursesType == EngineMain.Resourses.Textrures)
                    {
                        XmlSerializer texturelistSerializer = new XmlSerializer(typeof (TexturesList));
                        StreamWriter sw = new StreamWriter(EngineMain.GetFullResoursesPath(resoursesInfo.Path), false,
                            Encoding.GetEncoding(1251));
                        texturelistSerializer.Serialize(sw, resoursesPacks.TexturesInfoList[EngineMain.TextureListByID(resoursesPacks.ID, resoursesInfo.IDResourses)]);
                        sw.Close();
                    }
                }    
            }
        }

    }
}
