using Raylib_CsLo;
using System.Numerics;

namespace HopeEngine.ParticleTestGame;

public class ParticleGame : IGame
{
    public static string WindowTitle => "The 'Particle Game'";
    public static int WindowWidth => 1280;
    public static int WindowHeight => 720;
    public static int TargetFPS => 120;
    public static Vector2 WindowCenter => new Vector2(WindowWidth / 2, WindowHeight / 2);
    public static Vector2 MouseScreenPosition { get; private set; }
    public static float FrameTime { get; private set; }
    public static int FPS { get; private set; }

    private ParticleSystem _system;

    public void Initialize()
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, WindowTitle);
        Raylib.SetExitKey(KeyboardKey.KEY_ESCAPE);
        Raylib.SetTargetFPS(TargetFPS);
    }

    public void Deinitialize()
    {
        Raylib.CloseWindow();
    }

    public void Start()
    {
        _system = new ParticleSystem();
    }

    public void Update()
    {
        MouseScreenPosition = Raylib.GetMousePosition();
        FPS = Raylib.GetFPS();
        FrameTime = Raylib.GetFrameTime();

        _system.Update();
    }

    public void Draw()
    {
        Raylib.ClearBackground(Raylib.BLACK);

        _system.Draw();
    }
}
