using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace CubeWorldGameEngine.Helpers
{
    [Serializable]
    public class TexturesInfo
    {
        //ID текстуры собственно то по чему будем искать ее в массиве
        [XmlAttribute]
        public int ID { get; set; }
        [XmlAttribute]
        public bool IsAlpha { get; set; }
        
        //имя текстуры для упрощения редактирования
        //типа текстура травы
        [XmlAttribute]
        public string Name { get; set; }
        //имя основного bitmap файла текстуры
        public string ImageFileName { get; set; }
        //положение и размер в файле
        public Point Begin { get; set; }
        public Point Size { get; set; }

        //сама текстура динамически генерируемая в памяти
        [XmlIgnore]
        public Bitmap ImageBitmap { get; set; }
        

        public TexturesInfo()
        {
            Name = "";
            ImageFileName = "";
            Begin = new Point();
            Size = new Point();
            ImageBitmap = null;
        }

        public override string ToString()
        {
            return "ID: " + ID + "; Name: " + Name;
        }
    }
}
