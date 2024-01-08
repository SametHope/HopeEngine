using Raylib_CsLo;
using System.Diagnostics;

namespace HopeEngine;

using static Raylib;

/// <summary>
/// Provides the means to start the programs main loop and related utilities.
/// </summary>
public static class Looper
{
    /// <summary>
    /// This interface will be called every frame. When it is set to null, the next frame will not execute and the loop will terminate.
    /// </summary>
    public static IGame? IGame { get; set; }

    /// <summary>
    /// When set to false, <see cref="IGame"/>'s draw method will not be called. 
    /// <para>However <see cref="BeginDrawing"/> and <see cref="EndDrawing"/> will keep getting called.
    /// <br>This is because RayLib registers frame when <see cref="EndDrawing"/> is called.</br></para>
    /// </summary>
    public static bool ShouldDraw { get; set; } = true;

    /// <summary>
    /// Amount of iterations aka frames that have been completed.
    /// </summary>
    public static long FrameCount { get; private set; }

    /// <summary>
    /// Time that has passed between last frames completion to current frames start has passed.
    /// </summary>
    public static float FrameTime { get; private set; }

    /// <summary>
    /// Frames per second. Accounts for previous frames which makes it more accurate and dynamic than calculations made with <see cref="FrameTime"/>.
    /// </summary>
    public static int FPS { get; private set; }


    public static void Start()
    {
        LoopEventAttribute.InvokeMethodsOnAllInstances(LoopEventType.Before);

        IGame?.Initialize();
        IGame?.Start();

        Debug.Assert(IsWindowReady(), "Raylib window is not initialized.");

        while (!(IGame == null || WindowShouldClose()))
        {
            IGame?.Update();

            BeginDrawing();

            if(ShouldDraw) IGame?.Draw();

            EndDrawing();

            FrameCount++;
            FrameTime = GetFrameTime();
            FPS = GetFPS();
        }

        IGame?.Deinitialize();
    }

    /// <summary>
    /// Resets all fields to their default values.
    /// </summary>
    public static void Purge()
    {
        IGame = null;
        ShouldDraw = true;
        
        FrameCount = 0;
        FrameTime = 0;
        FPS = 0;
    }
}
