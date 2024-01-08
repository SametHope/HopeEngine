using Raylib_CsLo;
using System.Diagnostics;

namespace HopeEngine;

using static Raylib;

/// <summary>
/// Provides the means to start the games main loop.
/// </summary>
public static class Looper
{
    /// <summary>
    /// This interface will be called every frame. When it is set to null, the next frame will not execute and the loop will terminate.
    /// </summary>
    public static IGame? IGame { get; set; }

    /// <summary>
    /// Starts the looper, calling all methods in <see cref="IGame"/> with order.
    /// </summary>
    public static void Start()
    {
        Debug.Assert(IGame != null, "Game is not set.");

        IGame?.Initialize();
        IGame?.Start();

        Debug.Assert(IsWindowReady(), "Raylib window is not initialized.");

        while (!(IGame == null || WindowShouldClose()))
        {
            IGame?.Update();

            BeginDrawing();

            IGame?.Draw();

            EndDrawing();
        }

        IGame?.Deinitialize();
    }
}