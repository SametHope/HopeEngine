using Raylib_CsLo;
using System.Numerics;

namespace HopeEngine.TestGame
{
    public class ParticleSystem : IUpdate, IDraw
    {
        private const int ParticleCount = 100000;
        private const int SpawnRadiusMultiplier = 5;
        private const int ParticleMass = 1;
        public const float DragFactor = 0.98f;

        private static readonly Vector2[] ParticlePositions = new Vector2[ParticleCount];
        private static readonly Vector2[] ParticleVelocities = new Vector2[ParticleCount];
        private static readonly Color[] ParticleColors = new Color[ParticleCount];

        private float Force;
        private float Distance;

        private bool DoDraw = true;

        public ParticleSystem()
        {
            InitializeParticles();
        }

        private void InitializeParticles()
        {
            int spawnRadius = Engine.Game.WindowWidth * SpawnRadiusMultiplier;

            for (int i = 0; i < ParticleCount; i++)
            {
                ParticlePositions[i] = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2) + RandomPointInCircle(spawnRadius) * 25;
                ParticleColors[i] = new Color(Raylib.GetRandomValue(0, 255), Raylib.GetRandomValue(0, 255), Raylib.GetRandomValue(0, 255), 100);
                ParticleVelocities[i] = Vector2.Zero;
            }
        }

        public DrawMode DrawMode => DrawMode.World;

        public void Update(float deltaTime)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_D)) DoDraw = !DoDraw;
            Force = Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT) ? 100f : Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT) ? -20f : 0f;
            var mp = Engine.MouseScreenPosition;
            for (int i = 0; i < ParticleCount; i++)
            {
                Distance = Vector2.Distance(mp, ParticlePositions[i]);
                if (Distance != 0)
                {
                    ParticleVelocities[i] += GetAttraction(mp, 10f, ParticlePositions[i], ParticleMass, 3f, 1.3f, 0) * Force;

                    // Update
                    ParticleVelocities[i] += -ParticleVelocities[i] * DragFactor * deltaTime;
                    ParticlePositions[i] += ParticleVelocities[i] * deltaTime;
                }
            }
        }

        private static Vector2 GetAttraction(Vector2 pos, float mass, Vector2 oPos, float oMass, float gForce = 10f, float pVal = 1.5f, float extraF = 0f)
        {
            var gaDir = pos - oPos;
            var gaDist = gaDir.Length();

            return Vector2.Normalize(gaDir) * gForce * ((mass * oMass) / (gaDist * pVal) + extraF);
        }

        public void Draw()
        {
            Raylib.DrawText($"{Raylib.GetFPS()} : {Engine.DeltaTime}ms", 10, 10, 18, Raylib.GREEN);
            Raylib.DrawText($"{ParticleCount} particles", 10, 10 + 18 + 2, 18, Raylib.GREEN);

            if (!DoDraw) return;
            for (int i = 0; i < ParticleCount; i++)
            {
                //Raylib.DrawCircleV(ParticlePositions[i], 2, Raylib.RAYWHITE);
                Raylib.DrawPixelV(ParticlePositions[i], Raylib.RAYWHITE);
            }
        }

        private static Vector2 RandomPointInCircle(float radius = 1f)
        {
            double angle = Raylib.GetRandomValue(0, 360) * (Math.PI / 180f);
            double distance = Raylib.GetRandomValue(0, (int)(radius * 1000)) / 1000f;

            float offsetX = (float)(distance * Math.Cos(angle));
            float offsetY = (float)(distance * Math.Sin(angle));

            return new Vector2(offsetX, offsetY);
        }
    }
}
