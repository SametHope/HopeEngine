using Raylib_CsLo;

namespace HopeEngine.ParticleGame;

public class WindowResizeAgent : IUpdate
{
    private KeyboardKey _resizeKey;
    private Action? _onResize;

    public WindowResizeAgent(KeyboardKey resizeKey, Action? onResize = null)
    {
        _resizeKey = resizeKey;
        _onResize = onResize;
    }

    public void Update()
    {
        // Handle screen size changes
        // Note that resizing the window can take time which is afaik not possible to detect 
        // so we end up calling the action too early quite often and not en up not reflecting the changes properly
        // but I don't really care
        if (Raylib.IsKeyPressed(_resizeKey))
        {
            if (ParticleGame.WindowWidth == 1280 && ParticleGame.WindowHeight == 720 && !Raylib.IsWindowFullscreen())
            {
                ParticleGame.WindowWidth = 1920;
                ParticleGame.WindowHeight = 1080;
                Raylib.SetWindowSize(ParticleGame.WindowWidth, ParticleGame.WindowHeight);
                Raylib.ToggleFullscreen();
            }
            else
            {
                ParticleGame.WindowWidth = 1280;
                ParticleGame.WindowHeight = 720;
                Raylib.SetWindowSize(ParticleGame.WindowWidth, ParticleGame.WindowHeight);
                Raylib.ToggleFullscreen();
            }

            _onResize?.Invoke();
        }
    }
}