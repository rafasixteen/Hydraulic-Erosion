using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Meshes.Generators
{
    public struct TerrainWallTriangles : ITerrainGenerator
    {
        public int Resolution { get; set; }

        public int Size { get; set; }

        public readonly float Scale => (float)Size / Resolution;

        public readonly int VertexCount => (Resolution * 4 - 4) * 2;

        public readonly int IndexCount => (Resolution - 1) * 24;

        public readonly int JobLength => IndexCount / 6;

        public readonly Bounds Bounds => default;

        public NativeArray<float> Heightmap { get; set; }

        public float HeightMultipler { get; set; }

        public NativeArray<Vector3> TerrainMeshVertices { get; set; }

        public readonly void Execute<S>(S streams) where S : struct, IMeshStreams
        {
            int trianglesPerLine = Resolution - 1;

            int lastSum = (trianglesPerLine * 2) + 2;

            int firstIndex = trianglesPerLine * 2;
            int lastIndex = firstIndex + trianglesPerLine - 1;

            for (int i = 0; i < JobLength; i++)
            {
                int y = i / trianglesPerLine;

                int triangleIndex = i * 2;

                int a, b, c, d;

                if (y == 0) // Front
                {
                    a = i * 2;
                    b = i * 2 + 1;
                    c = (i + 1) * 2;
                    d = (i + 1) * 2 + 1;

                    streams.SetTriangle(triangleIndex + 0, new(a, d, b));
                    streams.SetTriangle(triangleIndex + 1, new(a, c, d));
                }
                else if (y == 3) // Back
                {
                    a = i * 2 - 2;
                    b = i * 2 + 1 - 2;
                    c = (i + 1) * 2 - 2;
                    d = (i + 1) * 2 + 1 - 2;

                    streams.SetTriangle(triangleIndex + 0, new(a, b, c));
                    streams.SetTriangle(triangleIndex + 1, new(c, b, d));
                }
                else if (y == 1) // Left
                {
                    int index = i - trianglesPerLine;
                    bool isSecondQuad = index == 1;
                    bool isFirstQuad = index == 0;

                    a = isFirstQuad ? 1 : isSecondQuad ? 1 + lastSum : 1 + lastSum + (index - 1) * 4;
                    b = a - 1;
                    c = (Resolution * 2) + 1 + index * 4;
                    d = c - 1;

                    streams.SetTriangle(triangleIndex + 0, new(a, c, b));
                    streams.SetTriangle(triangleIndex + 1, new(b, c, d));
                }
                else if (y == 2) // Right
                {
                    int index = i - trianglesPerLine * 2;
                    bool isLastQuad = index == lastIndex - firstIndex;

                    a = trianglesPerLine * 2 + index * 4;
                    b = a + 1;
                    c = isLastQuad ? trianglesPerLine * 2 + 4 + index * 4 - 4 + lastSum : trianglesPerLine * 2 + 4 + index * 4;
                    d = c + 1;

                    streams.SetTriangle(triangleIndex + 0, new(a, c, b));
                    streams.SetTriangle(triangleIndex + 1, new(b, c, d));
                }
            }
        }
    }
}