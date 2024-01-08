using Raylib_CsLo;

namespace HopeEngine.TestGame;

public class FPSDrawer : IDraw
{
    public DrawMode DrawMode => DrawMode.Screen;
    public static bool UseFPSDrawer { get; set; } = false;
    public void Draw()
    {
        if (UseFPSDrawer)
        {
            Raylib.DrawText($"{Raylib.GetFPS()} : {Engine.DeltaTime}ms", 10, 10, 18, Raylib.GREEN);
        }
    }

    [OnGameStart]
    private static void Init()
    {
        Engine.SortedDrawQueue[CallOrder.Late].EnqueueAddition(new FPSDrawer());
    }
}