using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace CubeWorldGameEngine.Helpers
{
    public enum MultiTextureEnum
    {
        Front,
        Back,
        Left,
        Right,
        Top,
        Bottom
    }

    public class MultiTextureBox
    {
        public Texture2D Front;
        public Texture2D Back;
        public Texture2D Left;
        public Texture2D Right;
        public Texture2D Top;
        public Texture2D Bottom;
    }

    public class DrawingSides
    {
        public bool Front { get; set;}
        public bool Back { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Top { get; set; }
        public bool Bottom { get; set; }

        public DrawingSides()
        {
            Front = true;
            Back = true;

            Left = true;
            Right = true;

            Top = true;
            Bottom = true;
        }
    }

    public class MultiTextureIndex
    {
        public string Front { get; set; }
        public string Back { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
        public string Top { get; set; }
        public string Bottom { get; set; }

        public MultiTextureIndex() {}
    }
}
