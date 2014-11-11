using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheBlindMan
{
    interface ICamera2D
    {
        Vector2 Position { get; set; }
        float MoveSpeed { get; set; }
        float Rotation { get; set; }
        Vector2 Origin { get; }
        float Scale { get; set; }
        Vector2 ScreenCenter { get; }
        Matrix Transform { get; }
        IFocusable Focus { get; set; }
        bool IsInView(Vector2 position, Texture2D texture);
    }
}
