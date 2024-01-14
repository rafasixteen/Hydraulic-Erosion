using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Meshes.Streams
{
    public struct SingleStream : IMeshStreams
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Stream
        {
            public float3 position;
            public float3 normal;
            public float4 tangent;
            public float2 texCoord;
        }

        [NativeDisableContainerSafetyRestriction] NativeArray<Stream> stream;
        [NativeDisableContainerSafetyRestriction] NativeArray<int3> triangles;


        public void Setup(Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount)
        {
            NativeArray<VertexAttributeDescriptor> vertexAttributes = new(length: 4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

            vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, dimension: 3, stream: 0);
            vertexAttributes[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, dimension: 3, stream: 0);
            vertexAttributes[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, dimension: 4, stream: 0);
            vertexAttributes[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, dimension: 2, stream: 0);

            meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
            vertexAttributes.Dispose();

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt32);

            meshData.subMeshCount = 1;

            SubMeshDescriptor subMeshDescriptor = new(0, indexCount, MeshTopology.Triangles)
            {
                bounds = bounds,
                vertexCount = vertexCount
            };

            meshData.SetSubMesh(0, subMeshDescriptor, MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices);

            stream = meshData.GetVertexData<Stream>(stream: 0);
            triangles = meshData.GetIndexData<int>().Reinterpret<int3>(4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex vertex) => stream[index] = new Stream
        {
            position = vertex.position,
            normal = vertex.normal,
            tangent = vertex.tangent,
            texCoord = vertex.texCoord,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTriangle(int index, int3 triangle) => triangles[index] = triangle;
    }
}