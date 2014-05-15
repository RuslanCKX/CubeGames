using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using CubeWorldGameEngine.Helpers;

namespace CubeWorldGameEngine.Helpers
{
    public class XMLReaderInfo
    {

  
        public static void ReadResoursesPacksInfo()
        {
            StreamReader reader = new StreamReader(EngineMain.FullResoursesPacksFileName(), Encoding.GetEncoding(1251));
            
            XmlSerializer serializer = new XmlSerializer(typeof(List<ResoursesPacksInfo>));

            EngineMain.ResPacks = (List<ResoursesPacksInfo>) serializer.Deserialize(reader);

            reader.Close();

            ReadTextureListInfo();
        }

        public static void ReadTextureListInfo()
        {
            foreach (ResoursesPacksInfo resoursesPacks in EngineMain.ResPacks)
            {
                foreach (ResoursesInfo resoursesInfo in resoursesPacks.ResoursesList)
                {
                    if (resoursesInfo.ResoursesType == EngineMain.Resourses.Textrures)
                    {
                        StreamReader reader = new StreamReader(EngineMain.GetFullResoursesPath(resoursesInfo.Path), Encoding.GetEncoding(1251));

                        XmlSerializer serializer = new XmlSerializer(typeof(TexturesList));
                        TexturesList tmpList;

                        try
                        {
                            tmpList = (TexturesList) serializer.Deserialize(reader);
                        }
                        catch (Exception)
                        {
                            tmpList = new TexturesList();
                        }

                        tmpList.ID = resoursesInfo.IDResourses;
                        
                        resoursesPacks.TexturesInfoList.Add(tmpList);

                    }
                }
            }

        }
  
    }
}
