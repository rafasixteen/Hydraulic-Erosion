using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Meshes
{
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct MeshIJobFor<G, S> : IJobFor where G : struct, IMeshGenerator where S : struct, IMeshStreams
    {
        private G generator;
        [WriteOnly] private S streams;

        public void Execute(int i) => generator.Execute(i, streams);

        public static JobHandle ScheduleParallel(Mesh mesh, Mesh.MeshData meshData, TerrainData terrainData, JobHandle dependency)
        {
            MeshIJobFor<G, S> job = new();

            job.generator.Resolution = terrainData.Resolution;
            job.generator.Size = terrainData.Size;
            job.generator.HeightMultipler = terrainData.HeightMultiplier;
            job.generator.Heightmap = new(terrainData.Heightmap, Allocator.TempJob);

            job.streams.Setup(meshData, mesh.bounds = job.generator.Bounds, job.generator.VertexCount, job.generator.IndexCount);

            JobHandle handle = job.ScheduleParallel(job.generator.JobLength, 1, dependency);
            JobHandle disposeJob = job.generator.Heightmap.Dispose(handle);
            return disposeJob;
        }
    }

    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct MeshIJob<G, S> : IJob where G : struct, ITerrainGenerator where S : struct, IMeshStreams
    {
        private G generator;
        [WriteOnly] private S streams;

        public void Execute() => generator.Execute(streams);

        public static void Schedule(Mesh mesh, Mesh.MeshData meshData, TerrainData terrainData, Vector3[] terrainMeshVertices, JobHandle dependency)
        {
            MeshIJob<G, S> job = new();

            job.generator.Resolution = terrainData.Resolution;
            job.generator.Size = terrainData.Size;
            job.generator.HeightMultipler = terrainData.HeightMultiplier;
            job.generator.Heightmap = new(terrainData.Heightmap, Allocator.TempJob);
            job.generator.TerrainMeshVertices = new(terrainMeshVertices, Allocator.TempJob);

            job.streams.Setup(meshData, mesh.bounds = job.generator.Bounds, job.generator.VertexCount, job.generator.IndexCount);
            
            job.Schedule(dependency).Complete();
            job.generator.Heightmap.Dispose();
            job.generator.TerrainMeshVertices.Dispose();
        }
    }
}