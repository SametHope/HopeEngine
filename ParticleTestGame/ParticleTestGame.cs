using Raylib_CsLo;

namespace HopeEngine.ParticleTestGame;

public class ParticleTestGame : Game
{
    public override string WindowTitle => "Particle Test Game";

    public override int WindowWidth => 1280;

    public override int WindowHeight => 720;

    public override int TargetFPS => 120;

    public override Color BackgroundColor => Raylib.BLACK;

    public ParticleTestGame()
    {
        Raylib.SetExitKey(KeyboardKey.KEY_ESCAPE);
    }

    public override void LoadGame()
    {
        var pm = new ParticleSystem();
        Engine.SortedUpdateQueue[CallOrder.Normal].EnqueueAddition(pm);
        Engine.SortedDrawQueue[CallOrder.Normal].EnqueueAddition(pm);
    }
}
