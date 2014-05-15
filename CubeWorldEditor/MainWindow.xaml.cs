using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CubeWorldGameEngine;
using CubeWorldGameEngine.Helpers;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using Path = System.IO.Path;
using Point = System.Drawing.Point;

namespace CubeWorldEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Error Control
        private void EngineErrorEvent(object sender, EngineErrorEventArgs e)
        {
            MessageBox.Show(e.EngineErrorID.ToString() + ". " + e.EngineErrorStringMsg, "Ошибка");
        }
        #endregion
        
        #region Main
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Config.Init();

            //подписываемся на обработку ошибок
            EngineMain.CubeGameErrorControl.EngineError += new EngineErrorEventHandler(EngineErrorEvent);

            //пытаемся прочитать информацию о ресурсах
            EngineMain.Init();
            //обновляем форму

            List<string> tmpList = Enum.GetNames(typeof (EngineMain.Resourses)).ToList();
            foreach (string tmp in tmpList)
            {
                lstResoursesEnum.Items.Add(tmp);
            }
            lstResoursesEnum.SelectedIndex = 0;

            UpdateResoursesPackList();

        }

        #endregion

        #region ResoursesPacks

        #region SaveLoadAll
        private void cmdSetPath_Click(object sender, RoutedEventArgs e)
        {
            Config.SetGamePath();
            Config.SetEnginePath();
            EngineMain.Init();
            UpdateResoursesPackList();
        }

        private void cmdSaveAll_Click(object sender, RoutedEventArgs e)
        {
            XMLWriterInfo.WriteResoursesPacksInfo();
            MessageBox.Show("Сохранено.");
        }
        #endregion

        #region Updater

        public void UpdateResoursesPackList()
        {
            lstResoursesPacks.Items.Clear();

            if (EngineMain.ResPacks.Count == 0) return;

            foreach (ResoursesPacksInfo resoursesPacksInfo in EngineMain.ResPacks)
            {
                lstResoursesPacks.Items.Add(resoursesPacksInfo.ToString());
            }

            lstResoursesPacks.SelectedIndex = 0;
            UpdateResoursesList();
        }

        public void UpdateResoursesList()
        {
            lstResourses.Items.Clear();

            if (lstResoursesPacks.SelectedIndex == -1) return;

            if (EngineMain.ResPacks[lstResoursesPacks.SelectedIndex].ResoursesList.Count == 0) return;

            foreach (ResoursesInfo resoursesInfo in EngineMain.ResPacks[lstResoursesPacks.SelectedIndex].ResoursesList)
            {
                lstResourses.Items.Add(resoursesInfo);
            }

            lstResourses.SelectedIndex = 0;
        }

        public void UpdateTabResourses()
        {

            ClearTabResourses();
            if (lstResoursesPacks.SelectedIndex == -1) return;

            ResoursesPacksInfo tmp = EngineMain.ResPacks[lstResoursesPacks.SelectedIndex];

            txtPackName.Text = tmp.Name;
            txtPackID.Text = tmp.ID.ToString(CultureInfo.InvariantCulture);

            UpdateResoursesList();
            SelectResoursesList();

        }

        public void ClearTabResourses()
        {
            txtPackName.Text = "";
            txtPackID.Text = "";
            txtPath.Text = "";

            lstResourses.Items.Clear();
        }

        public void SelectResoursesList()
        {
            txtPath.Text = "";

            if (lstResourses.SelectedIndex == -1 || lstResoursesPacks.SelectedIndex == -1) return;

            ResoursesInfo tmp = EngineMain.ResPacks[lstResoursesPacks.SelectedIndex].ResoursesList[lstResourses.SelectedIndex];

            lstResoursesEnum.SelectedItem = tmp.ResoursesType;
            txtPath.Text = tmp.Path;
            txtResoursesID.Text = tmp.IDResourses.ToString(CultureInfo.InvariantCulture);
        }

        #endregion
        
        private void lstResoursesPacks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstResoursesPacks.SelectedIndex == -1) return;
            UpdateTabResourses();
        }

        private void cmdSetResoursePath_Click(object sender, RoutedEventArgs e)
        {
   
            SaveFileDialog saveFile = new SaveFileDialog
            {
                InitialDirectory = EngineMain.AppPath + EngineMain.ResPath,
                Filter = @"файл описания ресурса (*.xml)|*.xml"
            };

            if (saveFile.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (saveFile.FileName.Contains(EngineMain.AppPath + EngineMain.ResPath) != true)
            {
                MessageBox.Show("Ошибка", "Выберите путь относительно папки ресурсов");
                return;
            }

            if (File.Exists(saveFile.FileName) != true)
            {
                FileStream fs = File.Create(saveFile.FileName);
                fs.Close();
            }

            txtPath.Text = saveFile.FileName.Replace(EngineMain.AppPath + EngineMain.ResPath, "");
        }

        private void cmdUpdatePath_Click(object sender, RoutedEventArgs e)
        {
            if (lstResoursesPacks.SelectedIndex == - 1) return;
            if (lstResourses.SelectedIndex == - 1) return;
            if (lstResoursesEnum.SelectedIndex == - 1) return;
            if (txtPath.Text == "") return;

            ResoursesInfo tmp = (ResoursesInfo) lstResourses.SelectedItem;

            tmp.Path = txtPath.Text;
            tmp.IDResourses = Convert.ToInt32(txtResoursesID.Text);

            tmp.ResoursesType =
                (EngineMain.Resourses)
                    Enum.Parse(typeof (EngineMain.Resourses), lstResoursesEnum.SelectedItem.ToString());
            
            EngineMain.ResPacks[lstResoursesPacks.SelectedIndex].ResoursesList[lstResourses.SelectedIndex] = tmp;
            UpdateResoursesList();
            lstResourses.SelectedItem = tmp;
        }

        private void cmdAddPath_Click(object sender, RoutedEventArgs e)
        {
            if (lstResoursesPacks.SelectedIndex == -1) return;
            if (lstResoursesEnum.SelectedIndex == -1) return;
            if (txtPath.Text == "") return;

            ResoursesInfo tmp = new ResoursesInfo
            {
                Path = txtPath.Text,
                IDResourses = Convert.ToInt32(txtResoursesID.Text),
                ResoursesType = (EngineMain.Resourses) Enum.Parse(typeof(EngineMain.Resourses), lstResoursesEnum.SelectedItem.ToString())
            };


            EngineMain.ResPacks[lstResoursesPacks.SelectedIndex].ResoursesList.Add(tmp);
            lstResourses.Items.Add(tmp);
            lstResourses.SelectedItem = tmp;


        }

        private void lstResourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectResoursesList();
        }

        private void cmdAddResPack_Click(object sender, RoutedEventArgs e)
        {
            string tmp = "";
            if (Dialogs.InputBox("Имя нового пакета", "Введите имя ресурс пакета", ref tmp) ==
                System.Windows.Forms.DialogResult.OK)
            {
                ResoursesPacksInfo resoursesPacks = new ResoursesPacksInfo {Name = tmp};

                int id = 0;
                if (EngineMain.ResPacks.Count != 0) id = EngineMain.ResPacks[EngineMain.ResPacks.Count - 1].ID + 1;
                resoursesPacks.ID = id;
                
                EngineMain.ResPacks.Add(resoursesPacks);
                
                UpdateResoursesPackList();

                lstResoursesPacks.SelectedIndex = lstResoursesPacks.Items.Count - 1;
            }
        }

        private void cmdDeleteResPack_Click(object sender, RoutedEventArgs e)
        {
            if (lstResoursesPacks.SelectedIndex == -1) return;

            EngineMain.ResPacks.RemoveAt(lstResoursesPacks.SelectedIndex);
            UpdateResoursesPackList();
        }

        private void cmdUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lstResoursesPacks.SelectedIndex == - 1) return;

            EngineMain.ResPacks[lstResoursesPacks.SelectedIndex].ID = Convert.ToInt32(txtPackID.Text);
            string tmp = txtPackName.Text;
            EngineMain.ResPacks[lstResoursesPacks.SelectedIndex].Name = tmp;

            EngineMain.ResPacks.Sort((a, b) => a.ID.CompareTo(b.ID));
            UpdateResoursesPackList();
            lstResoursesPacks.SelectedItem = tmp;
        }

        private void cmdGetNextID_Click(object sender, RoutedEventArgs e)
        {
            if (lstResoursesPacks.SelectedIndex == -1) return;
            txtPackID.Text = (EngineMain.ResPacks[EngineMain.ResPacks.Count - 1].ID + 1).ToString(CultureInfo.InvariantCulture);
        }

        private void cmdGetNextIDResourses_Click(object sender, RoutedEventArgs e)
        {
            if (lstResoursesPacks.SelectedIndex == -1) return;
            if (EngineMain.ResPacks[lstResoursesPacks.SelectedIndex].ResoursesList.Count == 0) return;

            txtResoursesID.Text =
                (EngineMain.ResPacks[lstResoursesPacks.SelectedIndex].ResoursesList[
                    EngineMain.ResPacks[lstResoursesPacks.SelectedIndex].ResoursesList.Count - 1].IDResourses + 1).ToString();
        }

        #endregion

        #region Textures

        public void UpdateTabTexturesPacksList()
        {
            lstTexturesResoursesPacksNames.Items.Clear();
            lstTextureFilesName.Items.Clear();
            lstTexturesList.Items.Clear();

            if (EngineMain.ResPacks.Count == 0) return;

            foreach (ResoursesPacksInfo resoursesPacks in EngineMain.ResPacks)
            {
                lstTexturesResoursesPacksNames.Items.Add(resoursesPacks.ToString());

            }

            lstTexturesResoursesPacksNames.SelectedIndex = 0;
            UpdateTabTexturesResoursesList();
        }

        public void UpdateTabTexturesResoursesList()
        {
            lstTextureFilesName.Items.Clear();
            txtTexturesListName.Text = "";
            lbltexturesListID.Content = "";

            if (lstTexturesResoursesPacksNames.SelectedIndex == -1) return;
            if (EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].ResoursesList.Count == 0) return;

            foreach (ResoursesInfo resoursesInfo in EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].ResoursesList)
            {
                if (resoursesInfo.ResoursesType == EngineMain.Resourses.Textrures)
                {
                    lstTextureFilesName.Items.Add(resoursesInfo);
                }
            }
            lstTextureFilesName.SelectedIndex = 0;
            UpdateTabTexturesList();
        }

        public void UpdateTabTexturesList()
        {
            lstTexturesList.Items.Clear();

            if (lstTexturesResoursesPacksNames.SelectedIndex == -1) return;
            if (EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].ResoursesList.Count == 0) return;
            if (lstTextureFilesName.SelectedIndex == -1) return;

            int ind = EngineMain.TextureListByID(lstTexturesResoursesPacksNames.SelectedIndex,
                ((ResoursesInfo) lstTextureFilesName.SelectedItem).IDResourses);

            TexturesList tmpTexturesList =
                EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].TexturesInfoList[ind];

            if (tmpTexturesList.TextureList.Count == 0) return;

            foreach (TexturesInfo tmpTexturesInfo in tmpTexturesList.TextureList)
            {
                lstTexturesList.Items.Add(tmpTexturesInfo.ToString());
            }
        }

        public void SelectTabTextureFileName()
        {
            if (lstTextureFilesName.SelectedIndex == -1) return;

            int index = ((ResoursesInfo) (lstTextureFilesName.SelectedItem)).IDResourses;

            lbltexturesListID.Content = "ID: " + index.ToString(CultureInfo.InvariantCulture);
            int textureIndex = EngineMain.TextureListByID(lstTexturesResoursesPacksNames.SelectedIndex,
                index);
            TexturesList tmpTexturesList =
                EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].TexturesInfoList[textureIndex];
            
            txtTexturesListName.Text = tmpTexturesList.Name;
        }

        public void SelectTabTextureInfo()
        {
            txtTextureID.Text = "";
            txtTextureName.Text = "";

            imgTextureMain.Source = null;
            imgTextureCutMain.Source = null;

            chkIsTextureAlpha.IsChecked = false;
            
            txtBeginX.Text = "";
            txtBeginY.Text = "";
            txtSizeX.Text = "";
            txtSizeY.Text = "";
            
            if (lstTexturesResoursesPacksNames.SelectedIndex == -1) return;
            if (EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].ResoursesList.Count == 0) return;
            if (lstTextureFilesName.SelectedIndex == -1) return;
            if (lstTexturesList.SelectedIndex == -1) return;

            int ind = EngineMain.TextureListByID(lstTexturesResoursesPacksNames.SelectedIndex,
                ((ResoursesInfo)lstTextureFilesName.SelectedItem).IDResourses);

            TexturesInfo tmpTexturesInfo =
                EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].TexturesInfoList[ind].TextureList[lstTexturesList.SelectedIndex];

            txtTextureName.Text = tmpTexturesInfo.Name;
            txtTextureID.Text = tmpTexturesInfo.ID.ToString(CultureInfo.InvariantCulture);

            if (tmpTexturesInfo.ImageFileName != "")
            {
                imgTextureMain.Source = ImageHelpers.ImageSourceFromBitmap(tmpTexturesInfo);

                if (ImageHelpers.IsCanCutImage(tmpTexturesInfo))
                {
                    imgTextureCutMain.Source = ImageHelpers.ImageSourceFromBitmap(ImageHelpers.CutImage(tmpTexturesInfo));
                }
            }
            
            chkIsTextureAlpha.IsChecked = tmpTexturesInfo.IsAlpha;

            txtBeginX.Text = tmpTexturesInfo.Begin.X.ToString(CultureInfo.InvariantCulture);
            txtBeginY.Text = tmpTexturesInfo.Begin.Y.ToString(CultureInfo.InvariantCulture);

            txtSizeX.Text = tmpTexturesInfo.Size.X.ToString(CultureInfo.InvariantCulture);
            txtSizeY.Text = tmpTexturesInfo.Size.Y.ToString(CultureInfo.InvariantCulture);
            
        }

        private void cmdUpdateTextureList_Click(object sender, RoutedEventArgs e)
        {
            UpdateTabTexturesPacksList();
        }

        private void lstTexturesResoursesPacksNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTabTexturesResoursesList();
        }

        private void lstTextureFilesName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTabTexturesList();
            SelectTabTextureFileName();
        }

        private void cmdUpdateTextureListName_Click(object sender, RoutedEventArgs e)
        {
            if (lstTextureFilesName.SelectedIndex == -1) return;
            if (txtTexturesListName.Text == "") return;

            int index = ((ResoursesInfo)(lstTextureFilesName.SelectedItem)).IDResourses;

            int textureIndex = EngineMain.TextureListByID(lstTexturesResoursesPacksNames.SelectedIndex,
                index);
            TexturesList tmpTexturesList =
                EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].TexturesInfoList[textureIndex];

            tmpTexturesList.Name = txtTexturesListName.Text;

            int tmpIndexFilesNames = lstTextureFilesName.SelectedIndex;
            int tmpIndexSelTexture = lstTexturesList.SelectedIndex;

            UpdateTabTexturesResoursesList();

            lstTextureFilesName.SelectedIndex = tmpIndexFilesNames;
            lstTexturesList.SelectedIndex = tmpIndexSelTexture;
        }

        private void cmdAddTexture_Click(object sender, RoutedEventArgs e)
        {
            string tmp = "";
            if (Dialogs.InputBox("Имя нового текстуры", "Введите имя новой текстуры", ref tmp) ==
                System.Windows.Forms.DialogResult.OK)
            {
                TexturesInfo tmpTexturesInfo = new TexturesInfo {Name = tmp};

                int index = ((ResoursesInfo)(lstTextureFilesName.SelectedItem)).IDResourses;
                int textureIndex = EngineMain.TextureListByID(lstTexturesResoursesPacksNames.SelectedIndex,
                    index);
                TexturesList tmpTexturesList =
                    EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].TexturesInfoList[textureIndex];

                if (tmpTexturesList.TextureList.Count != 0)
                    tmpTexturesInfo.ID = tmpTexturesList.TextureList[tmpTexturesList.TextureList.Count - 1].ID;

                tmpTexturesList.TextureList.Add(tmpTexturesInfo);
                EngineMain.SortTexturesInfo();
                UpdateTabTexturesList();
                lstTexturesList.SelectedItem = (tmpTexturesInfo.ToString());
            }
        }

        private void cmdDeleteTexture1_Click(object sender, RoutedEventArgs e)
        {
            if (lstTexturesList.SelectedIndex == -1) return;
             int index = ((ResoursesInfo)(lstTextureFilesName.SelectedItem)).IDResourses;
                int textureIndex = EngineMain.TextureListByID(lstTexturesResoursesPacksNames.SelectedIndex,
                    index);
            TexturesList tmpTexturesList =
                EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].TexturesInfoList[textureIndex];
            tmpTexturesList.TextureList.RemoveAt(lstTexturesList.SelectedIndex);
            UpdateTabTexturesList();
        }

        private void lstTexturesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectTabTextureInfo();
        }

        private void cmdTextureSelectMainFile_Click(object sender, RoutedEventArgs e)
        {
            if (lstTexturesList.SelectedIndex == -1) return;

            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = @"Image (*.bmp, *.png, *.jpg, *.gif)|*.bmp;*.jpg;*.png;*.gif",
                InitialDirectory = EngineMain.GetFullResoursesPath("")
            };

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            string file = dlg.FileName;

            string directoryName = Path.GetDirectoryName(file);

            int index = ((ResoursesInfo)(lstTextureFilesName.SelectedItem)).IDResourses;
            int textureIndex = EngineMain.TextureListByID(lstTexturesResoursesPacksNames.SelectedIndex,
                index);
            TexturesList tmpTexturesList =
                EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].TexturesInfoList[textureIndex];

            if (directoryName != null && directoryName.Contains(EngineMain.GetFullResoursesPath("")) == false)
            {
                string pathfile = Path.GetDirectoryName(EngineMain.GetFullResoursesPath(((ResoursesInfo) lstTextureFilesName.SelectedItem).Path));

                pathfile += "\\" + tmpTexturesList.Name;

                if (Directory.Exists(pathfile) == false)
                {
                    Directory.CreateDirectory(pathfile);
                }
                
                File.Copy(file, pathfile + "\\" + Path.GetFileName(file));

                file = pathfile + "\\" + Path.GetFileName(file);
                
            }

            file = file.Replace(EngineMain.GetFullResoursesPath(""), "");
            tmpTexturesList.TextureList[lstTexturesList.SelectedIndex].ImageFileName = file;
            imgTextureMain.Source =
                ImageHelpers.ImageSourceFromBitmap(tmpTexturesList.TextureList[lstTexturesList.SelectedIndex]);
        }

        private void cmdTextureCut_Click(object sender, RoutedEventArgs e)
        {
            if (lstTexturesList.SelectedIndex == -1) return;

            int index = ((ResoursesInfo)(lstTextureFilesName.SelectedItem)).IDResourses;
            int textureIndex = EngineMain.TextureListByID(lstTexturesResoursesPacksNames.SelectedIndex,
                index);
            TexturesInfo tmpTexturesInfo =
                EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].TexturesInfoList[textureIndex].TextureList[lstTexturesList.SelectedIndex];

            Point begin = new Point(Convert.ToInt32(txtBeginX.Text), Convert.ToInt32(txtBeginY.Text));
            Point size = new Point(Convert.ToInt32(txtSizeX.Text), Convert.ToInt32(txtSizeY.Text));

            tmpTexturesInfo.Begin = begin;
            tmpTexturesInfo.Size = size;

            if (ImageHelpers.IsCanCutImage(tmpTexturesInfo) != true)
            {
                MessageBox.Show("Не возможно обрезать текстуру");
                return;
            }

            tmpTexturesInfo.ImageBitmap = ImageHelpers.CutImage(tmpTexturesInfo);
            imgTextureCutMain.Source = ImageHelpers.ImageSourceFromBitmap(ImageHelpers.CutImage(tmpTexturesInfo));
        }

        private void cmdUpdateTexture_Click(object sender, RoutedEventArgs e)
        {
            
            if (lstTexturesList.SelectedIndex == -1) return;

            int index = ((ResoursesInfo)(lstTextureFilesName.SelectedItem)).IDResourses;
            int textureIndex = EngineMain.TextureListByID(lstTexturesResoursesPacksNames.SelectedIndex,
                index);
            TexturesInfo tmpTexturesInfo =
                EngineMain.ResPacks[lstTexturesResoursesPacksNames.SelectedIndex].TexturesInfoList[textureIndex].TextureList[lstTexturesList.SelectedIndex];

            Point begin = new Point(Convert.ToInt32(txtBeginX.Text), Convert.ToInt32(txtBeginY.Text));
            Point size = new Point(Convert.ToInt32(txtSizeX.Text), Convert.ToInt32(txtSizeY.Text));

            tmpTexturesInfo.Begin = begin;
            tmpTexturesInfo.Size = size;

            tmpTexturesInfo.Name = txtTextureName.Text;
            tmpTexturesInfo.ID = Convert.ToInt32(txtTextureID.Text);
            tmpTexturesInfo.ImageBitmap = ImageHelpers.CutImage(tmpTexturesInfo);

            EngineMain.SortTexturesInfo();
            UpdateTabTexturesList();
            lstTexturesList.SelectedItem = tmpTexturesInfo.ToString();
        }

        #endregion

    }
}

