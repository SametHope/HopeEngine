using Raylib_CsLo;
using System.Diagnostics;
using System.Numerics;

namespace HopeEngine;

/// <summary>
/// Provides a game loop and some general utilities.
/// </summary>
public static class Engine
{
    #region General Fields
    public static Game Game { get; private set; }

    /// <summary>
    /// All entities that will be updated with their order to update.
    /// </summary>
    public static readonly SortedList<CallOrder, QueueList<IUpdate>> SortedUpdateQueue = new();
    /// <summary>
    /// All entities that will be updated with their order to draw.
    /// </summary>
    public static readonly SortedList<CallOrder, QueueList<IDraw>> SortedDrawQueue = new();

    /// <summary>
    /// When true, next frame will not execute and app will terminate.
    /// </summary>
    public static bool ShouldQuit { get; set; }

    /// <summary>
    /// The main 2D camera of the game.
    /// </summary>
    // Note: Do not add { get; private set; } because it does not play nice with structs 
    public static Camera2D Camera;
    #endregion

    #region Per-Frame Fields
    /// <summary>
    /// Updated at the start of each frame.
    /// </summary>
    public static Vector2 MouseScreenPosition { get; private set; }
    /// <summary>
    /// Updated at the start of each frame, uses default camera to get mouses world position.
    /// </summary>
    public static Vector2 MouseWorldPosition { get; private set; }
    /// <summary>
    /// Updated at the start of each frame, time that has passed since drawing last frame.
    /// </summary>
    public static float DeltaTime { get; set; }
    /// <summary>
    /// Amount of frames that have been run.
    /// </summary>
    public static long FrameCount { get; private set; }
    #endregion

    #region Config Fields
    private static bool _didSetup = false;
    #endregion 

    /// <summary>
    /// Setup the engine.
    /// </summary>
    public static void Setup()
    {
        Debug.Assert(!_didSetup, "Engine was already setup");
        InitializeSortedQueues();

        Camera = new Camera2D();
        Camera.zoom = 1;
        _didSetup = true;
    }

    /// <summary>
    /// Initializes the window and calls <see cref="Game.LoadGame"/> on <paramref name="game"/>, effectively starting gameloop.
    /// </summary>
    public static void Start(Game game)
    {
        Debug.Assert(_didSetup, "Engine was not setup.");
        Debug.Assert(game != null, "Game was not provided.");

        Raylib.InitWindow(game.WindowWidth, game.WindowHeight, game.WindowTitle);
        Raylib.SetTargetFPS(game.TargetFPS);

        Game = game;

        OnGameStartAttribute.InvokeMethodsOnAllInstances();
        game.LoadGame();

        // Process all addition requests from queues in the correct order
        ProcessUpdateQueueAdditions();
        ProcessDrawQueueAdditions();

        while (!(ShouldQuit || Raylib.WindowShouldClose()))
        {
            RunMainLoopLogic();
            FrameCount++;
        }

        Raylib.CloseWindow();
    }

    private static void RunMainLoopLogic()
    {
        // Setup per-frame variables
        DeltaTime = Raylib.GetFrameTime();
        MouseScreenPosition = Raylib.GetMousePosition();
        MouseWorldPosition = Raylib.GetScreenToWorld2D(MouseScreenPosition, Camera);

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Game.BackgroundColor);

        // Update all currently registered update interfaces in the correct order
        ProcessUpdateQueue();

        Raylib.BeginMode2D(Camera);

        // Draw all currently registered world draw interfaces in the correct order
        ProcessDrawQueue(DrawMode.World);

        Raylib.EndMode2D();

        // Draw all currently registered screen draw in the correct order
        ProcessDrawQueue(DrawMode.Screen);

        // Process all removal requests from queues
        ProcessUpdateQueueRemovals();
        ProcessDrawQueueRemovals();

        // Process all addition requests from queues
        ProcessUpdateQueueAdditions();
        ProcessDrawQueueAdditions();

        Raylib.EndDrawing();
    }

    /// <summary>
    /// Returns screen space center of the screen.
    /// </summary>
    public static Vector2 GetScreenCenter() => new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);

    #region Queue Methods
    public static void InitializeSortedQueues()
    {
        SortedUpdateQueue.Add(CallOrder.Early, new());
        SortedUpdateQueue.Add(CallOrder.Normal, new());
        SortedUpdateQueue.Add(CallOrder.Late, new());

        SortedDrawQueue.Add(CallOrder.Early, new());
        SortedDrawQueue.Add(CallOrder.Normal, new());
        SortedDrawQueue.Add(CallOrder.Late, new());
    }

    private static void ProcessUpdateQueueAdditions()
    {
        foreach (var updateQueue in SortedUpdateQueue.Values)
        {
            updateQueue.ProcessPendingAdditions();
        }
    }
    private static void ProcessUpdateQueueRemovals()
    {
        foreach (var updateQueue in SortedUpdateQueue.Values)
        {
            updateQueue.ProcessPendingRemovals();
        }
    }

    private static void ProcessDrawQueueAdditions()
    {
        foreach (var drawQueue in SortedDrawQueue.Values)
        {
            drawQueue.ProcessPendingAdditions();
        }
    }
    private static void ProcessDrawQueueRemovals()
    {
        foreach (var drawQueue in SortedDrawQueue.Values)
        {
            drawQueue.ProcessPendingRemovals();
        }
    }

    private static void ProcessUpdateQueue()
    {
        foreach (var queueList in SortedUpdateQueue.Values)
        {
            foreach (var IUpdate in queueList)
            {
                IUpdate.Update(DeltaTime);
            }
        }
    }
    private static void ProcessDrawQueue(DrawMode mode)
    {
        foreach (var queueList in SortedDrawQueue.Values)
        {
            foreach (var IDraw in queueList)
            {
                if (IDraw.DrawMode == mode)
                {
                    IDraw.Draw();
                }
            }
        }
    }
    #endregion
}
