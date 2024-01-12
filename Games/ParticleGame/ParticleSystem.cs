using Raylib_CsLo;
using System.Numerics;

namespace HopeEngine.ParticleGame;

public class ParticleSystem : IUpdate, IDraw
{
    private const int _P_COUNT = 128000 * 2;
    private const int _P_SPAWN_RADIUS = 1000 * 2;
    private const int _P_MASS = 1;
    private const float _P_DRAG_FACTOR = 0.02f;

    private static readonly Particle[] Particles = new Particle[_P_COUNT];

    private bool _shouldDrawParticles = true;
    private int _divider = 2 * 2;

    private static readonly Dictionary<int, (Vector2 aPos, float aForce)> Attractors = new();
    private void TryAttractorChange(KeyboardKey key, int id, Vector2 mp, float leftF, float rightF)
    {
        if (Raylib.IsKeyDown(key))
        {
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
            {
                Attractors[id] = (mp, leftF);
            }
            else if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT))
            {
                Attractors[id] = (mp, rightF);
            }
            else if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_MIDDLE))
            {
                if (Attractors.ContainsKey(id)) Attractors.Remove(id);
            }
        }
    }

    public ParticleSystem()
    {
        InitializeParticles();
    }

    private void InitializeParticles()
    {
        for (int i = 0; i < _P_COUNT; i++)
        {
            // Set particle positions to a random circle at the center of the screen
            Particles[i].Pos = ParticleGame.WindowCenter + ParticleUtils.GetRandomPointInCircle(_P_SPAWN_RADIUS);
            //Particles[i].Col = ParticleUtils.GetRandomColor();
            Particles[i].Col = Raylib.RAYWHITE;
            Particles[i].Vel = Vector2.Zero;
        }
    }

    public void Update()
    {
        // Toggle draw if needed
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_D)) _shouldDrawParticles = !_shouldDrawParticles;

        // Restart draw if needed
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_R)) InitializeParticles();

        // Reduct draw calls if needed
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN)) _divider = _divider * 2;
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
        {
            // Preven't setting divider below 1
            _divider = _divider / 2;
            if (_divider < 1) _divider = 1;
        }

        // Cache mouse position
        Vector2 mp = ParticleGame.MouseScreenPosition;

        // 1
        TryAttractorChange(KeyboardKey.KEY_ONE, 1, mp, 100f, -100f / 1f);
        TryAttractorChange(KeyboardKey.KEY_TWO, 2, mp, 100f, -100f / 1f);
        TryAttractorChange(KeyboardKey.KEY_THREE, 3, mp, 100f, -100f / 1f);
        TryAttractorChange(KeyboardKey.KEY_FOUR, 4, mp, 100f, -100f / 1f);
        TryAttractorChange(KeyboardKey.KEY_FIVE, 5, mp, 100f, -100f / 1f);
        TryAttractorChange(KeyboardKey.KEY_SIX, 6, mp, 250f, -250f / 1f);
        TryAttractorChange(KeyboardKey.KEY_SEVEN, 7, mp, 250f, -250f / 1f);
        TryAttractorChange(KeyboardKey.KEY_EIGHT, 8, mp, 250f, -250f / 1f);
        TryAttractorChange(KeyboardKey.KEY_NINE, 9, mp, 500f, -500f / 1f);
        TryAttractorChange(KeyboardKey.KEY_ZERO, 0, mp, 1000f, -1000f / 1f);

        Parallel.For(0, _P_COUNT, i =>
        {
            // Skip particles that are exactly at the same position just in case
            // so we dont 0/0 or other things that may result with NaN
            var Distance = Vector2.Distance(mp, Particles[i].Pos);
            if (Distance == 0) return;

            foreach (var kvp in Attractors)
            {
                Particles[i].Vel += ParticleUtils.GetAttraction(kvp.Value.aPos, 10f, Particles[i].Pos, _P_MASS, 4f, 1.35f, 0) * kvp.Value.aForce;
            }

            // Apply attraction
            //Particles[i].Vel += ParticleUtils.GetAttraction(mp, 10f, Particles[i].Pos, _P_MASS, 4f, 1.35f, 0) * clickForceFactor;

            // Apply drag 
            Particles[i].Vel += -Particles[i].Vel * _P_DRAG_FACTOR;

            // Use velocity
            Particles[i].Pos += Particles[i].Vel * ParticleGame.FrameTime;
        });
    }

    public void Draw()
    {
        DrawTexts();
        if (_shouldDrawParticles)
        {
            DrawAttractors();
            DrawParticles();
            DrawAttractorLabels();
        }
    }

    private void DrawTexts()
    {
        int fontSize = 16;
        int lineHeight = fontSize + 2;
        int baseX = 10;
        int baseY = 10;

        string[] lines =
            {
            $"{ParticleGame.FPS} : {ParticleGame.FrameTime}ms",
            $"Drawing {_P_COUNT/_divider} / {_P_COUNT} particles",
            $"Press d to toggle draw calls",
            $"Use left click to attract particles",
            $"Use right click to repel particles",
            $"Press up to increase draw calls",
            $"Press down to decrease draw calls",
            $"Press r to restart",
            };

        for (int i = 0; i < lines.Length; i++)
        {
            Raylib.DrawText(lines[i], baseX, baseY + i * lineHeight, fontSize, Raylib.DARKGREEN);
        }
    }

    private void DrawParticles()
    {
        for (int i = 0; i < _P_COUNT / _divider; i++)
        {
            Raylib.DrawPixelV(Particles[i].Pos, Particles[i].Col);
        }
    }

    private void DrawAttractors()
    {
        foreach (var kvp in Attractors)
        {
            Raylib.DrawCircleV(kvp.Value.aPos, MathF.Abs(kvp.Value.aForce) / 20 + 1, kvp.Value.aForce > 0 ? Raylib.RED : Raylib.BLUE);
        }
    }
    private void DrawAttractorLabels()
    {
        foreach (var kvp in Attractors)
        {
            Raylib.DrawText(kvp.Key.ToString(), (int)kvp.Value.aPos.X, (int)kvp.Value.aPos.Y, 16, Raylib.GREEN);
        }
    }
}