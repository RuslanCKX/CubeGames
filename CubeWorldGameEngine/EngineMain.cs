using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CubeWorldGameEngine.Mods;
using System.IO;

namespace CubeWorldGameEngine
{
    public class EngineMain
    {

        #region Константы

        public const string ResPath = @"Resourses\\"; //путь к папке с описанием ресурсов относительно основной папки
        public const string ResPacksFileName = "respacks.xml"; //файл описания всего и вся
        #endregion
 
        public bool EditorMode = false;
        public List<IMods> Mods;
        public static string AppPath; //!!! с двумя слешами в конце

        public static bool InitPath()
        {
            if (AppPath != null && IsResPacksFileFound() != true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsResoursesFolderFound()
        {
            if (Directory.Exists(AppPath) != true)
            {
                //папка не найдена
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsResPacksFileFound()
        {
            if (File.Exists(AppPath + ResPath + ResPacksFileName) != true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
