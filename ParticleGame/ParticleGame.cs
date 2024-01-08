using Raylib_CsLo;
using System.Numerics;

namespace HopeEngine.ParticleTestGame;

public class ParticleGame : IGame
{
    public static string WindowTitle => "Particle Test Game";
    public static int WindowWidth => 1280;
    public static int WindowHeight => 720;
    public static int TargetFPS => 120;
    public static Vector2 ScreenCenter => new Vector2(WindowWidth / 2, WindowHeight / 2);

    public static readonly QueueList<IUpdate> UpdateQueueList = new();

    public static readonly QueueList<IDraw> DrawQueueList = new();

    public static Vector2 MouseScreenPosition { get; private set; }

    public void Initialize()
    {
        Console.WriteLine("Now? Now.");
        Raylib.InitWindow(WindowWidth, WindowHeight, WindowTitle);
        Raylib.SetExitKey(KeyboardKey.KEY_ESCAPE);
        Raylib.SetTargetFPS(TargetFPS);
    }

    public void Deinitialize()
    {
        Raylib.CloseWindow();
        Console.WriteLine("Done? Done.");
    }

    public void Start()
    {
        var pm = new ParticleSystem();
        UpdateQueueList.EnqueueAddition(pm);
        DrawQueueList.EnqueueAddition(pm);
    }

    public void Update()
    {
        MouseScreenPosition = Raylib.GetMousePosition();

        foreach (var IUpdate in UpdateQueueList)
        {
            IUpdate.Update();
        }
        
        UpdateQueueList.ProcessPendingRemovals();
        DrawQueueList.ProcessPendingRemovals();

        UpdateQueueList.ProcessPendingAdditions();
        DrawQueueList.ProcessPendingAdditions();
    }

    public void Draw()
    {
        Raylib.ClearBackground(Raylib.BLACK);

        foreach (var IDraw in DrawQueueList)
        {
            IDraw.Draw();
        }
    }
}
