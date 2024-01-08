//using Raylib_CsLo;

//namespace HopeEngine;

//public class TextEnt : Entity
//{
//    public string Text;
//    public int X, Y, FontSize;
//    public Color Color;
//    public TextEnt(string name, string cat, string text, int x, int y, int fontSize, Color color, Engine engine, bool centerX = true) : base(name, cat, engine)
//    {
//        Text = text;
//        if (centerX) X = x - Raylib.MeasureText(text, fontSize) / 2;
//        else X = x;
//        Y = y;
//        FontSize = fontSize;
//        Color = color;
//    }

//    public override void Update(float deltaTime)
//    {
//        Raylib.DrawText(Text, X, Y, FontSize, Color);
//    }
//}
