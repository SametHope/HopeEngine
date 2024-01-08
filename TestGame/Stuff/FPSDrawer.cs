using Raylib_CsLo;

namespace HopeEngine.TestGame;

public class FPSDrawer : IDraw
{
    public DrawMode DrawMode => DrawMode.Screen;

    public void Draw()
    {
        Raylib.DrawText($"{Raylib.GetFPS()} : {Engine.DeltaTime}ms", 10, 10, 18, Raylib.GREEN);
    }
}