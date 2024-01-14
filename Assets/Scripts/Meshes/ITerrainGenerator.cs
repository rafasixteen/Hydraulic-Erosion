using Unity.Collections;
using UnityEngine;

namespace Assets.Scripts.Meshes
{
    public interface ITerrainGenerator
    {
        void Execute<S>(S streams) where S : struct, IMeshStreams;

        int Resolution { get; set; }

        int Size { get; set; }

        NativeArray<float> Heightmap { get; set; }

        NativeArray<Vector3> TerrainMeshVertices { get; set; }

        float HeightMultipler { get; set; }

        float Scale { get; }

        int VertexCount { get; }

        int IndexCount { get; }

        int JobLength { get; }

        Bounds Bounds { get; }
    }
}