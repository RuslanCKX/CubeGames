using System.IO;
using System.Windows;
using System.Windows.Forms;
using CubeWorldGameEngine;
using MessageBox = System.Windows.MessageBox;


namespace CubeWorldEditor
{
    public class Config
    {
        //путь к игре по умолчанию
        public static string DefaultGamePath = "";
        //путь файла конфигурации редактора
        private const string FilePath = "config\\config";

        //запись файла конфигурации
        public static bool WriteDefaultSetting()
        {
            if (Directory.Exists("config") != true) Directory.CreateDirectory("config");

            StreamWriter settingWriter = new StreamWriter(File.Create(FilePath));
            //пока только одна путь к игре
            settingWriter.WriteLine("DefaultGamePath = " + DefaultGamePath);
            settingWriter.Close();
            return true;
        }

        //чтение дефолтовых настроек редактора
        public static bool ReadDefaultSetting()
        {
            //проверяем наличие файла
            if (File.Exists(FilePath) != true) return false;
            
            StreamReader settingsReader = new StreamReader(FilePath);

            while (settingsReader.EndOfStream != true)
            {
                string tmpReadLine = settingsReader.ReadLine();

                if (tmpReadLine != null)
                {
                    string[] tmpLines = tmpReadLine.Split('=');

                    if (tmpLines.Length != 2) return false;

                    //есть настройка пути по умолчанию
                    if (tmpLines[0].ToLower().Trim() == "defaultgamepath")
                    {
                        //если она задана
                        if (tmpLines[1].Trim() != "")
                        {
                            DefaultGamePath = tmpLines[1].Trim();
                        }
                    }
                }
            }

            settingsReader.Close();

            if (DefaultGamePath == null) return false;
           
            return true;
        }

        //установка пути к игре для чтения настроек
        public static void SetGamePath()
        {
            FolderBrowserDialog defaultBrowserDialog = new FolderBrowserDialog
            {
                Description = @"Выберите папку с игрой по умолчанию:"
            };

            if (defaultBrowserDialog.ShowDialog() == DialogResult.OK && defaultBrowserDialog.SelectedPath != null)
            {
                DefaultGamePath = defaultBrowserDialog.SelectedPath + "\\";
                WriteDefaultSetting();
            }

        }

        //инициализация редактора
        public static void Init()
        {
            if (ReadDefaultSetting() != true)
            {
                //путь к игре не найден

                MessageBoxResult msg = MessageBox.Show("Путь к игре по умолчанию не найден! Указать сейчас?", "Ошибка",
                    MessageBoxButton.YesNo);
                if (msg != MessageBoxResult.Yes) return;

                SetGamePath();

                if (DefaultGamePath == null) return;

                EngineMain.AppPath = DefaultGamePath;

                SetEnginePath();
            }
            else
            {
                SetEnginePath();
            }
        }

        public static void SetEnginePath()
        {
            EngineMain.AppPath = DefaultGamePath;
            
            if (EngineMain.InitPath() == false)
            {
                //почемуто не найден файл ресурсов
                //возможно первый запуск
                if (EngineMain.IsResoursesFolderFound() != true || EngineMain.IsResPacksFileFound() != true)
                {
                    MessageBoxResult msg = MessageBox.Show("Путь к файлу ресурсов не найден! Создать?", "Ошибка",
                        MessageBoxButton.YesNo);

                    if (msg != MessageBoxResult.Yes) return;

                    if (EngineMain.IsResoursesFolderFound() != true) Directory.CreateDirectory(EngineMain.AppPath + EngineMain.ResPath);

                    StreamWriter tmpFile =
                        new StreamWriter(EngineMain.AppPath + EngineMain.ResPath + EngineMain.ResPacksFileName);
                    tmpFile.Close();
                }

            }
        }
    }
  
}
