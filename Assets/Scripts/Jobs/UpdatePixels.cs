using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Jobs
{
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct UpdatePixels : IJobFor
    {
        [ReadOnly] public int resolution;
        [ReadOnly] public NativeArray<float> heightmap;
        [ReadOnly] private Color32 black;
        [ReadOnly] private Color32 white;
        [WriteOnly] public NativeArray<Color32> pixels;

        public void Execute(int i)
        {
            float noiseValue = math.clamp(heightmap[i], 0f, 1f);

            float r = math.lerp(black[0], white[0], noiseValue);
            float g = math.lerp(black[1], white[1], noiseValue);
            float b = math.lerp(black[2], white[2], noiseValue);

            Color32 color = new((byte)r, (byte)g, (byte)b, 255);

            pixels[i] = color;
        }

        public static void ScheduleParallel(TerrainData terrainData, NativeArray<Color32> pixels, JobHandle dependency)
        {
            UpdatePixels job = new()
            {
                resolution = terrainData.Resolution,
                heightmap = new(terrainData.Heightmap, Allocator.TempJob),
                black = new(0, 0, 0, 255),
                white = new(255, 255, 255, 255),
                pixels = pixels
            };

            job.ScheduleParallel(terrainData.Heightmap.Length, 1, dependency).Complete();
            terrainData.Heightmap = job.heightmap.ToArray();
            job.heightmap.Dispose();
        }
    }
}