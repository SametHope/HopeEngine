using Raylib_CsLo;
using System.Numerics;

namespace HopeEngine.ParticleGame;

public class ParticleGame : IGame
{
    public static string WindowTitle => "The 'Particle Game'";
    public static int WindowWidth { get; private set; } = 1280;
    public static int WindowHeight { get; private set; } = 720;
    public static int TargetFPS { get; private set; } = 120;
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
        // Setup frame variables
        MouseScreenPosition = Raylib.GetMousePosition();
        FPS = Raylib.GetFPS();
        FrameTime = Raylib.GetFrameTime();

        // Handle screen size changes
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_F11))
        {
            if(WindowWidth == 1280 && WindowHeight == 720 && !Raylib.IsWindowFullscreen())
            {
                WindowWidth = 1920;
                WindowHeight = 1080;
                Raylib.SetWindowSize(WindowWidth, WindowHeight);
                Raylib.ToggleFullscreen();
            }
            else
            {
                WindowWidth = 1280;
                WindowHeight = 720;
                Raylib.SetWindowSize(WindowWidth, WindowHeight);
                Raylib.ToggleFullscreen();
            }
            _system = new ParticleSystem();
        }

        _system.Update();
    }

    public void Draw()
    {
        Raylib.ClearBackground(Raylib.BLACK);

        _system.Draw();
    }
}
