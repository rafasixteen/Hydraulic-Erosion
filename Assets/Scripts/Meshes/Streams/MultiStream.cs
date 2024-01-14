using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Meshes.Streams
{
    public struct MultiStream : IMeshStreams
    {
        [NativeDisableContainerSafetyRestriction] NativeArray<float3> stream0;
        [NativeDisableContainerSafetyRestriction] NativeArray<float3> stream1;
        [NativeDisableContainerSafetyRestriction] NativeArray<float4> stream2;
        [NativeDisableContainerSafetyRestriction] NativeArray<float2> stream3;

        [NativeDisableContainerSafetyRestriction] NativeArray<int3> triangles;

        public void Setup(Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount)
        {
            NativeArray<VertexAttributeDescriptor> vertexAttributes = new(length: 4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

            vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, dimension: 3, stream: 0);
            vertexAttributes[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, dimension: 3, stream: 1);
            vertexAttributes[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, dimension: 4, stream: 2);
            vertexAttributes[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, dimension: 2, stream: 3);

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

            stream0 = meshData.GetVertexData<float3>(stream: 0);
            stream1 = meshData.GetVertexData<float3>(stream: 1);
            stream2 = meshData.GetVertexData<float4>(stream: 2);
            stream3 = meshData.GetVertexData<float2>(stream: 3);
            triangles = meshData.GetIndexData<int>().Reinterpret<int3>(4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex vertex)
        {
            stream0[index] = vertex.position;
            stream1[index] = vertex.normal;
            stream2[index] = vertex.tangent;
            stream3[index] = vertex.texCoord;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTriangle(int index, int3 triangle) => triangles[index] = triangle;
    }
}