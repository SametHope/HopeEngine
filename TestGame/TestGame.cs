using Raylib_CsLo;

namespace HopeEngine.TestGame;

public class TestGame : Game
{
    public override string WindowTitle => "Test Game";

    public override int WindowWidth => 1280;

    public override int WindowHeight => 720;

    public override int TargetFPS => 120;

    public override Color BackgroundColor => Raylib.BLACK;

    public TestGame()
    {
        Raylib.SetExitKey(KeyboardKey.KEY_ESCAPE);

        Engine.SortedDrawQueue[CallOrder.Late].EnqueueAddition(new FPSDrawer());
    }

    public override void LoadGame()
    {
        var pm = new ParticleSystem();
        Engine.SortedUpdateQueue[CallOrder.Normal].EnqueueAddition(pm);
        Engine.SortedDrawQueue[CallOrder.Normal].EnqueueAddition(pm);


    }
}
