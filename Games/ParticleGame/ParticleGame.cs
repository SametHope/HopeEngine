using Raylib_CsLo;
using System.Numerics;

namespace HopeEngine.ParticleGame;

public class ParticleGame : IGame
{
    public const string VERSION = "1.1.4";
    public static string WindowTitle => $"The '{nameof(ParticleGame)}' {VERSION}";
    public static int WindowWidth { get; set; } = 1280;
    public static int WindowHeight { get; set; } = 720;
    public static int TargetFPS { get; private set; } = 120;
    public static Vector2 WindowCenter => new Vector2(WindowWidth / 2, WindowHeight / 2);

    public static Vector2 MouseScreenPosition { get; private set; }
    public static float FrameTime { get; private set; }
    public static int FPS { get; private set; }

    private ParticleSystem _system;
    private WindowResizeAgent _resizeAgent;

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
        _resizeAgent = new WindowResizeAgent(KeyboardKey.KEY_F11, () => _system = new ParticleSystem());
    }

    public void Update()
    {
        // Setup frame variables
        MouseScreenPosition = Raylib.GetMousePosition();
        FPS = Raylib.GetFPS();
        FrameTime = Raylib.GetFrameTime();

        _resizeAgent.Update();
        _system.Update();
    }

    public void Draw()
    {
        Raylib.ClearBackground(Raylib.BLACK);

        _system.Draw();
    }
}
