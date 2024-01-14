using Assets.Scripts.Meshes.Streams;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Jobs
{
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct Normals : IJobFor
    {
        [NativeDisableContainerSafetyRestriction] public NativeArray<SingleStream.Stream> vertices;
        [ReadOnly] public NativeArray<int> triangles;

        public void Execute(int i)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            float3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);

            SingleStream.Stream verticeA = vertices[vertexIndexA];
            SingleStream.Stream verticeB = vertices[vertexIndexB];
            SingleStream.Stream verticeC = vertices[vertexIndexC];

            verticeA.normal += triangleNormal;
            verticeB.normal += triangleNormal;
            verticeC.normal += triangleNormal;

            vertices[vertexIndexA] = verticeA;
            vertices[vertexIndexB] = verticeB;
            vertices[vertexIndexC] = verticeC;
        }

        private float3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
        {
            float3 pointA = vertices[indexA].position;
            float3 pointB = vertices[indexB].position;
            float3 pointC = vertices[indexC].position;

            float3 sideAB = pointB - pointA;
            float3 sideAC = pointC - pointA;

            return math.normalize(math.cross(sideAB, sideAC));
        }

        public static JobHandle ScheduleParallel(Mesh.MeshData meshData, TerrainData terrainData, JobHandle dependency)
        {
            Normals job = new()
            {
                vertices = meshData.GetVertexData<SingleStream.Stream>(stream: 0),
                triangles = new NativeArray<int>(meshData.GetIndexData<int>(), Allocator.TempJob)
            };

            JobHandle handle = job.ScheduleParallel(terrainData.IndexCount / 3, 1, dependency);
            JobHandle disposeJob = job.triangles.Dispose(handle);
            return disposeJob;
        }

        public static JobHandle Schedule(Mesh.MeshData meshData, TerrainData terrainData, JobHandle dependency)
        {
            Normals job = new()
            {
                vertices = meshData.GetVertexData<SingleStream.Stream>(stream: 0),
                triangles = new NativeArray<int>(meshData.GetIndexData<int>(), Allocator.TempJob)
            };

            JobHandle handle = job.Schedule(terrainData.IndexCount / 3, dependency);
            JobHandle disposeJob = job.triangles.Dispose(handle);
            return disposeJob;
        }
    }
}