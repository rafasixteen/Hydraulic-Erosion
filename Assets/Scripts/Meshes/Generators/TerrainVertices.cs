using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Meshes.Generators
{
    public struct TerrainVertices : IMeshGenerator
    {
        public int Resolution { get; set; }

        public readonly int VertexCount => Resolution * Resolution;

        public readonly int IndexCount => (Resolution - 1) * (Resolution - 1) * 6;

        public readonly int JobLength => VertexCount;

        public readonly Bounds Bounds => new();

        public int Size { get; set; }

        public readonly float Scale => (float)Size / Resolution;

        public NativeArray<float> Heightmap { get; set; }

        public float HeightMultipler { get; set; }

        public readonly void Execute<S>(int i, S streams) where S : struct, IMeshStreams
        {
            int x = i % Resolution;
            int y = i / Resolution;

            Vertex vertex = new();
            vertex.tangent.xw = new float2(1f, -1f);

            vertex.texCoord = new float2(x / (float)(Resolution - 1), y / (float)(Resolution - 1));

            float xPos = (x * Scale) - (Size / 2);
            float yPos = (y * Scale) - (Size / 2);

            vertex.position = new float3(xPos, Heightmap[i] * HeightMultipler, yPos);

            streams.SetVertex(i, vertex);
        }
    }
}