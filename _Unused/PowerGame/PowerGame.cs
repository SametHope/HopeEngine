using Raylib_CsLo;
using System.Numerics;

namespace HopeEngine.PowerGame;

public class PowerGame : IGame
{
    public static string WindowTitle => "The 'Power Game'";
    public static int WindowWidth { get; private set; } = 1280;
    public static int WindowHeight { get; private set; } = 720;
    public static int TargetFPS { get; private set; } = 120;

    public static Vector2 WindowCenter => new Vector2(WindowWidth / 2, WindowHeight / 2);
    public static Vector2 MouseScreenPosition { get; private set; }
    public static float FrameTime { get; private set; }
    public static int FPS { get; private set; }

    public static readonly QueueList<(string id, IUpdate iface)> UpdateQueueList = new();
    public static readonly QueueList<(string id, IDraw iface)> DrawQueueList = new();

    public bool ReadyGame()
    {
        Console.WriteLine($"{nameof(PowerGame)} is not playable.");
        return false;
    }

    public void InitializeWindow()
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, WindowTitle);
        Raylib.SetExitKey(KeyboardKey.KEY_ESCAPE);
        Raylib.SetTargetFPS(TargetFPS);
    }

    public void DeinitializeWindow()
    {
        Raylib.CloseWindow();
    }

    public void StartGame()
    {
        UpdateQueueList.EnqueueAddition(("DBG", new DebugInputHandler()));
    }

    public void Update()
    {
        // Setup frame variables
        MouseScreenPosition = Raylib.GetMousePosition();
        FPS = Raylib.GetFPS();
        FrameTime = Raylib.GetFrameTime();

        foreach (var IUpdate in UpdateQueueList)
        {
            IUpdate.iface.Update();
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
            IDraw.iface.Draw();
        }
    }
}

public class DebugInputHandler : IUpdate
{
    public void Update()
    {
        Vector2 mp = PowerGame.MouseScreenPosition - Generator.GetHypoteticalRect(PowerGame.MouseScreenPosition).GetScale();
        Rectangle hypoRect = Generator.GetHypoteticalRect(mp);
        Vector2 hypoGenPos = hypoRect.GetPosition();

        Generator? genAtRect = PowerGame.UpdateQueueList
            .Where(t => t.id == Generator.Id)
            .FirstOrDefault(tuple => Raylib.CheckCollisionRecs(hypoRect, (tuple.iface as Generator).Rect))
            .iface as Generator;

        Generator? genAtPoint = null;
        if (genAtRect != null && Raylib.CheckCollisionPointRec(mp, genAtRect.Rect))
        {
            genAtPoint = genAtRect;
        }

        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT) && genAtRect == null)
        {
            Generator gen = new(PowerGame.MouseScreenPosition, 2, 10);
            PowerGame.UpdateQueueList.EnqueueAddition((Generator.Id, gen));
            PowerGame.DrawQueueList.EnqueueAddition((Generator.Id, gen));
        }
        else if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT) && genAtPoint != null)
        {
            PowerGame.UpdateQueueList.EnqueueRemoval((Generator.Id, genAtPoint));
            PowerGame.DrawQueueList.EnqueueRemoval((Generator.Id, genAtPoint));
        }

    }
}

public class Generator : IUpdate, IDraw
{
    public const string Id = "GEN";
    public float GenerationRate { get; set; }
    public float SafetyLimit { get; private set; }
    public float CurrentPower { get; private set; }

    public Rectangle Rect { get; init; }

    private static readonly Vector2 Scale = new Vector2(50f, 50f);
    private static readonly Color Color = Raylib.RAYWHITE;
    private static readonly Color OverflowColor = Raylib.RED;

    private bool _isOverflowing => CurrentPower > SafetyLimit;

    public Generator(Vector2 position, float generationRate, float safetyLimit)
    {
        Rect = GetHypoteticalRect(position);
        GenerationRate = generationRate;
        SafetyLimit = safetyLimit;
    }

    public static Rectangle GetHypoteticalRect(Vector2 position) => new(position.X, position.Y, Scale.X, Scale.Y);

    public void Update()
    {
        CurrentPower += GenerationRate * PowerGame.FrameTime;

        if (_isOverflowing && CurrentPower > SafetyLimit * 2f)
        {
            PowerGame.UpdateQueueList.EnqueueRemoval((Id, this));
            PowerGame.DrawQueueList.EnqueueRemoval((Id, this));
        }
    }

    public void Draw()
    {
        if (!_isOverflowing)
        {
            Raylib.DrawRectangle((int)Rect.x, (int)Rect.y, (int)Scale.X, (int)Scale.Y, Color);
        }
        else
        {
            Raylib.DrawRectangle((int)Rect.x, (int)Rect.y, (int)Scale.X, (int)Scale.Y, OverflowColor);
        }
    }
}

public static class Extensions
{
    public static Vector2 GetPosition(this Rectangle rect) => new Vector2(rect.x, rect.y);
    public static Vector2 GetScale(this Rectangle rect) => new Vector2(rect.width, rect.height);
}