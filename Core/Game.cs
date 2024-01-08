using Raylib_CsLo;

namespace HopeEngine;

public abstract class Game
{
    public abstract string WindowTitle { get; }
    public abstract int WindowWidth { get; }
    public abstract int WindowHeight { get; }
    public abstract int TargetFPS { get; }
    public abstract Color BackgroundColor { get; }
    
    public abstract void LoadGame();
}