using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class NoiseGenerator
    {
        private readonly NoiseData noiseData;
        private readonly TerrainData terrainData;

        private readonly ComputeShader noiseComputeShader;

        public NoiseGenerator(NoiseData noiseData, TerrainData terrainData, ComputeShader noiseComputeShader)
        {
            this.noiseData = noiseData;
            this.terrainData = terrainData;
            this.noiseComputeShader = noiseComputeShader;
        }

        public void Normalize(float newMinValue, float newMaxValue)
        {
            float range = MaxValue - MinValue;

            Parallel.For(0, terrainData.Heightmap.Length, i =>
            {
                terrainData.Heightmap[i] = (terrainData.Heightmap[i] - MinValue) / range * (newMaxValue - newMinValue) + newMinValue;
            });
        }

        public void Generate()
        {
            ComputeBuffer heightmapBuffer = new(terrainData.Heightmap.Length, sizeof(float));
            ComputeBuffer valueMinMaxBuffer = new(count: 2, sizeof(float));

            int kernelIndex = noiseComputeShader.FindKernel("CSMain");

            float[] valueMinMax = { float.MaxValue, float.MinValue };
            valueMinMaxBuffer.SetData(valueMinMax);

            noiseComputeShader.SetBuffer(kernelIndex, "heightmap", heightmapBuffer);
            noiseComputeShader.SetBuffer(kernelIndex, "valueMinMax", valueMinMaxBuffer);

            noiseComputeShader.SetInt("resolution", terrainData.Resolution);
            noiseComputeShader.SetInt("halfSize", terrainData.Size / 2);
            noiseComputeShader.SetFloat("scale", terrainData.Scale);

            noiseComputeShader.SetInt("noiseType", (int)noiseData.NoiseType);
            noiseComputeShader.SetInt("fractalType", (int)noiseData.FractalType);

            noiseComputeShader.SetInt("seed", noiseData.Seed);
            noiseComputeShader.SetInt("octaves", noiseData.Octaves);

            noiseComputeShader.SetFloat("lacunarity", noiseData.Lacunarity);
            noiseComputeShader.SetFloat("gain", noiseData.Gain);
            noiseComputeShader.SetFloat("frequency", noiseData.Frequency);

            noiseComputeShader.Dispatch(kernelIndex, terrainData.Resolution / 8, terrainData.Resolution / 8, 1);

            heightmapBuffer.GetData(terrainData.Heightmap);
            valueMinMaxBuffer.GetData(valueMinMax);

            MinValue = valueMinMax[0];
            MaxValue = valueMinMax[1];

            heightmapBuffer.Dispose();
            valueMinMaxBuffer.Dispose();
        }

        public float MaxValue { get; private set; }
        public float MinValue { get; private set; }
    }
}