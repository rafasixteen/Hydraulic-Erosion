using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Erosion
    {
        private readonly NoiseData noiseData;
        private readonly TerrainData terrainData;
        private readonly ErosionData erosionData;

        private readonly ComputeShader erosionComputeShader;

        private readonly List<int> erosionBrushIndices = new();
        private readonly List<float> erosionBrushWeights = new();
        private int[] dropletPositions;

        private int currentSeed;
        private int currentResolution;
        private int currentErosionRadius;

        public Erosion(NoiseData noiseData, TerrainData terrainData, ErosionData erosionData, ComputeShader erosionComputeShader)
        {
            this.noiseData = noiseData;
            this.terrainData = terrainData;
            this.erosionData = erosionData;
            this.erosionComputeShader = erosionComputeShader;
        }

        private void Initialize(int mapSize)
        {
            if (currentSeed != noiseData.Seed)
            {
                Random.InitState(noiseData.Seed);
                currentSeed = noiseData.Seed;
            }

            if (currentResolution != terrainData.Resolution || currentErosionRadius != erosionData.ErosionRadius)
            {
                InitializeBrushIndices(mapSize, erosionData.ErosionRadius);
                currentResolution = terrainData.Resolution;
                currentErosionRadius = erosionData.ErosionRadius;
            }
        }

        private void InitializeBrushIndices(int mapSize, int erosionRadius)
        {
            erosionBrushIndices.Clear();
            erosionBrushWeights.Clear();

            float weightSum = 0;
            int index = 0;
            for (int brushY = -erosionRadius; brushY <= erosionRadius; brushY++)
            {
                for (int brushX = -erosionRadius; brushX <= erosionRadius; brushX++)
                {
                    float sqrDst = brushX * brushX + brushY * brushY;
                    if (sqrDst < erosionRadius * erosionRadius)
                    {
                        erosionBrushIndices.Add(brushY * mapSize + brushX);
                        float brushWeight = 1 - Mathf.Sqrt(sqrDst) / erosionRadius;
                        weightSum += brushWeight;
                        erosionBrushWeights.Add(brushWeight);
                        index++;
                    }
                }
            }

            for (int i = 0; i < erosionBrushWeights.Count; i++)
            {
                erosionBrushWeights[i] /= weightSum;
            }
        }

        private void GenerateDropletPositions(int iterations, int mapSize, int erosionRadius)
        {
            dropletPositions = new int[iterations];
            for (int i = 0; i < iterations; i++)
            {
                int randomX = Random.Range(erosionRadius, mapSize + erosionRadius);
                int randomY = Random.Range(erosionRadius, mapSize + erosionRadius);
                dropletPositions[i] = randomY * mapSize + randomX;
            }
        }

        public void Erode(int iterations)
        {
            int mapSizeWithBorder = terrainData.Resolution;
            int mapSize = mapSizeWithBorder - (erosionData.ErosionRadius * 2);

            Initialize(mapSize);
            GenerateDropletPositions(iterations, mapSize, erosionData.ErosionRadius);

            ComputeBuffer heightmapBuffer = new(terrainData.Heightmap.Length, sizeof(float), ComputeBufferType.Default);
            ComputeBuffer dropletPositionsBuffer = new(dropletPositions.Length, sizeof(int), ComputeBufferType.Default);
            ComputeBuffer erosionBrushIndicesBuffer = new(erosionBrushIndices.Count, sizeof(int), ComputeBufferType.Default);
            ComputeBuffer erosionBrushWeightsBuffer = new(erosionBrushWeights.Count, sizeof(float), ComputeBufferType.Default);
            heightmapBuffer.SetData(terrainData.Heightmap);
            dropletPositionsBuffer.SetData(dropletPositions);
            erosionBrushIndicesBuffer.SetData(erosionBrushIndices);
            erosionBrushWeightsBuffer.SetData(erosionBrushWeights);
            erosionComputeShader.SetBuffer(0, "heightmap", heightmapBuffer);
            erosionComputeShader.SetBuffer(0, "dropletPositions", dropletPositionsBuffer);
            erosionComputeShader.SetBuffer(0, "erosionBrushIndices", erosionBrushIndicesBuffer);
            erosionComputeShader.SetBuffer(0, "erosionBrushWeights", erosionBrushWeightsBuffer);

            erosionComputeShader.SetInt("resolution", mapSizeWithBorder);
            erosionComputeShader.SetInt("erosionBrushIndicesCount", erosionBrushIndices.Count);
            erosionComputeShader.SetInt("dropletsMaxLifetime", 30);
            erosionComputeShader.SetInt("erosionRadius", erosionData.ErosionRadius);
            erosionComputeShader.SetFloat("gravity", ErosionData.GRAVITY);
            erosionComputeShader.SetFloat("evaporationSpeed", erosionData.EvaporationSpeed);
            erosionComputeShader.SetFloat("inertia", erosionData.Inertia);
            erosionComputeShader.SetFloat("sedimentCapacityFactor", erosionData.SedimentCapacityFactor);
            erosionComputeShader.SetFloat("minimumSedimentCapacity", erosionData.MinimumSedimentCapacity);
            erosionComputeShader.SetFloat("erosionSpeed", erosionData.ErosionSpeed);
            erosionComputeShader.SetFloat("depositionSpeed", erosionData.DepositionSpeed);

            int threadX = (iterations + 1023) / 1024;
            erosionComputeShader.Dispatch(0, threadX, 1, 1);
            heightmapBuffer.GetData(terrainData.Heightmap);

            heightmapBuffer.Release();
            dropletPositionsBuffer.Release();
            erosionBrushIndicesBuffer.Release();
            erosionBrushWeightsBuffer.Release();
        }
    }
}