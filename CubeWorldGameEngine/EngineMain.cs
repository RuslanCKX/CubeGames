using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CubeWorldGameEngine.Helpers;
using System.IO;
using CubeWorldGameEngine.Mods;
using CubeWorldGameEngine.Helpers;

namespace CubeWorldGameEngine
{
    [Serializable]
    public static class EngineMain
    {

        #region Константы

        public const string ResPath = "Resourses\\"; //путь к папке с описанием ресурсов относительно основной папки
        public const string ResPacksFileName = "respacks.xml"; //файл описания всего и вся

        #endregion

        #region Basic Enums

        public enum Resourses
        {
            Textrures,
            BaseBlocks
        }

        #endregion

        #region Shared variables

        public static EngineErrorControl CubeGameErrorControl = new EngineErrorControl();
        public static bool EditorMode = false;
        public static List<IMods> Mods = new List<IMods>();
        public static string AppPath; //!!! с двумя слешами в конце
        public static List<ResoursesPacksInfo> ResPacks = new List<ResoursesPacksInfo>();

        #endregion

        #region Init

        public static void Init(string GamePath = "")
        {
            if (GamePath != "") AppPath = GamePath;
            XMLReaderInfo.ReadResoursesPacksInfo();

            SortAll();
        }

        #endregion

        #region Work with Path

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
            if (Directory.Exists(AppPath + ResPath) != true)
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

        public static string FullResoursesPacksFileName()
        {
            return AppPath + ResPath + ResPacksFileName;
        }

        public static string GetFullResoursesPath(string path)
        {
            return AppPath + ResPath + path;
        }

        #endregion

        #region Find Values

        public static int TextureListByID(int IDResPacks, int IDTexture)
        {
            return ResPacks[IDResPacks].TexturesInfoList.FindIndex(x => x.ID == IDTexture);

        }

        public static TexturesList TexturesListByIDList(int IDResPacks, int IDTexture)
        {
            return ResPacks[IDResPacks].TexturesInfoList.Find(x => x.ID == IDTexture);
        }

        #endregion

        #region Sort Values

        public static void SortAll()
        {
            SortResPacks();
            SortResList();
            SortTexturesInfoList();
            SortTexturesInfo();
            SortBaseBlocksInfoList();
            SortBaseBlocksInfo();
        }

        public static void SortResPacks()
        {
            if (ResPacks.Count != 0) ResPacks.Sort((a, b) => a.ID.CompareTo(b.ID));
        }

        public static void SortResList()
        {
            if (ResPacks.Count != 0)
            {
                foreach (ResoursesPacksInfo resoursesPacks in ResPacks)
                {
                    if (resoursesPacks.ResoursesList.Count != 0)
                    {
                        resoursesPacks.ResoursesList =
                            resoursesPacks.ResoursesList.OrderBy(x => x.ResoursesType)
                                .ThenBy(x => x.IDResourses)
                                .ToList();
                    }
                }
            }
        }

        public static void SortTexturesInfoList()
        {
            if (ResPacks.Count != 0)
            {
                foreach (ResoursesPacksInfo resoursesPacks in ResPacks)
                {
                    if (resoursesPacks.TexturesInfoList.Count != 0)
                    {
                        resoursesPacks.TexturesInfoList.Sort((a, b) => a.ID.CompareTo(b.ID));

                     }
                }
            }
        }

        public static void SortBaseBlocksInfoList()
        {
            if (ResPacks.Count != 0)
            {
                foreach (ResoursesPacksInfo resoursesPacks in ResPacks)
                {
                    if (resoursesPacks.BaseBlocksInfoList.Count != 0)
                    {
                        resoursesPacks.BaseBlocksInfoList.Sort((a, b) => a.ID.CompareTo(b.ID));

                    }
                }
            }
        }

        public static void SortTexturesInfo()
        {
            if (ResPacks.Count != 0)
            {
                foreach (ResoursesPacksInfo resoursesPacks in ResPacks)
                {
                    if (resoursesPacks.TexturesInfoList.Count != 0)
                    {
                        foreach (TexturesList texturesList in resoursesPacks.TexturesInfoList)
                        {
                            if (texturesList.TextureList.Count != 0)
                            {
                                texturesList.TextureList.Sort((a, b) => a.ID.CompareTo(b.ID));
                            }
                        }

                    }
                }
            }
        }

        public static void SortBaseBlocksInfo()
        {
            if (ResPacks.Count != 0)
            {
                foreach (ResoursesPacksInfo resoursesPacks in ResPacks)
                {
                    if (resoursesPacks.BaseBlocksInfoList.Count != 0)
                    {
                        foreach (BaseBlocksList baseBlocksList in resoursesPacks.BaseBlocksInfoList)
                        {
                            if (baseBlocksList.BaseBlocks.Count != 0)
                            {
                                baseBlocksList.BaseBlocks.Sort((a, b) => a.ID.CompareTo(b.ID));
                            }
                        }

                    }
                }
            }
        }
        #endregion
    }
}
