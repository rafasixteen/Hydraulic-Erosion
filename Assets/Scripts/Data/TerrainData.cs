using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Data/Terrain")]
    public class TerrainData : ScriptableObject
    {
        public void Initialize()
        {
            Resolution = 256;
            Size = 64;
            HeightMultiplier = 20f;
        }

        public int VertexCount
        {
            get => Resolution * Resolution;
        }

        public int IndexCount
        {
            get => (Resolution - 1) * (Resolution - 1) * 6;
        }

        private int _resolution;
        public int Resolution
        {
            get => _resolution;
            set
            {
                _resolution = Mathf.CeilToInt((float)value / 8) * 8;
                Heightmap = new float[_resolution * _resolution];
            }
        }

        private int _size;
        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                EventManager.TerrainSizeChanged(Size);
            }
        }

        public float Scale
        {
            get => (float)Size / Resolution;
        }

        private float _heightMultiplier;
        public float HeightMultiplier
        {
            get => _heightMultiplier;
            set
            {
                _heightMultiplier = value;
            }
        }

        public float[] Heightmap { get; set; }
    }
}