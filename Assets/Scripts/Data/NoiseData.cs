using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Data/Noise")]
    public class NoiseData : ScriptableObject
    {
        public void Initialize()
        {
            NoiseType = FastNoiseLite.NoiseType.OpenSimplex2S;
            FractalType = FastNoiseLite.FractalType.FBm;
            Octaves = 8;
            Lacunarity = 2f;
            Gain = 0.5f;
            Frequency = 0.03f;
            Seed = 1337;
        }

        public void RandomSeed()
        {
            System.Random random = new();
            Seed = random.Next(int.MinValue, int.MaxValue);
        }

        public FastNoiseLite Noise = new();

        private FastNoiseLite.NoiseType _noiseType;
        public FastNoiseLite.NoiseType NoiseType
        {
            get => _noiseType;
            set
            {
                _noiseType = value;
                Noise.SetNoiseType(_noiseType);
            }
        }

        private FastNoiseLite.FractalType _fractalType;
        public FastNoiseLite.FractalType FractalType
        {
            get => _fractalType;
            set
            {
                _fractalType = value;
                Noise.SetFractalType(_fractalType);
            }
        }

        private int _octaves;
        public int Octaves
        {
            get => _octaves;
            set
            {
                _octaves = value;
                Noise.SetFractalOctaves(_octaves);
            }
        }

        private float _lacunarity;
        public float Lacunarity
        {
            get => _lacunarity;
            set
            {
                _lacunarity = value;
                Noise.SetFractalLacunarity(_lacunarity);
            }
        }

        private float _gain;
        public float Gain
        {
            get => _gain;
            set
            {
                _gain = value;
                Noise.SetFractalGain(_gain);
            }
        }

        private float _frequency;
        public float Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
                Noise.SetFrequency(_frequency);
            }
        }

        private int _seed;
        public int Seed
        {
            get => _seed;
            set
            {
                _seed = value;
                Noise.SetSeed(_seed);
            }
        }
    }
}