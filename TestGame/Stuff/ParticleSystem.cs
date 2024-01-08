﻿using Raylib_CsLo;
using System.Numerics;

namespace HopeEngine.TestGame;

public struct Particle
{
    public Vector2 Pos;
    public Vector2 Vel;
    public Color Col;
}

public unsafe class ParticleSystem : IUpdate, IDraw
{
    private const int _P_COUNT = 80000;
    private const int _P_SPAWN_RADIUS = 1000 * 2;
    private const int _P_MASS = 1;
    private const float _P_DRAG_FACTOR = 0.98f;

    private static readonly Particle[] Particles = new Particle[_P_COUNT];

    private bool _shouldDrawParticles = true;
    private int _divider = 2;

    public DrawMode DrawMode => DrawMode.World;

    public ParticleSystem()
    {
        InitializeParticles();
    }

    private void InitializeParticles()
    {
        for (int i = 0; i < _P_COUNT; i++)
        {
            // Set particle positions to a random circle at the center of the screen
            Particles[i].Pos = Engine.GetScreenCenter() + ParticleUtils.GetRandomPointInCircle(_P_SPAWN_RADIUS);
            //Particles[i].Col = ParticleUtils.GetRandomColor();
            Particles[i].Col = Raylib.RAYWHITE;
        }
    }

    public void Update(float deltaTime)
    {
        // Toggle draw if needed
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_D)) _shouldDrawParticles = !_shouldDrawParticles;

        // Reduct draw calls if needed
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN)) _divider = _divider * 2;
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
        {
            // Preven't setting divider below 1
            _divider = _divider / 2;
            if (_divider < 1) _divider = 1;
        }

        // Setup click force factor
        float clickForceFactor;
        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT)) clickForceFactor = 100f;
        else if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT)) clickForceFactor = -25f;
        else clickForceFactor = 0f;

        // Cache mouse position
        Vector2 mp = Engine.MouseScreenPosition;

        for (int i = 0; i < _P_COUNT; i++)
        {
            // Skip particles that are exactly at the same position just in case
            // so we dont 0/0 or other things that may result with NaN
            var Distance = Vector2.Distance(mp, Particles[i].Pos);
            if (Distance == 0) continue;

            // Apply attraction
            Particles[i].Vel += ParticleUtils.GetAttraction(mp, 10f, Particles[i].Pos, _P_MASS, 3f, 1.3f, 0) * clickForceFactor;

            // Apply drag and velocity changes
            Particles[i].Vel += -Particles[i].Vel * _P_DRAG_FACTOR * deltaTime;
            Particles[i].Pos += Particles[i].Vel * deltaTime;
        }
    }

    public void Draw()
    {
        DrawTexts();
        if (_shouldDrawParticles)
        {
            DrawParticles();
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
            $"{Raylib.GetFPS()} : {Engine.DeltaTime}ms",
            $"Drawing {_P_COUNT/_divider} / {_P_COUNT} particles",
            $"Press d to toggle draw calls",
            $"Use left click to attract particles",
            $"Use right click to repel particles",
            $"Press up to increase draw calls",
            $"Press down to decrease draw calls",
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

public static class ParticleUtils
{
    public static Color GetRandomColor()
    {
        return new Color(Raylib.GetRandomValue(0, 255), Raylib.GetRandomValue(0, 255), Raylib.GetRandomValue(0, 255), 255);
    }

    public static Vector2 GetRandomPointInCircle(float radius = 1f)
    {
        double angleInRadians = Raylib.GetRandomValue(0, 360) * Math.PI / 180.0;
        double randomDistance = Math.Sqrt(Raylib.GetRandomValue(0, 1000000)) / 1000.0 * radius;

        float offsetX = (float)(randomDistance * Math.Cos(angleInRadians));
        float offsetY = (float)(randomDistance * Math.Sin(angleInRadians));

        return new Vector2(offsetX, offsetY);
    }

    public static Vector2 GetAttraction(Vector2 pos, float mass, Vector2 oPos, float oMass, float gForce = 10f, float pVal = 1.5f, float extraF = 0f)
    {
        var gaDir = pos - oPos;
        var gaDist = gaDir.Length();

        return Vector2.Normalize(gaDir) * gForce * ((mass * oMass) / (gaDist * pVal) + extraF);
    }
}
