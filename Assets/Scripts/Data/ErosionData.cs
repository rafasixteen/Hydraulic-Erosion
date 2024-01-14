using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Data/Erosion")]
    public class ErosionData : ScriptableObject
    {
        public void Initialize()
        {
            Iterations = 10_000;
            IterationsPerFrame = 100;
            Animate = false;

            ErosionRadius = 3;
            EvaporationSpeed = 0.1f;
            Inertia = 0.3f;
            SedimentCapacityFactor = 3f;
            MinimumSedimentCapacity = 0.01f;
            ErosionSpeed = 0.3f;
            DepositionSpeed = 0.3f;
        }

        public const float MINIMUM_VOLUME = 0.01f;
        public const float GRAVITY = 4f;

        public int Iterations;
        public int IterationsPerFrame;
        public bool Animate;

        public int ErosionRadius;
        public float EvaporationSpeed;
        public float Inertia; //Higher values make channel turns smoother.
        public float SedimentCapacityFactor; // Multiplier For How Much Sediment A Droplet Can Carry
        public float MinimumSedimentCapacity; // Used To Prevent Carry Capacity Getting Too Close To Zero On Flatter Terrain
        public float ErosionSpeed; // Erosion speed (how fast the soil is removed).
        public float DepositionSpeed; // Deposition speed (how fast the extra sediment is dropped).
    }
}