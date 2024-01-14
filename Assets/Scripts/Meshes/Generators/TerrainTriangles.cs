using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Meshes.Generators
{
    public struct TerrainTriangles : IMeshGenerator
    {
        public int Resolution { get; set; }

        public readonly int VertexCount => Resolution * Resolution;

        public readonly int IndexCount => (Resolution - 1) * (Resolution - 1) * 6;

        public readonly int JobLength => IndexCount / 6;

        public readonly Bounds Bounds => new();

        public int Size { get; set; }

        public readonly float Scale { get; }

        public NativeArray<float> Heightmap { get; set; }

        public float HeightMultipler { get; set; }

        public readonly void Execute<S>(int i, S streams) where S : struct, IMeshStreams
        {
            int x = i % (Resolution - 1);
            int y = i / (Resolution - 1);

            int triangleIndex = i * 2;

            int a = y * Resolution + x;
            int b = a + 1;
            int c = (y + 1) * Resolution + x;
            int d = c + 1;

            streams.SetTriangle(triangleIndex + 0, new int3(c, b, a));
            streams.SetTriangle(triangleIndex + 1, new int3(c, d, b));
        }
    }
}