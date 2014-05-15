using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Graphics;
using Point = System.Drawing.Point;

namespace CubeWorldGameEngine.Helpers
{
    public class ImageHelpers
    {
        public static Bitmap GetBitmap(TexturesInfo tex)
        {
            return new Bitmap(EngineMain.GetFullResoursesPath(tex.ImageFileName));
        }

        public static Bitmap CutImage(Bitmap img, Point begin, Point size)
        {
            return img.Clone(new Rectangle(begin.X, begin.Y, size.X, size.Y), img.PixelFormat);
        }

        public static Bitmap CutImage(TexturesInfo tex)
        {
            Bitmap tmpBitmap = new Bitmap(EngineMain.GetFullResoursesPath(tex.ImageFileName));
            return tmpBitmap.Clone(new Rectangle(tex.Begin.X, tex.Begin.Y, tex.Size.X, tex.Size.Y), tmpBitmap.PixelFormat);
        }

        public static bool IsCanCutImage(Point originalSize, Point begin, Point size)
        {
            if ((begin.X > originalSize.X) || ((begin.X + size.X) > originalSize.X)) return false;
            if ((begin.Y > originalSize.Y) || ((begin.Y + size.Y) > originalSize.Y)) return false;
            return true;
        }

        public static bool IsCanCutImage(TexturesInfo tex)
        {
            Bitmap tmpBitmap = GetBitmap(tex);

            Point originalSize = new Point(tmpBitmap.Width, tmpBitmap.Width);
            Point begin = tex.Begin;
            Point size = tex.Size;

            if ((begin.X > originalSize.X) || ((begin.X + size.X) > originalSize.X)) return false;
            if ((begin.Y > originalSize.Y) || ((begin.Y + size.Y) > originalSize.Y)) return false;
            return true;
        }

        public static Texture2D TextureFromBitmap(GraphicsDevice gdev, System.Drawing.Bitmap bmp)
        {

            Stream fs = new MemoryStream();
            bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
            var tex = Texture2D.FromStream(gdev, fs);

            fs.Close();
            fs.Dispose();

            return tex;
        }

        public static ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                        IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static ImageSource ImageSourceFromBitmap(TexturesInfo tex)
        {
            Bitmap bmp = GetBitmap(tex);
            return Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                        IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

    }
}
