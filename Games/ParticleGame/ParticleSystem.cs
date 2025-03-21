﻿using Raylib_CsLo;
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

    public ParticleSystem()
    {
        ResetParticles();
    }

    private void ResetParticles()
    {
        Raylib.SetMousePosition((int)ParticleGame.WindowCenter.X, (int)ParticleGame.WindowCenter.Y);

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
        ProcessInputs();

        float clickForceFactor = CalculateClickForceFactor();
        UpdateParticleForces(clickForceFactor);
    }

    private void ProcessInputs()
    {
        // Toggle draw if needed
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_D)) _shouldDrawParticles = !_shouldDrawParticles;

        // Reset if needed
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_R)) ResetParticles();

        // Modify draw calls if needed
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN)) _divider = _divider * 2;
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
        {
            // Preven't setting divider below 1
            _divider = _divider / 2;
            if (_divider < 1) _divider = 1;
        }
    }

    private float CalculateClickForceFactor()
    {
        float clickForceFactor;
        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT)) clickForceFactor = 100f;
        else if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT)) clickForceFactor = -17.5f;
        else clickForceFactor = 0f;

        // This could be way nicer but it is ok I like it to be more explicit
        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT) && Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT)) clickForceFactor = 10f;

        return clickForceFactor;
    }

    private void UpdateParticleForces(float clickForceFactor)
    {
        // Cache mouse position just in case
        Vector2 mp = ParticleGame.MouseScreenPosition;

        Parallel.For(0, _P_COUNT, i =>
        {
            // Skip particles that are exactly at the same position just in case
            // so we dont 0/0 or other things that may result with NaN
            var Distance = Vector2.Distance(mp, Particles[i].Pos);
            if (Distance == 0) return;

            // Apply attraction
            Particles[i].Vel += ParticleUtils.GetAttraction(mp, 10f, Particles[i].Pos, _P_MASS, 4f, 1.35f, 0) * clickForceFactor;

            // Apply drag 
            Particles[i].Vel += -Particles[i].Vel * _P_DRAG_FACTOR;

            // Use velocity
            Particles[i].Pos += Particles[i].Vel * ParticleGame.FrameTime;
        });
    }

    public void Draw()
    {
        if (_shouldDrawParticles)
        {
            DrawParticles();
        }
        DrawTexts();
    }

    private void DrawTexts()
    {
        int fontSize = 15;
        int lineHeight = fontSize + 2;
        int baseX = 10;
        int baseY = 10;

        string[] lines =
            {
            $"{ParticleGame.FPS} FPS : {ParticleGame.FrameTime} ms",
            $"Drawing {_P_COUNT/_divider} / {_P_COUNT} particles",
            $"Press d to toggle draw calls",
            $"Use left click to attract particles",
            $"Use right click to repel particles",
            $"Press up to increase draw calls",
            $"Press down to decrease draw calls",
            $"Press r to reset particles",
            $"Press f11 to resize the window",
            $"Press escape to quit",
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
}