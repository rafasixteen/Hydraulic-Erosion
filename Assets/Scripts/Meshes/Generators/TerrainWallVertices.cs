using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Meshes.Generators
{
    public struct TerrainWallVertices : ITerrainGenerator
    {
        public int Resolution { get; set; }

        public int Size { get; set; }

        public readonly float Scale => (float)Size / Resolution;

        public readonly int VertexCount => (Resolution * 4 - 4) * 2;

        public readonly int IndexCount => (Resolution - 1) * 24;

        public readonly int JobLength => Resolution * Resolution;

        public readonly Bounds Bounds => default;

        public NativeArray<float> Heightmap { get; set; }

        public float HeightMultipler { get; set; }

        public NativeArray<Vector3> TerrainMeshVertices { get; set; }

        public readonly void Execute<S>(S streams) where S : struct, IMeshStreams
        {
            int index = 0;
            for (int i = 0; i < JobLength; i++)
            {
                int x = i % Resolution;
                int y = i / Resolution;

                bool isBorderVertice = y == 0 || y == (Resolution - 1) || x == 0 || x == (Resolution - 1);

                if (isBorderVertice)
                {
                    Vertex vertex = new();
                    vertex.tangent.xw = new float2(1f, -1f);
                    vertex.position = TerrainMeshVertices[i];

                    streams.SetVertex(index, vertex);
                    index++;

                    vertex.position.y = -10f;
                    streams.SetVertex(index, vertex);
                    index++;
                }
            }
        }
    }
}