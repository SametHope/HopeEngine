using Raylib_CsLo;
using System.Numerics;

namespace HopeEngine.SandboxGame;

public class SandboxGame : IGame
{
    public static string WindowTitle => $"The '{nameof(SandboxGame)}'";

    public const int WindowWidth = 1280;
    public const int WindowHeight = 720;

    public static int TargetFPS { get; private set; } = 120;

    public static Vector2 MouseScreenPosition { get; private set; }
    public static float FrameTime { get; private set; }
    public static int FPS { get; private set; }

    public static readonly QueueList<IUpdate> UpdateQueueList = new();
    public static readonly QueueList<IDraw> DrawQueueList = new();

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
        UpdateQueueList.EnqueueAddition( new DebugInputHandler());
    }

    public void Update()
    {
        // Setup frame variables
        MouseScreenPosition = Raylib.GetMousePosition();
        FPS = Raylib.GetFPS();
        FrameTime = Raylib.GetFrameTime();

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

public class ParticleMaster : IUpdate, IDraw
{
    const int GridWidth = SandboxGame.WindowWidth;
    const int GridHeight = SandboxGame.WindowHeight;

    public ParticleType[,] Grid = new ParticleType[GridWidth, GridHeight];

    public void Update()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                UpdateParticle(x, y, Grid[x, y]);
            }
        }
    }

    public void Draw()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                UpdateParticle(x, y, Grid[x, y]);
            }
        }
    }

    private void UpdateParticle(int px, int py, ParticleType ptype)
    {

    }

}

public class DebugInputHandler : IUpdate
{
    public void Update()
    {

    }
}

public enum ParticleType
{
    Empty = 0,
    Iron,
}